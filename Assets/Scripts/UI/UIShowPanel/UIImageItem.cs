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
        public T_Image model;
        private Vector3 offset;
        Vector3 worldPoint;
        public Texture2D hand; 
        private void Awake()
		{
		}

		protected override void OnBeforeDestroy()
		{
		}

        internal void Init(T_Graphic graphicItem,Transform parent)
        {
            model = graphicItem as T_Image;
            this.transform.Parent(parent)
                .Show()
                .LocalPosition(graphicItem.localPos.Value)
                .LocalScale(graphicItem.localScale)
                .LocalRotation(Quaternion.Euler(graphicItem.locaEulerAngle));
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            //(1)将光标的屏幕坐标转换为世界坐标
            RectTransformUtility.ScreenPointToWorldPointInRectangle(transform as RectTransform, eventData.position, eventData.pressEventCamera, out worldPoint);
            //(2)记录偏移量
            offset = transform.position - worldPoint;
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(transform as RectTransform, eventData.position, eventData.pressEventCamera, out worldPoint);
            //print(rtf + "   eventDataPos:" + eventDataPos+ "    eventData.pressEventCamera"+ eventData.pressEventCamera);
            transform.position = worldPoint + offset;
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