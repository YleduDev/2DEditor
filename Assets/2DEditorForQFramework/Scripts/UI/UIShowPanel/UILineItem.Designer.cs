/****************************************************************************
 * 2019.5 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class UILineItem
	{
		[SerializeField] public LineHead LineHead;
		[SerializeField] public LineSegment LineSegment;
		[SerializeField] public LineEnd LineEnd;

		public void Clear()
		{
			LineHead = null;
			LineSegment = null;
			LineEnd = null;
		}

		public override string ComponentName
		{
			get { return "UILineItem";}
		}
	}
}
