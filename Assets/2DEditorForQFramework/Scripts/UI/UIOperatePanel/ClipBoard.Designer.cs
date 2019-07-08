/****************************************************************************
 * 2019.5 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class ClipBoard
	{
		[SerializeField] public Button pasteBtn;
		[SerializeField] public Button shearBtn;
		[SerializeField] public Button copyBtn;

		public void Clear()
		{
			pasteBtn = null;
			shearBtn = null;
			copyBtn = null;
		}

		public override string ComponentName
		{
			get { return "ClipBoard";}
		}
	}
}
