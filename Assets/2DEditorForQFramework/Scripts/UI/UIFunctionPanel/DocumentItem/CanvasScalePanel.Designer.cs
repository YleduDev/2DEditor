/****************************************************************************
 * 2019.4 DESKTOP-IVCS95Q
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class CanvasScalePanel
	{
		[SerializeField] public Button canvasEnsureBtn;
		[SerializeField] public InputField wideInputField;
		[SerializeField] public InputField heightInputField;
		[SerializeField] public Button canvasCancelBtn;
		[SerializeField] public Image canvasPrefab;

		public void Clear()
		{
			canvasEnsureBtn = null;
			wideInputField = null;
			heightInputField = null;
			canvasCancelBtn = null;
			canvasPrefab = null;
		}

		public override string ComponentName
		{
			get { return "CanvasScalePanel";}
		}
	}
}
