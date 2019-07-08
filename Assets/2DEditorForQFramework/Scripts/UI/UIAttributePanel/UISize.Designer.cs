/****************************************************************************
 * 2019.4 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class UISize
	{
		[SerializeField] public Button Size_Button;
		[SerializeField] public Image Size_Content;
		[SerializeField] public InputField InputField_wide_Size;
		[SerializeField] public InputField InputField_high_Size;
		[SerializeField] public InputField InputField_angle_Size;
		[SerializeField] public Image Mask_UISize;

		public void Clear()
		{
			Size_Button = null;
			Size_Content = null;
			InputField_wide_Size = null;
			InputField_high_Size = null;
			InputField_angle_Size = null;
			Mask_UISize = null;
		}

		public override string ComponentName
		{
			get { return "UISize";}
		}
	}
}
