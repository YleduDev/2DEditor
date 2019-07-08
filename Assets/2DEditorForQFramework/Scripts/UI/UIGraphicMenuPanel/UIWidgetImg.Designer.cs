/****************************************************************************
 * 2019.5 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class UIWidgetImg
	{
		[SerializeField] public Image Image;
		[SerializeField] public Text txt;
		[SerializeField] public Button UIimgDeleteBtn;

		public void Clear()
		{
			Image = null;
			txt = null;
			UIimgDeleteBtn = null;
		}

		public override string ComponentName
		{
			get { return "UIWidgetImg";}
		}
	}
}
