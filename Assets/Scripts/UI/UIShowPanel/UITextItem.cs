/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using TDE;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace QFramework.TDE
{
    public partial class UITextItem : UIElement, IDragHandler, IBeginDragHandler, IPointerClickHandler
    {
        Vector2 offset;
        Vector2 localPoint;
        RectTransform parentRT;
        RectTransform rect;
        public T_Text text;
 

        internal void Init(T_Graphic graphicItem, Transform parent)
        {
            text = graphicItem as T_Text;
            EditorBoxInit(text);
            parentRT = parent as RectTransform;
            rect = transform as RectTransform;
            this.transform.Parent(parent)
                .Show()
                .LocalPosition(graphicItem.localPos.Value)
                .LocalScale(graphicItem.localScale.Value)
                .LocalRotation(graphicItem.locaRotation.Value);
            TextSubscribeInit();
        }

        //事件订阅处理
        void TextSubscribeInit()
        {
            //点击选中
            this.ApplySelfTo(self => self.text.isSelected.Subscribe(on=>{
                if (on) UIEditorBox.Show();else UIEditorBox.Hide();}))
                //移动
                .ApplySelfTo(self => self.text.localPos.Subscribe(v2 =>rect.LocalPosition(v2)))
                //大小
                .ApplySelfTo(self => self.text.localScale.Subscribe(v3 => rect.LocalScale(v3)))
                //旋转
                .ApplySelfTo(self => self.text.locaRotation.Subscribe(qua =>rect.LocalRotation(qua)))
                //宽
                .ApplySelfTo(self => self.text.height.Subscribe(f => rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, f)))
                //高
                .ApplySelfTo(self => self.text.widht.Subscribe(f =>rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, f)));
        }


        //待优化 设计方式不太理想
        private void EditorBoxInit(T_Graphic model)
        {
            UIRotate uRot = UIRotate.GetComponent<UIRotate>();
            uRot.Init(model, transform);

            UICornerDrag LeftDownUIDrag = UILeftDown.GetComponent<UICornerDrag>();
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
            //(1)将光标的屏幕坐标转换为世界坐标
            //RectTransformUtility.ScreenPointToWorldPointInRectangle(transform as RectTransform, eventData.position, eventData.pressEventCamera, out worldPoint);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, eventData.position, eventData.pressEventCamera, out localPoint);
            //(2)记录偏移量
            offset = (Vector2)transform.localPosition - localPoint;
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, eventData.position, eventData.pressEventCamera, out localPoint);
            text.localPos.Value = localPoint + offset;
        }

        //点击选中
        public void OnPointerClick(PointerEventData eventData)
        {
            Global.OnClick(text);
        }
        #endregion

        private void Awake(){ }

        protected override void OnBeforeDestroy(){}
    }
}