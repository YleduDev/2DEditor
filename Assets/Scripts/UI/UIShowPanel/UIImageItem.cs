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
	public partial class UIImageItem : UIElement
	{
		private void Awake()
		{
		}

		protected override void OnBeforeDestroy()
		{
		}

        internal void Init(T_Graphic graphicItem,Transform parent)
        {
            this.transform.Parent(parent).Show().LocalPosition(graphicItem.localPos)
                .LocalScale(graphicItem.localScale)
                .LocalRotation(Quaternion.Euler(graphicItem.locaEulerAngle));
        }
    }
}