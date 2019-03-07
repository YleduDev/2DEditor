/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class UIImageContent
	{
		[SerializeField] public RectTransform LineParent;
		[SerializeField] public RectTransform TextParent;
		[SerializeField] public RectTransform ImageParent;

		public void Clear()
		{
			LineParent = null;
			TextParent = null;
			ImageParent = null;
		}

		public override string ComponentName
		{
			get { return "UIImageContent";}
		}
	}
}
