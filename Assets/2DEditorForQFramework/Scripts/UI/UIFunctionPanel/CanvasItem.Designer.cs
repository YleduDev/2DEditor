/****************************************************************************
 * 2019.4 DESKTOP-IVCS95Q
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class CanvasItem
	{
		[SerializeField] public Button canvasRorateBtn;
		[SerializeField] public Button canvasScaleBtn;
		[SerializeField] public Button canvasCutBtn;

		public void Clear()
		{
			canvasRorateBtn = null;
			canvasScaleBtn = null;
			canvasCutBtn = null;
		}

		public override string ComponentName
		{
			get { return "CanvasItem";}
		}
	}
}
