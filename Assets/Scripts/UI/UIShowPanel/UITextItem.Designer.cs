/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class UITextItem
	{
		[SerializeField] public UITextEditorBox UITextEditorBox;
		[SerializeField] public RectTransform Text;

		public void Clear()
		{
			UITextEditorBox = null;
			Text = null;
		}

		public override string ComponentName
		{
			get { return "UITextItem";}
		}
	}
}
