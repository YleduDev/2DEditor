/****************************************************************************
 * 2019.4 DESKTOP-IVCS95Q
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class Tool : UIElement
	{
		private void Awake()
		{
		}
      public void Init()
       {
            linkedDataBtn.onClick.AddListener(()=>UIMgr.OpenPanel<UIServerDatasMenuPanel>());
#if UNITY_WEBGL
             linkedServerBtn.Hide();
#else
            linkedServerBtn.onClick.AddListener(() => UIMgr.OpenPanel<UILinkedServerPanel>());
#endif

        }
		protected override void OnBeforeDestroy()
		{
		}
	}
}