/****************************************************************************
 * 2019.4 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class UIDataToggle
	{
		[SerializeField] public Text Label;

		public void Clear()
		{
			Label = null;
		}

		public override string ComponentName
		{
			get { return "UIDataToggle";}
		}
	}
}
