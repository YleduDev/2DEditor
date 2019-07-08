/****************************************************************************
 * 2019.4 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class UIFill
	{
		[SerializeField] public Button Fill_Button;
		[SerializeField] public Image Fill_Content;
		[SerializeField] public Toggle Toggle_noFill;
		[SerializeField] public Toggle Toggle_yFill;
		[SerializeField] public Image color_Fill;
		[SerializeField] public Button down_Fill;
		[SerializeField] public Scrollbar Scrollbar_transparent_Fill;
		[SerializeField] public InputField InputField_transparent_Fill;
		[SerializeField] public Toggle Toggle_noFrame;
		[SerializeField] public Toggle Toggle_yFrame;
		[SerializeField] public InputField InputField_wide_Fill;
		[SerializeField] public Image color_Frame;
		[SerializeField] public Button down_Frame;
		[SerializeField] public Scrollbar Scrollbar_transparent_Frame;
		[SerializeField] public Image Dropdown_frame_agg;
		[SerializeField] public InputField InputField_angle_Fill;
		[SerializeField] public Image Mask_UIFill;

		public void Clear()
		{
			Fill_Button = null;
			Fill_Content = null;
			Toggle_noFill = null;
			Toggle_yFill = null;
			color_Fill = null;
			down_Fill = null;
			Scrollbar_transparent_Fill = null;
			InputField_transparent_Fill = null;
			Toggle_noFrame = null;
			Toggle_yFrame = null;
			InputField_wide_Fill = null;
			color_Frame = null;
			down_Frame = null;
			Scrollbar_transparent_Frame = null;
			Dropdown_frame_agg = null;
			InputField_angle_Fill = null;
			Mask_UIFill = null;
		}

		public override string ComponentName
		{
			get { return "UIFill";}
		}
	}
}
