/****************************************************************************
 * 2019.5 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class ShapeStyle
	{
		[SerializeField] public Button fillColorBtn;
		[SerializeField] public Button fillColorShowBtn;
		[SerializeField] public Button borderColorBtn;
		[SerializeField] public Button borderColorShowBtn;

		public void Clear()
		{
			fillColorBtn = null;
			fillColorShowBtn = null;
			borderColorBtn = null;
			borderColorShowBtn = null;
		}

		public override string ComponentName
		{
			get { return "ShapeStyle";}
		}
	}
}
