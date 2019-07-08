/****************************************************************************
 * 2019.5 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class Operation
	{
		[SerializeField] public Button saveBtn;
		[SerializeField] public Button restoreBtn;
		[SerializeField] public Button deleteBtn;
		[SerializeField] public Button forwardBtn;
		[SerializeField] public Button uploadBtn;
		[SerializeField] public Button retreatBtn;

		public void Clear()
		{
			saveBtn = null;
			restoreBtn = null;
			deleteBtn = null;
			forwardBtn = null;
			uploadBtn = null;
			retreatBtn = null;
		}

		public override string ComponentName
		{
			get { return "Operation";}
		}
	}
}
