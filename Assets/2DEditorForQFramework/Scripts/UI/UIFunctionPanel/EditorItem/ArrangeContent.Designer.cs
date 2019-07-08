/****************************************************************************
 * 2019.4 DESKTOP-IVCS95Q
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class ArrangeContent
	{
		[SerializeField] public Button groupBtn;
		[SerializeField] public Button cancelGrpupBtn;
		[SerializeField] public Button alignBtn;
		[SerializeField] public Button upLayerBtn;
		[SerializeField] public Button downLayerBtn;
		[SerializeField] public Button bringToFrontBtn;
		[SerializeField] public Button atTheBottomBtn;

		public void Clear()
		{
			groupBtn = null;
			cancelGrpupBtn = null;
			alignBtn = null;
			upLayerBtn = null;
			downLayerBtn = null;
			bringToFrontBtn = null;
			atTheBottomBtn = null;
		}

		public override string ComponentName
		{
			get { return "ArrangeContent";}
		}
	}
}
