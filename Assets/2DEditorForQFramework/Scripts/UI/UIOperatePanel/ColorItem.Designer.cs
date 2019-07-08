/****************************************************************************
 * 2019.5 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class ColorItem
	{
		[SerializeField] public Image colorBtnContent;
		[SerializeField] public Button selfMotionBtn;
		[SerializeField] public Image selfMotionImg;
		[SerializeField] public Button otherColorBtn;
		[SerializeField] public Button OtherColorPanel;
		[SerializeField] public Image ColorShow;
		[SerializeField] public Button ensureBtn;
		[SerializeField] public Button cancelBtn;
		[SerializeField] public Button closeBtn;

		public void Clear()
		{
			colorBtnContent = null;
			selfMotionBtn = null;
			selfMotionImg = null;
			otherColorBtn = null;
			OtherColorPanel = null;
			ColorShow = null;
			ensureBtn = null;
			cancelBtn = null;
			closeBtn = null;
		}

		public override string ComponentName
		{
			get { return "ColorItem";}
		}
	}
}
