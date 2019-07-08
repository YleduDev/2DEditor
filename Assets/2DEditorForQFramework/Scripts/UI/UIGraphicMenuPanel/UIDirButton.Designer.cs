/****************************************************************************
 * 2019.5 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class UIDirButton
	{
		[SerializeField] public Image Image;
		[SerializeField] public Text txt;

		public void Clear()
		{
			Image = null;
			txt = null;
		}

		public override string ComponentName
		{
			get { return "UIDirButton";}
		}
	}
}
