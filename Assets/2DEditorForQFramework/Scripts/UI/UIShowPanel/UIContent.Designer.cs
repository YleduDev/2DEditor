/****************************************************************************
 * 2019.5 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class UIContent
	{
		[SerializeField] public Button BGButton;
		[SerializeField] public RectTransform LineParent;
		[SerializeField] public RectTransform ImageParent;
		[SerializeField] public RectTransform TextParent;

		public void Clear()
		{
			BGButton = null;
			LineParent = null;
			ImageParent = null;
			TextParent = null;
		}

		public override string ComponentName
		{
			get { return "UIContent";}
		}
	}
}
