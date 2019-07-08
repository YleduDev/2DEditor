/****************************************************************************
 * 2019.5 LAPTOP-R0ONNKOC
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
		[SerializeField] public Button DeletBtn;
		[SerializeField] public Image EditorContent;

		public void Clear()
		{
			Button = null;
			Text = null;
			DeletBtn = null;
			EditorContent = null;
		}

		public override string ComponentName
		{
			get { return "UIGraphicItem";}
		}
	}
}
