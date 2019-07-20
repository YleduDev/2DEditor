/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/
using System;
using TDE;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text;

namespace QFramework.TDE
{
    public partial class UIImageItem : UIElement, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IPointerDownHandler
    {
        Vector2 offset;
        Vector2 localPoint;
        RectTransform parentRT;
        RectTransform rect;
        TSceneData model;
        Image UIimage;

        //��ʼ���������(ֻ��һ�Σ�
        bool AssetNodeDataOnInit = false;

        private T_Image Image;
        ResLoader loader = ResLoader.Allocate();
        internal void Init(TSceneData model, T_Image graphicItem)
        {
            Image = graphicItem ;
            this.model = model;
            parentRT = Global.imageParent;
            rect = transform as RectTransform;
            
            if (Image.sceneLoaded.IsNotNull()) Image.sceneLoaded = null;
            if (Image.sceneSaveBefore.IsNotNull()) Image.sceneSaveBefore = null;
            Image.sceneLoaded += OnSceneLoadEnd;
            Image.sceneSaveBefore += OnSceneBefore;

            this.transform.Parent(parentRT)
                .Show()
                .LocalPosition(Image.localPos.Value)
                .LocalScale(Image.localScale.Value)
                .LocalRotation(Global.GetQuaternionForQS(Image.locaRotation.Value))
                .ApplySelfTo(self => { UIimage = self.GetComponent<Image>(); })
               .SetAsLastSibling();
            //�༭����ʼ��
            EditorBoxInit(Image);
            //���߹��߳�ʼ��
            UILineSwitch.Init(model, Image);
            ImageSubscribeInit();
        }

