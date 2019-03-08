/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class UIImageItem
	{
		[SerializeField] public UIImageEditorBox UIImageEditorBox;
		[SerializeField] public UILineSwitch UILineSwitch;

		public void Clear()
		{
			UIImageEditorBox = null;
			UILineSwitch = null;
		}

		public override string ComponentName
		{
			get { return "UIImageItem";}
		}
	}
}
