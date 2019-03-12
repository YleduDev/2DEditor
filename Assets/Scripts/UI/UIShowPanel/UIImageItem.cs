/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TDE;
using UnityEngine.EventSystems;

namespace QFramework.TDE
{
	public partial class UIImageItem : UIElement, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IPointerClickHandler
    {

        Vector2 offset;
        Vector2 localPoint;
        RectTransform canvasRT;

        public Texture2D hand;
        public T_Image model;
        private void Awake()
		{
		}

		protected override void OnBeforeDestroy()
		{
		}

        internal void Init(T_Graphic graphicItem, Transform parent)
        {
            model = graphicItem as T_Image;
            EditorBoxInit(model);
            canvasRT = UIManager.Instance.RootCanvas.transform as RectTransform;
            this.transform.Parent(parent)
                .Show()
                .LocalPosition(graphicItem.localPos.Value)
                .LocalScale(graphicItem.localScale.Value)
                .LocalRotation(graphicItem.locaRotation.Value);
        }


        //待优化 设计方式不太理想
        private void EditorBoxInit(T_Graphic model)
        {
            UIRotate uRot = UIRotate.GetComponent<UIRotate>();
            uRot.Init(model, transform);

            UIDrag LeftDownUIDrag= UILeftDown.GetComponent<UIDrag>();
            LeftDownUIDrag.Init(model, new Center(UILeftUP.transform, UIRigghtUP.transform
                , UILeftDown.transform, UIRightDown.transform
                ));
            UIDrag LeftUpUIDrag = UILeftUP.GetComponent<UIDrag>();
            LeftUpUIDrag.Init(model, new Center(UILeftUP.transform, UIRigghtUP.transform
                , UILeftDown.transform, UIRightDown.transform
                ));
            UIDrag RigghtUpUIDrag = UIRigghtUP.GetComponent<UIDrag>();
            RigghtUpUIDrag.Init(model, new Center(UILeftUP.transform, UIRigghtUP.transform
                , UILeftDown.transform, UIRightDown.transform
                ));
            UIDrag RightDownUIDrag = UIRightDown.GetComponent<UIDrag>();
            RightDownUIDrag.Init(model, new Center(UILeftUP.transform, UIRigghtUP.transform
                , UILeftDown.transform, UIRightDown.transform
                ));
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT, eventData.position, eventData.pressEventCamera, out localPoint);
            //(2)记录偏移量
            offset = (Vector2)transform.localPosition - localPoint;
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT, eventData.position, eventData.pressEventCamera, out localPoint);
            model.localPos.Value = localPoint + offset;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Cursor.SetCursor(hand, new Vector2(hand.width * .5f, hand.height * .5f), CursorMode.Auto);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Global.OnClick(model);
        }
    }
}