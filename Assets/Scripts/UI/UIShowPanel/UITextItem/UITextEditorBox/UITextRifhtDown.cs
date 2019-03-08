/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UniRx;
using UnityEngine.EventSystems;
using TDE;

namespace QFramework.TDE
{
    public partial class UITextRifhtDown : UIElement, IDragHandler
    {
        public T_Text model;
        public ScaleCenter ScaleCenter= ScaleCenter.RightDown;
        public void OnDrag(PointerEventData eventData)
        {
            model.localPos.Value += (eventData.delta)*.5f;
            model.widht.Value += eventData.delta.x;
            model.height.Value += (-eventData.delta.y);
        }

        private void Awake()
        {
        }

        protected override void OnBeforeDestroy()
        {
        }
    }
}