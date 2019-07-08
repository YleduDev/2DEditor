/****************************************************************************
 * 2019.5 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class Paragraph
	{
		[SerializeField] public Button topVerticalBtn;
		[SerializeField] public Image topVerticalImg;
		[SerializeField] public Button middleVerticalBtn;
		[SerializeField] public Image middleVerticalImg;
		[SerializeField] public Button bottomVerticalBtn;
		[SerializeField] public Image bottomVerticalImg;
		[SerializeField] public Button leftHorizontalBtn;
		[SerializeField] public Image leftHorizontalImg;
		[SerializeField] public Button centerHorizontalBtn;
		[SerializeField] public Image centerHorizontalImg;
		[SerializeField] public Button rightHorizontalBtn;
		[SerializeField] public Image rightHorizontalImg;
		[SerializeField] public Button flushHorizontalBtn;
		[SerializeField] public Image flushHorizontalImg;

		public void Clear()
		{
			topVerticalBtn = null;
			topVerticalImg = null;
			middleVerticalBtn = null;
			middleVerticalImg = null;
			bottomVerticalBtn = null;
			bottomVerticalImg = null;
			leftHorizontalBtn = null;
			leftHorizontalImg = null;
			centerHorizontalBtn = null;
			centerHorizontalImg = null;
			rightHorizontalBtn = null;
			rightHorizontalImg = null;
			flushHorizontalBtn = null;
			flushHorizontalImg = null;
		}

		public override string ComponentName
		{
			get { return "Paragraph";}
		}
	}
}
