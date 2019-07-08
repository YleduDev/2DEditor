/****************************************************************************
 * 2019.4 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class UILine
	{
		[SerializeField] public Button Line_Button;
		[SerializeField] public Image Line_Content;
		[SerializeField] public Toggle Toggle_noLine;
		[SerializeField] public Toggle Toggle_yLine;
		[SerializeField] public Image color;
		[SerializeField] public Button down;
		[SerializeField] public Scrollbar Scrollbar_transparent_Line;
		[SerializeField] public InputField InputField_wide_Line;
		[SerializeField] public Image Dropdown_frame_Line;
		[SerializeField] public Image Dropdown_frame_LineSize;
		[SerializeField] public Image Dropdown_frame_EndLine;
		[SerializeField] public Image Dropdown_frame_FrontLine;
		[SerializeField] public Image Mask_UILine;

		public void Clear()
		{
			Line_Button = null;
			Line_Content = null;
			Toggle_noLine = null;
			Toggle_yLine = null;
			color = null;
			down = null;
			Scrollbar_transparent_Line = null;
			InputField_wide_Line = null;
			Dropdown_frame_Line = null;
			Dropdown_frame_LineSize = null;
			Dropdown_frame_EndLine = null;
			Dropdown_frame_FrontLine = null;
			Mask_UILine = null;
		}

		public override string ComponentName
		{
			get { return "UILine";}
		}
	}
}
