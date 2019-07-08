/****************************************************************************
 * 2019.4 DESKTOP-IVCS95Q
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class ViewItem
	{
		[SerializeField] public Button displayBtn;
		[SerializeField] public Button dispalyScaleBtn;
		[SerializeField] public Button windowBtn;
		[SerializeField] public Image displayContent;
		[SerializeField] public Button scalePlateBtn;
		[SerializeField] public Button gridBtn;
		[SerializeField] public Button referenceBtn;
		[SerializeField] public Image dispalyScaleContent;
		[SerializeField] public Button scaleBtn;
		[SerializeField] public Button windowScaleBtn;
		[SerializeField] public Image windowContent;
		[SerializeField] public Button newWindowBtn;
		[SerializeField] public Button reorderAllBtn;
		[SerializeField] public Button stackUpBtn;
		[SerializeField] public Button cutWindowBtn;

		public void Clear()
		{
			displayBtn = null;
			dispalyScaleBtn = null;
			windowBtn = null;
			displayContent = null;
			scalePlateBtn = null;
			gridBtn = null;
			referenceBtn = null;
			dispalyScaleContent = null;
			scaleBtn = null;
			windowScaleBtn = null;
			windowContent = null;
			newWindowBtn = null;
			reorderAllBtn = null;
			stackUpBtn = null;
			cutWindowBtn = null;
		}

		public override string ComponentName
		{
			get { return "ViewItem";}
		}
	}
}
