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
            //�༭����ʼ��
            EditorBoxInit(Image);
            //���߹��߳�ʼ��
            UILineSwitch.Init(model, LineParent as RectTransform, Image);
            ImageSubscribeInit();
        }

        // Model ֵ����
        void ImageSubscribeInit()
        {
            //���ѡ��
            this.ApplySelfTo(self => self.Image.isSelected.Subscribe(on =>{
                 if (on) { UIEditorBox.Show(); UILineSwitch.Show(); }
                 else { UIEditorBox.Hide(); UILineSwitch.Hide(); }}))
                //�ƶ�
                .ApplySelfTo(self => self.Image.localPos.Subscribe(
                 v2 => { self.Image.TransformChange(); rect.LocalPosition(v2); }))
                //��С
                .ApplySelfTo(self => self.Image.localScale.Subscribe(
                 v3 => rect.LocalScale(v3)))
                //��ת
                .ApplySelfTo(self => self.Image.locaRotation.Subscribe(
                 qua => { self.Image.TransformChange(); rect.LocalRotation(qua); }))
                //��
                .ApplySelfTo(self => self.Image.height.Subscribe(
                 f => { self.Image.TransformChange(); rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, f); }))
                 //��
                .ApplySelfTo(self => self.Image.widht.Subscribe(
                 f => { self.Image.TransformChange(); rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, f); }));
        }

        //���Ż� ��Ʒ�ʽ��̫����
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
            //(2)��¼ƫ����
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