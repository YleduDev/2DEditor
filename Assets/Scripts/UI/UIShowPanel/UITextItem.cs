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
        Vector2 offset;
        Vector2 localPoint;
        RectTransform parentRT;
        RectTransform rect;
        public T_Text text;
        Image image;
        ResLoader loader = ResLoader.Allocate();

        internal void Init(T_Graphic graphicItem)
        {
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

        //�¼����Ĵ���
        void TextSubscribeInit()
        {
            //���ѡ��
            this.ApplySelfTo(self => self.text.isSelected.Subscribe(on =>
            {
                if (on) { UIEditorBox?.Show();Global.OnSelectedGraphic = text; } else UIEditorBox?.Hide();
            }))
                //�ƶ�
                .ApplySelfTo(self => self.text.localPos.Subscribe(v2 => rect.LocalPosition(v2)))
                //��С
                .ApplySelfTo(self => self.text.localScale.Subscribe(v3 => rect.LocalScale(v3)))
                //��ת
                .ApplySelfTo(self => self.text.locaRotation.Subscribe(qua => rect.LocalRotation(Global.GetQuaternionForQS(qua))))
                //��
                .ApplySelfTo(self => self.text.height.Subscribe(f => rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, f)))
                //��
                .ApplySelfTo(self => self.text.widht.Subscribe(f => rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, f)))
                 //��ɫ
                 .ApplySelfTo(self => self.text.mainColor.Subscribe(color => image.color = Global.GetColorQS(color)))
                
                 .ApplySelfTo(self => self.text.spritrsStr.Subscribe(spriteName => { image.sprite = loader.LoadSprite(spriteName); } ));
        }


        //���Ż� ��Ʒ�ʽ��̫����
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
            //(1)��������Ļ����ת��Ϊ��������
            //RectTransformUtility.ScreenPointToWorldPointInRectangle(transform as RectTransform, eventData.position, eventData.pressEventCamera, out worldPoint);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, eventData.position, eventData.pressEventCamera, out localPoint);
            //(2)��¼ƫ����
            offset = (Vector2)transform.localPosition - localPoint;
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, eventData.position, eventData.pressEventCamera, out localPoint);
            if (!Global.GetLocalPointOnCanvas(localPoint)) return;
            text.localPos.Value = localPoint + offset;
        }

        //���ѡ��
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