/****************************************************************************
 * 2019.5 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class Arrange
	{
		[SerializeField] public Button bringToFrontBtn;
		[SerializeField] public Button groupBtn;
		[SerializeField] public Button atTheBottomBtn;
		[SerializeField] public Button cancelGroupBtn;

		public void Clear()
		{
			bringToFrontBtn = null;
			groupBtn = null;
			atTheBottomBtn = null;
			cancelGroupBtn = null;
		}

		public override string ComponentName
		{
			get { return "Arrange";}
		}
	}
}
