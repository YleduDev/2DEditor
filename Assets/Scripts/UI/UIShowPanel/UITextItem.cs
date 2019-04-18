/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using TDE;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QFramework.TDE
{
    public partial class UITextItem : UIElement, IDragHandler, IBeginDragHandler, IPointerDownHandler
    {
        TSceneData model;
        Vector2 offset;
        Vector2 localPoint;
        RectTransform parentRT;
        RectTransform rect;
        public T_Text text;
        Image image;
        ResLoader loader = ResLoader.Allocate();

        internal void Init(TSceneData model, T_Graphic graphicItem)
        {
            this.model = model;
            text = graphicItem as T_Text;
            parentRT = Global.textParent;
            EditorBoxInit(text);
            rect = transform as RectTransform;
            this.transform.Parent(parentRT)
                .Show()
                .LocalPosition(graphicItem.localPos.Value)
                .LocalScale(graphicItem.localScale.Value)
                .LocalRotation(Global.GetQuaternionForQS(graphicItem.locaRotation.Value))
                .ApplySelfTo(self => image = GetComponent<Image>());
            TextSubscribeInit();
        }

        //事件订阅处理
        void TextSubscribeInit()
        {
            //点击选中
            this.ApplySelfTo(self => self.text.isSelected.Subscribe(on =>
            {
                if (on) { UIEditorBox?.Show();Global.OnSelectedGraphic.Value = text; } else UIEditorBox?.Hide();
            }))
                //移动
                .ApplySelfTo(self => self.text.localPos.Subscribe(v2 => rect.LocalPosition(v2)))
                //大小
                .ApplySelfTo(self => self.text.localScale.Subscribe(v3 => rect.LocalScale(v3)))
                //旋转
                .ApplySelfTo(self => self.text.locaRotation.Subscribe(qua => rect.LocalRotation(Global.GetQuaternionForQS(qua))))
                //宽
                .ApplySelfTo(self => self.text.height.Subscribe(f => rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, f)))
                //高
                .ApplySelfTo(self => self.text.widht.Subscribe(f => rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, f)))
                 //颜色
                 .ApplySelfTo(self => self.text.mainColor.Subscribe(color => image.color = Global.GetColorCS(color)))
                
                 .ApplySelfTo(self => self.text.spritrsStr.Subscribe(spriteName => {

                     if (!self.text.lastSpritrsStr.IsNullOrEmpty() && model.textrueReferenceDict.ContainsKey(self.text.lastSpritrsStr))
                     {
                         model.textrueReferenceDict[spriteName] -= 1;
                     }
                     Sprite sprite = null;
                     // 先查缓存
                     if (model.textrueDict.ContainsKey(spriteName))
                     {
                         byte[] buffer = Base64Helper.ConvertBase64(model.textrueDict[spriteName]);
                         Texture2D tex = new Texture2D(2, 2);
                         tex.LoadImage(buffer);
                         tex.Apply();
                         tex.name = spriteName;
                         sprite = Global.GetSprite(tex);

                         model.textrueReferenceDict[spriteName] += 1;
                     }
                     //缓存没有查本地
                     if (sprite.IsNull())
                         sprite = Global.GetSprite(spriteName);
                     image.sprite = sprite;
                 }))
                 .ApplySelfTo(self => self.text.AssetNodeData.Subscribe(data => { if (data.IsNotNull()) self.text.mainColor.Value = Global.GetColorForState(data); }));
        }


        //待优化 设计方式不太理想
        private void EditorBoxInit(T_Graphic model)
        {
            UIRotate uRot = UIRotate.GetComponent<UIRotate>();
            uRot.Init(model, transform);

            UICornerDrag LeftDownUIDrag = UILeftDown.GetComponent<UICornerDrag>();
            LeftDownUIDrag.Init(model, new Corner(UILeftUP.transform, UIRigghtUP.transform
                , UILeftDown.transform, UIRightDown.transform,parentRT
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
            //(1)将光标的屏幕坐标转换为世界坐标
            //RectTransformUtility.ScreenPointToWorldPointInRectangle(transform as RectTransform, eventData.position, eventData.pressEventCamera, out worldPoint);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, eventData.position, eventData.pressEventCamera, out localPoint);
            //(2)记录偏移量
            offset = (Vector2)transform.localPosition - localPoint;
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, eventData.position, eventData.pressEventCamera, out localPoint);
            if (!Global.GetLocalPointOnCanvas(localPoint)) return;
            text.localPos.Value = localPoint + offset;
        }

        //点击选中
        public void OnPointerDown(PointerEventData eventData)
        {
            Global.OnClick(text);
        }
        #endregion

        private void Awake(){ }

        protected override void OnBeforeDestroy(){
            loader.Recycle2Cache();
            loader = null;
        }
    }
}