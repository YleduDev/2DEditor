/****************************************************************************
 * 2019.4 DESKTOP-IVCS95Q
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class DocumentItem
	{
		[SerializeField] public Button newCreateBtn;
		[SerializeField] public Button openBtn;
		[SerializeField] public Button RecentlyOpenBtn;
		[SerializeField] public Button saveBtn;
		[SerializeField] public Button saveAsBtn;
		[SerializeField] public Button uploadBtn;
		[SerializeField] public Button printBtn;
		[SerializeField] public Button quitBtn;
		[SerializeField] public Image recentlyOpenContent;
		[SerializeField] public Button ClearDocuments;
		[SerializeField] public CanvasScalePanel CanvasScalePanel;

		public void Clear()
		{
			newCreateBtn = null;
			openBtn = null;
			RecentlyOpenBtn = null;
			saveBtn = null;
			saveAsBtn = null;
			uploadBtn = null;
			printBtn = null;
			quitBtn = null;
			recentlyOpenContent = null;
			ClearDocuments = null;
			CanvasScalePanel = null;
		}

		public override string ComponentName
		{
			get { return "DocumentItem";}
		}
	}
}
