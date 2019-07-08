/****************************************************************************
 * 2019.4 LAPTOP-R0ONNKOC
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class UIServerDatasCloseButton : UIElement
	{
		private void Awake()
		{
            //透明不进行检测
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
		}

		protected override void OnBeforeDestroy()
		{
		}
	}
}