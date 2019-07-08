/****************************************************************************
 * 2019.4 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class UITitle
	{
		[SerializeField] public Button Title_Button;
		[SerializeField] public Image Title_Content;
		[SerializeField] public Image Dropdown_Title;

		public void Clear()
		{
			Title_Button = null;
			Title_Content = null;
			Dropdown_Title = null;
		}

		public override string ComponentName
		{
			get { return "UITitle";}
		}
	}
}
