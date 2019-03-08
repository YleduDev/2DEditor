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
	public partial class UITextItem : UIElement, IDragHandler, IBeginDragHandler,IPointerClickHandler
    {
        Vector3 offset;
        Vector3 worldPoint;
        public T_Text model;
        private void Awake()
		{
		}

		protected override void OnBeforeDestroy()
		{
		}

        internal void Init(T_Graphic graphicItem,Transform parent)
        {
            model = graphicItem as T_Text;
            UITextEditorBox.UITextRifhtDown.model = model;

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
            transform.position = worldPoint + offset;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Global.OnClick(model);
        }
    }
}