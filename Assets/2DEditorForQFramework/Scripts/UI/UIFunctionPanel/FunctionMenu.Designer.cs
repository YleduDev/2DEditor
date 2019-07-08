/****************************************************************************
 * 2019.4 DESKTOP-IVCS95Q
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class FunctionMenu
	{
		[SerializeField] public Button documentBtn;
		[SerializeField] public Button editorBtn;
		[SerializeField] public Button pelBtn;
		[SerializeField] public Button canvasBtn;
		[SerializeField] public Button dataBtn;
		[SerializeField] public Button viewBtn;
		[SerializeField] public Button helpBtn;
		[SerializeField] public Button questionBtn;
		[SerializeField] public Button minBtn;
		[SerializeField] public Button maxBtn;
		[SerializeField] public Button quitBtn;

		public void Clear()
		{
			documentBtn = null;
			editorBtn = null;
			pelBtn = null;
			canvasBtn = null;
			dataBtn = null;
			viewBtn = null;
			helpBtn = null;
			questionBtn = null;
			minBtn = null;
			maxBtn = null;
			quitBtn = null;
		}

		public override string ComponentName
		{
			get { return "FunctionMenu";}
		}
	}
}
