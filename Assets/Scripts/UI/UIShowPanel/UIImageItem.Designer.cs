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
		[SerializeField] public Image UILineSwitch;
		[SerializeField] public UIImageEditorBox UIImageEditorBox;

		public void Clear()
		{
			UILineSwitch = null;
			UIImageEditorBox = null;
		}

		public override string ComponentName
		{
			get { return "UIImageItem";}
		}
	}
}
