/****************************************************************************
 * 2019.4 LAPTOP-R0ONNKOC
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TDE;
using UniRx;

namespace QFramework.TDE
{
	public partial class UITitle : UIElement
	{

        private void Awake()
		{
            Global.OnSelectedGraphic.Subscribe(data => { ChangeValue(); });
        }

		protected override void OnBeforeDestroy()
		{
		}

        private void ChangeValue()
        {
            if (Global.OnSelectedGraphic.Value.IsNotNull())
            {
                Dropdown_Title.GetComponent<Dropdown>().value = (int)Global.OnSelectedGraphic.Value.graphicType;
            }
        }

    }
}