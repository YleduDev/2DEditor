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
	public partial class UIImageItem : UIElement,IPointerEnterHandler,IPointerExitHandler,IDragHandler,IBeginDragHandler,IPointerClickHandler
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

        internal void Init(T_Graphic graphicItem,Transform parent)
        {
            model = graphicItem as T_Image;
            canvasRT = UIManager.Instance.RootCanvas.transform as RectTransform;
            this.transform.Parent(parent)
                .Show()
                .LocalPosition(graphicItem.localPos.Value)
                .LocalScale(graphicItem.localScale)
                .LocalRotation(Quaternion.Euler(graphicItem.locaEulerAngle));
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT, eventData.position, eventData.pressEventCamera, out localPoint);
            //(2)¼ÇÂ¼Æ«ÒÆÁ¿
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