/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class UIGraphicItem
	{
		[SerializeField] public Button Button;
		[SerializeField] public Text Text;
		[SerializeField] public Image EditorContent;

		public void Clear()
		{
			Button = null;
			Text = null;
			EditorContent = null;
		}

		public override string ComponentName
		{
			get { return "UIGraphicItem";}
		}
	}
}
