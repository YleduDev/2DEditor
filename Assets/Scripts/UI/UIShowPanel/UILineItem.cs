/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TDE;

namespace QFramework.TDE
{
	public partial class UILineItem : UIElement
	{
        public T_Line model;

        private void Awake()
		{
		}

		protected override void OnBeforeDestroy()
		{
		}
        internal void Init(T_Graphic graphicItem, RectTransform parent)
        {
            model = graphicItem as T_Line;
            this.transform.Parent(parent)
               .Show()
               .LocalPosition(model.localPos.Value)
               .LocalScale(model.localScale.Value)
               .LocalRotation(model.locaRotation.Value);
            RectTransform rect = transform as RectTransform;
        }
    }
}