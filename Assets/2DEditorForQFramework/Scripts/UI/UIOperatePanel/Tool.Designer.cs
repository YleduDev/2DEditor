/****************************************************************************
 * 2019.5 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class Tool
	{
		[SerializeField] public Button selectBtn;
		[SerializeField] public Button linkedDataBtn;
		[SerializeField] public Button linkedDataShowBtn;
		[SerializeField] public Button updatePelBtn;
		[SerializeField] public Button alignBtn;
		[SerializeField] public Button alignShowBtn;
		[SerializeField] public Button linkedServerBtn;

		public void Clear()
		{
			selectBtn = null;
			linkedDataBtn = null;
			linkedDataShowBtn = null;
			updatePelBtn = null;
			alignBtn = null;
			alignShowBtn = null;
			linkedServerBtn = null;
		}

		public override string ComponentName
		{
			get { return "Tool";}
		}
	}
}
