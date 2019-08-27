/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using System.Text;
using TDE;
using TMPro;
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
        TextMeshProUGUI textPro;
        TMP_InputField tmp_InputField;
        //初始化绑点数据(只做一次）
        bool AssetNodeDataOnInit = false;
        ResLoader loader = ResLoader.Allocate();

        internal void Init(TSceneData model, T_Graphic graphicItem)
        {
            this.model = model;
            text = graphicItem as T_Text;
            parentRT = Global.textParent;
            EditorBoxInit(text);
            rect = transform as RectTransform;

            if (text.sceneLoaded.IsNotNull()) text.sceneLoaded = null;
            if (text.sceneSaveBefore.IsNotNull()) text.sceneSaveBefore = null;

            text.sceneLoaded += OnSceneLoadEnd;
            text.sceneSaveBefore += OnSceneBefore;

            this.transform.Parent(parentRT)
                .Show()
                .LocalPosition(graphicItem.localPos.Value)
                .LocalScale(graphicItem.localScale.Value)
                .LocalRotation(Global.GetQuaternionForQS(graphicItem.locaRotation.Value))
                .ApplySelfTo(self => image = GetComponent<Image>())
                .ApplySelfTo(self => textPro = Text.GetComponent<TextMeshProUGUI>())
                .ApplySelfTo(self => tmp_InputField = self.GetComponent<TMP_InputField>())
               .SetAsLastSibling();
            //.ApplySelfTo(self => tmp_InputField.onValueChanged.AddListener(str=> text.content.Value=str));
            tmp_InputField?.onEndEdit.AddListener(str => this.text.content.Value = str);
            TextSubscribeInit();
            AssetNodeDataOnInit = true;
        }

        //事件订阅处理
        void TextSubscribeInit()
        {
            //点击选中
            this.ApplySelfTo(self => self.text.isSelected.Subscribe(on =>
            {
                if (on) { if (self.UIEditorBox) self.UIEditorBox.Show(); }
                else { if (self.UIEditorBox) self.UIEditorBox.Hide(); }
            }))
                //移动
                .ApplySelfTo(self => self.text.localPos.Subscribe(v2 => self.LocalPosition(v2)))
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
                 //字体颜色
                 .ApplySelfTo(self => self.text.fontColor.Subscribe(color => textPro.color = Global.GetColorCS(color)))
                 //内容 
                 //ObserveOnMainThread 表示再主线程中调用 因为tmp_InputField插件中的方法不能异步执行（报错）
                 .ApplySelfTo(self => self.text.content.ObserveOnMainThread().Subscribe(str => { if (tmp_InputField) tmp_InputField.text = str; }))
                //sprite
                 .ApplySelfTo(self => self.text.spritrsStr.Subscribe(spriteName => {
                     if (!self.text.lastSpritrsStr.IsNullOrEmpty() && model.textrueReferenceDict.ContainsKey(self.text.lastSpritrsStr))
                     {
                         model.textrueReferenceDict[spriteName] -= 1;
                     }
                     Sprite sprite = Global.GetSprite(spriteName);
                     // 先查缓存
                     if (!sprite && model.textrueDict.ContainsKey(spriteName))
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
                     image.sprite = sprite;
                 }))
                //绑点数据
                 .ApplySelfTo(self => self.text.AssetNodeData.Subscribe(data => {
                     if (data.IsNotNull())
                     {
                         if (!AssetNodeDataOnInit)
                         {
                             ServerData.GetAssetNodeForID(data.Path, (str) =>
                             {
                                 if (!string.IsNullOrEmpty(str))
                                 {
                                     AssetNode asset = SerializeHelper.FromJson<AssetNode>(str);
                                     if (asset.IsNull()) { data = null; self.text.ColorInit(); }
                                     else
                                     {
                                         data.Id = asset.id;
                                         data.Data = asset.value;
                                         //Debug.Log(asset.valueStr);
                                         data.State = asset.state;
                                         data.Value= asset.value;
                                     }
                                 }
                                 //添加下面2端代码是因为 这个接口师协程 防止第一次获取初始值导致值获取的有误
                                 self.text.content.Value = data.Value;
                                 self.text.mainColor.Value = Global.GetColorForState(data);
                             });
                         }
                         self.text.content.Value = data.Value;
                         self.text.mainColor.Value = Global.GetColorForState(data);
                     }
                 }))
                 .ApplySelfTo(self => self.text.siblingType.Subscribe(
                     dataType =>
                     {
                         int index;
                         switch (dataType)
                         {
                             case SiblingEditorType.None: break;
                             case SiblingEditorType.UPOne:
                                 index = self.rect.GetSiblingIndex() + 1; self.rect.SetSiblingIndex(index); self.text.siblingType.Value = SiblingEditorType.None;
                                 break;
                             case SiblingEditorType.DonwOne:
                                 index = self.rect.GetSiblingIndex() - 1; self.rect.SetSiblingIndex(index); self.text.siblingType.Value = SiblingEditorType.None;
                                 break;
                             case SiblingEditorType.UpEnd:
                                 self.rect.SetAsLastSibling(); self.text.siblingType.Value = SiblingEditorType.None;
                                 break;
                             case SiblingEditorType.DonwEnd:
                                 self.rect.SetAsFirstSibling(); self.text.siblingType.Value = SiblingEditorType.None;
                                 break;
                         }
                     }));
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
        public void OnSceneLoadEnd()
        {
            this.rect.SetSiblingIndex(this.text.localSiblingIndex);

        }

        public void OnSceneBefore()
        {
            this.text.localSiblingIndex = rect.GetSiblingIndex();   //this.rect.SetSiblingIndex();
        }
    }
}