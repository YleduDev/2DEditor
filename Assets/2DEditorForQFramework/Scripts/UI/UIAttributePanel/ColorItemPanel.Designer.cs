/****************************************************************************
 * 2019.4 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class ColorItemPanel
	{
		[SerializeField] public Image colorButten;
		[SerializeField] public Button OtherColor;
		[SerializeField] public Image ColorShowButten;
		[SerializeField] public Button ensureButten;

		public void Clear()
		{
			colorButten = null;
			OtherColor = null;
			ColorShowButten = null;
			ensureButten = null;
		}

		public override string ComponentName
		{
			get { return "ColorItemPanel";}
		}
	}
}
