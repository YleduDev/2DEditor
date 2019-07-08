/****************************************************************************
 * 2019.5 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class UIImageItem
	{
		[SerializeField] public RectTransform UIEditorBox;
		[SerializeField] public Image UIRotate;
		[SerializeField] public Image UILeftUP;
		[SerializeField] public Image UIRigghtUP;
		[SerializeField] public Image UIRightDown;
		[SerializeField] public Image UILeftDown;
		[SerializeField] public UILineSwitch UILineSwitch;

		public void Clear()
		{
			UIEditorBox = null;
			UIRotate = null;
			UILeftUP = null;
			UIRigghtUP = null;
			UIRightDown = null;
			UILeftDown = null;
			UILineSwitch = null;
		}

		public override string ComponentName
		{
			get { return "UIImageItem";}
		}
	}
}
