/****************************************************************************
 * 2019.5 LAPTOP-R0ONNKOC
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class UIDirButton : UIElement
	{
		private void Awake()
		{
		}

		protected override void OnBeforeDestroy()
		{
		}

        internal void Init(string path,Action act)
        {
         Button button=GetComponent<Button>();
            if (button) button.onClick.AddListener(() => { PictureMgrForWindows.Instance.GetPather(path); act?.Invoke(); });
        }
    }
}