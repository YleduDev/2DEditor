/****************************************************************************
 * 2019.4 DESKTOP-IVCS95Q
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class HelpItem
	{
		[SerializeField] public Button helpBtn;
		[SerializeField] public Image specification;

		public void Clear()
		{
			helpBtn = null;
			specification = null;
		}

		public override string ComponentName
		{
			get { return "HelpItem";}
		}
	}
}
