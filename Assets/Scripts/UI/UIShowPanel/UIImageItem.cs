/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/
using TDE;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
namespace QFramework.TDE
{
    public partial class UIImageItem : UIElement, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IPointerDownHandler
    {
        Vector2 offset;
        Vector2 localPoint;
        RectTransform parentRT;
        RectTransform rect;
        TSceneData model;

        private T_Image Image;     

        internal void Init(TSceneData model, T_Image graphicItem, Transform parent,Transform LineParent)
        {
            Image = graphicItem ;
            parentRT = parent as RectTransform;
            rect = transform as RectTransform;
            this.transform.Parent(parent)
                .Show()
                .LocalPosition(Image.localPos.Value)
                .LocalScale(Image.localScale.Value)
                .LocalRotation(Image.locaRotation.Value);
            //编辑面板初始化
            EditorBoxInit(Image);
            //划线工具初始化
            UILineSwitch.Init(model, LineParent as RectTransform, Image);
            ImageSubscribeInit();
        }

        // Model 值订阅
        void ImageSubscribeInit()
        {
            //点击选中
            this.ApplySelfTo(self => self.Image.isSelected.Subscribe(on =>{
                 if (on) { UIEditorBox.Show(); UILineSwitch.Show(); }
                 else { UIEditorBox.Hide(); UILineSwitch.Hide(); }}))
                //移动
                .ApplySelfTo(self => self.Image.localPos.Subscribe(
                 v2 => { self.Image.TransformChange(); rect.LocalPosition(v2); }))
                //大小
                .ApplySelfTo(self => self.Image.localScale.Subscribe(
                 v3 => rect.LocalScale(v3)))
                //旋转
                .ApplySelfTo(self => self.Image.locaRotation.Subscribe(
                 qua => { self.Image.TransformChange(); rect.LocalRotation(qua); }))
                //宽
                .ApplySelfTo(self => self.Image.height.Subscribe(
                 f => { self.Image.TransformChange(); rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, f); }))
                 //高
                .ApplySelfTo(self => self.Image.widht.Subscribe(
                 f => { self.Image.TransformChange(); rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, f); }));
        }

        //待优化 设计方式不太理想
        private void EditorBoxInit(T_Graphic model)
        {
            UIRotate uRot = UIRotate.GetComponent<UIRotate>();
            uRot.Init(model, transform);

            UICornerDrag LeftDownUIDrag= UILeftDown.GetComponent<UICornerDrag>();
            LeftDownUIDrag.Init(model, new Corner(UILeftUP.transform, UIRigghtUP.transform
                , UILeftDown.transform, UIRightDown.transform
                ));
            UICornerDrag LeftUpUIDrag = UILeftUP.GetComponent<UICornerDrag>();
            LeftUpUIDrag.Init(model, new Corner(UILeftUP.transform, UIRigghtUP.transform
                , UILeftDown.transform, UIRightDown.transform
                ));
            UICornerDrag RigghtUpUIDrag = UIRigghtUP.GetComponent<UICornerDrag>();
            RigghtUpUIDrag.Init(model, new Corner(UILeftUP.transform, UIRigghtUP.transform
                , UILeftDown.transform, UIRightDown.transform
                ));
            UICornerDrag RightDownUIDrag = UIRightDown.GetComponent<UICornerDrag>();
            RightDownUIDrag.Init(model, new Corner(UILeftUP.transform, UIRigghtUP.transform
                , UILeftDown.transform, UIRightDown.transform
                ));
        }

        #region MonoEvent
        public void OnBeginDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, eventData.position, eventData.pressEventCamera, out localPoint);
            //(2)记录偏移量
            offset = (Vector2)transform.localPosition - localPoint;
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, eventData.position, eventData.pressEventCamera, out localPoint);
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

        private void Awake(){}

        protected override void OnBeforeDestroy(){}
    }
}