        // Model ֵ����
        void ImageSubscribeInit()
        {
            //���ѡ��
            this.ApplySelfTo(self => self.Image.isSelected.Subscribe(on =>
            {
                if (on) { self.UIEditorBox?.Show(); self.UILineSwitch?.Show(); }
                else { self.UIEditorBox.gameObject.Hide(); self.UILineSwitch?.Hide(); }
            }))
                //�ƶ�
                .ApplySelfTo(self => self.Image.localPos.Subscribe(
                 v2 => { self.Image.TransformChange(); self.LocalPosition(v2 ); }))
                //��С
                .ApplySelfTo(self => self.Image.localScale.Subscribe(
                 v3 => rect.LocalScale(v3)))
                //��ת
                .ApplySelfTo(self => self.Image.locaRotation.Subscribe(
                 qua => { self.Image.TransformChange(); self.rect.LocalRotation(Global.GetQuaternionForQS(qua)); }))
                //gao
                .ApplySelfTo(self => self.Image.height.Subscribe(
                 f => { self.Image.TransformChange(); self.rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, f); }))
                //kuan
                .ApplySelfTo(self => self.Image.widht.Subscribe(
                 f => { self.Image.TransformChange(); self.rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, f); }))
                 //��ɫ
                 .ApplySelfTo(self => self.Image.mainColor.Subscribe(color => UIimage.color = Global.GetColorCS(color)))
                 //Sprite
                 .ApplySelfTo(self => self.Image.spritrsStr.Subscribe(spriteName =>
                 {
                     // Log.I(spriteName);
                     //��һ���Ƿ���Ҫ���������
                     if (!self.Image.spritrsStr.Value.Equals(spriteName) && model.textrueDict.ContainsKey(self.Image.spritrsStr.Value))
                     {
                         model.textrueReferenceDict[self.Image.spritrsStr.Value] -= 1;
                     }
                     Sprite sprite = Global.GetSprite(spriteName);

                     // �Ȳ黺��
                     if (sprite.IsNull() && model.textrueDict.ContainsKey(spriteName))
                     {
                         byte[] buffer = Base64Helper.ConvertBase64(model.textrueDict[spriteName]);
                         Texture2D tex = new Texture2D(2, 2);
                         tex.LoadImage(buffer);
                         tex.Apply();
                         tex.name = spriteName;
                         sprite = Global.GetSprite(tex);

                         model.textrueReferenceDict[spriteName] += 1;
                     }
                     else if (!sprite.IsNull() && model.textrueDict.ContainsKey(spriteName)) model.textrueReferenceDict[spriteName] += 1;
                     //����û�в鱾��                                        
                     UIimage.sprite = sprite;
                 }))
                 // .ApplySelfTo(self => self.Image.ColorInit())
                 //.ApplySelfTo(self => self.Image.AssetNodeData = new ReactiveProperty<WebSocketMessage>())
                 .ApplySelfTo(self => self.Image.AssetNodeData.Subscribe(data =>
                 {
                     if (data.IsNotNull())
                     {

                         if (!AssetNodeDataOnInit)
                         {
                             ServerData.GetAssetNodeForID(data.Id, (str) =>
                             {
                                 if (!string.IsNullOrEmpty(str))
                                 {
                                     AssetNode asset = SerializeHelper.FromJson<AssetNode>(str);
                                     if (asset.IsNull()) { data = null; self.Image.ColorInit(); }
                                     else
                                     {
                                         data.Data = asset.valueStr;
                                         data.State = asset.state;
                                     }
                                 }
                             });
                             AssetNodeDataOnInit = true;
                         }
                         self.Image.mainColor.Value = Global.GetColorForState(data);
                     }
                     else AssetNodeDataOnInit = true;
                 }))
                 .ApplySelfTo(self => self.Image.siblingType.Subscribe(
                     dataType =>
                     {
                         int index;
                         switch (dataType)
                         {
                             case SiblingEditorType.None:break;
                             case SiblingEditorType.UPOne:
                                 index = self.rect.GetSiblingIndex() + 1; self.rect.SetSiblingIndex(index); self.Image.siblingType.Value = SiblingEditorType.None;
                                 break;
                             case SiblingEditorType.DonwOne:
                                 index = self.rect.GetSiblingIndex() - 1; self.rect.SetSiblingIndex(index); self.Image.siblingType.Value = SiblingEditorType.None;
                                 break;
                             case SiblingEditorType.UpEnd:
                                 self.rect.SetAsLastSibling(); self.Image.siblingType.Value = SiblingEditorType.None;
                                 break;
                             case SiblingEditorType.DonwEnd:
                                 self.rect.SetAsFirstSibling(); self.Image.siblingType.Value = SiblingEditorType.None;
                                 break;
                         }
                     }));
        }
        
        //���Ż� ��Ʒ�ʽ��̫����
        private void EditorBoxInit(T_Graphic model)
        {
            UIRotate uRot = UIRotate.GetComponent<UIRotate>();
            uRot.Init(model, transform);

            UICornerDrag LeftDownUIDrag= UILeftDown.GetComponent<UICornerDrag>();
            LeftDownUIDrag.Init(model, new Corner(UILeftUP.transform, UIRigghtUP.transform
                , UILeftDown.transform, UIRightDown.transform, parentRT
                ));
            UICornerDrag LeftUpUIDrag = UILeftUP.GetComponent<UICornerDrag>();
            LeftUpUIDrag.Init(model, new Corner(UILeftUP.transform, UIRigghtUP.transform
                , UILeftDown.transform, UIRightDown.transform, parentRT
                ));
            UICornerDrag RigghtUpUIDrag = UIRigghtUP.GetComponent<UICornerDrag>();
            RigghtUpUIDrag.Init(model, new Corner(UILeftUP.transform, UIRigghtUP.transform
                , UILeftDown.transform, UIRightDown.transform, parentRT
                ));
            UICornerDrag RightDownUIDrag = UIRightDown.GetComponent<UICornerDrag>();
            RightDownUIDrag.Init(model, new Corner(UILeftUP.transform, UIRigghtUP.transform
                , UILeftDown.transform, UIRightDown.transform, parentRT
                ));
        }

        #region MonoEvent
        public void OnBeginDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, eventData.position, eventData.pressEventCamera, out localPoint);
            //(2)��¼ƫ����
            offset = (Vector2)transform.localPosition - localPoint;
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, eventData.position, eventData.pressEventCamera, out localPoint);
            if (!Global.GetLocalPointOnCanvas(localPoint)) return;
            Image.localPos.Value = localPoint + offset;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            UILineSwitch.Show();
       }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(!Image.isSelected.Value)
            UILineSwitch.Hide();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            Global.OnClick(Image);
        }
        #endregion

        public void OnSceneLoadEnd()
        {
            this.rect.SetSiblingIndex(this.Image.localSiblingIndex);
        
        }

        public void OnSceneBefore()
        {
            this.Image.localSiblingIndex = rect.GetSiblingIndex();   //this.rect.SetSiblingIndex();
    
        }

        private void Awake(){}

        protected override void OnBeforeDestroy(){
            loader.Recycle2Cache();
            loader = null;
        }
    }
}