/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class UIImageEditorBox
	{
		[SerializeField] public UIImageRotate UIImageRotate;
		[SerializeField] public UIImageLeftUP UIImageLeftUP;
		[SerializeField] public UIImageRifhtUP UIImageRifhtUP;
		[SerializeField] public UIImageRifhtDown UIImageRifhtDown;
		[SerializeField] public UIImageLeftDown UIImageLeftDown;

		public void Clear()
		{
			UIImageRotate = null;
			UIImageLeftUP = null;
			UIImageRifhtUP = null;
			UIImageRifhtDown = null;
			UIImageLeftDown = null;
		}

		public override string ComponentName
		{
			get { return "UIImageEditorBox";}
		}
	}
}
