/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.EventSystems;

namespace QFramework.TDE
{
	public partial class UIimg : UIElement,IDragHandler,IBeginDragHandler,IEndDragHandler
	{
        Sprite sprite;

		private void Awake(){}

		protected override void OnBeforeDestroy(){}

        internal void Init(Sprite sprite)
        {
            this.sprite = sprite;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {

        }
        public void OnDrag(PointerEventData eventData)
        {
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
           
        }
    }
}