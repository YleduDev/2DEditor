/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class UITextEditorBox
	{
		[SerializeField] public UITextRotate UITextRotate;
		[SerializeField] public UITextLeftUP UITextLeftUP;
		[SerializeField] public UITextRifhtUP UITextRifhtUP;
		[SerializeField] public UITextRifhtDown UITextRifhtDown;
		[SerializeField] public UIImageLeftDown UITextLeftDown;

		public void Clear()
		{
			UITextRotate = null;
			UITextLeftUP = null;
			UITextRifhtUP = null;
			UITextRifhtDown = null;
			UITextLeftDown = null;
		}

		public override string ComponentName
		{
			get { return "UITextEditorBox";}
		}
	}
}
