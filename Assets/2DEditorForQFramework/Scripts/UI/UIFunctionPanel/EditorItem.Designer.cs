/****************************************************************************
 * 2019.4 DESKTOP-IVCS95Q
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class EditorItem
	{
		[SerializeField] public ArrangeContent ArrangeContent;
		[SerializeField] public Button restoreBtn;
		[SerializeField] public Button forwardBtn;
		[SerializeField] public Button retreatBtn;
		[SerializeField] public Button copyBtn;
		[SerializeField] public Button pasteBtn;
		[SerializeField] public Button shearBtn;
		[SerializeField] public Button deleteBtn;
		[SerializeField] public Button arrangeBtn;

		public void Clear()
		{
			ArrangeContent = null;
			restoreBtn = null;
			forwardBtn = null;
			retreatBtn = null;
			copyBtn = null;
			pasteBtn = null;
			shearBtn = null;
			deleteBtn = null;
			arrangeBtn = null;
		}

		public override string ComponentName
		{
			get { return "EditorItem";}
		}
	}
}
