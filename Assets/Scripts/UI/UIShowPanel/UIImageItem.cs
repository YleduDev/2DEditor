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
    public partial class UIImageItem : UIElement, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IPointerDownHandler
    {
        Vector2 offset;
        Vector2 localPoint;
        RectTransform parentRT;
        RectTransform rect;
        TSceneData model;
        Image UIimage;

        private T_Image Image;     

        internal void Init(TSceneData model, T_Image graphicItem)
        {
            Image = graphicItem ;
            this.model = model;
            parentRT = Global.imageParent;
            rect = transform as RectTransform;
            this.transform.Parent(parentRT)
                .Show()
                .LocalPosition(Image.localPos.Value)
                .LocalScale(Image.localScale.Value)
                .LocalRotation(Global.GetQuaternionForQS(Image.locaRotation.Value))
                .ApplySelfTo(self => {UIimage = self.GetComponent<Image>(); });
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
                if (on) { self.UIEditorBox?.Show(); self.UILineSwitch?.Show(); Global.OnSelectedGraphic = Image; }
                else {  self.UIEditorBox.gameObject.Hide(); self.UILineSwitch?.Hide(); }
            }))
                //�ƶ�
                .ApplySelfTo(self => self.Image.localPos.Subscribe(
                 v2 => { self.Image.TransformChange(); rect.LocalPosition(v2); }))
                //��С
                .ApplySelfTo(self => self.Image.localScale.Subscribe(
                 v3 => rect.LocalScale(v3)))
                //��ת
                .ApplySelfTo(self => self.Image.locaRotation.Subscribe(
                 qua => { self.Image.TransformChange(); rect.LocalRotation(Global.GetQuaternionForQS(qua)); }))
                //��
                .ApplySelfTo(self => self.Image.height.Subscribe(
                 f => { self.Image.TransformChange(); rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, f); }))
                //��
                .ApplySelfTo(self => self.Image.widht.Subscribe(
                 f => { self.Image.TransformChange(); rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, f); }))
                //��ɫ
                 .ApplySelfTo(self => self.Image.mainColor.Subscribe(color=> UIimage.color = Global.GetColorQS(color)))
                 //Sprite
                 .ApplySelfTo(self => self.Image.spritrsStr.Subscribe(spriteName=> { UIimage.sprite = Global.GetSprite(spriteName); }));
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

        private void Awake(){}

        protected override void OnBeforeDestroy(){}
    }
}