/****************************************************************************
 * 2019.4 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class UIServerDataItem
	{
		[SerializeField] public Text txt;

		public void Clear()
		{
			txt = null;
		}

		public override string ComponentName
		{
			get { return "UIServerDataItem";}
		}
	}
}
