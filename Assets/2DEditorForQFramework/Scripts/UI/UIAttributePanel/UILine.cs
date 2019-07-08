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
	public partial class UILine : UIElement
	{
		private void Awake()
		{
            Dropdown_frame_LineSize.GetComponent<Dropdown>().onValueChanged.AddListener(OutputLineSize);
            Global.OnSelectedGraphic.Subscribe(data => { ChangeValue(); });
        }

		protected override void OnBeforeDestroy()
		{

		}

        private void ChangeValue()
        {
            if (Global.OnSelectedGraphic.Value.IsNotNull())
            {
                if (Global.OnSelectedGraphic.Value.graphicType == GraphicType.Line)
                {
                    color.color = new Color
                        (Global.OnSelectedGraphic.Value.mainColor.Value.r,
                        Global.OnSelectedGraphic.Value.mainColor.Value.g,
                        Global.OnSelectedGraphic.Value.mainColor.Value.b,
                        Global.OnSelectedGraphic.Value.mainColor.Value.a);
                }
            }
        }

        public void OutputLineSize(int wide)
        {
            if (Global.OnSelectedGraphic.IsNotNull())
            {
                //Global.OnSelectedGraphic.Value. = Dropdown_frame_LineSize.GetComponent<Dropdown>().value;
            }
        }

    }
}