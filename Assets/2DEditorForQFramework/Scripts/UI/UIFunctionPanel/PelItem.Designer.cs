/****************************************************************************
 * 2019.4 DESKTOP-IVCS95Q
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class PelItem
	{
		[SerializeField] public Button peLibraryBtn;
		[SerializeField] public Button insertPelBtn;
		[SerializeField] public Button deletePelBtn;
		[SerializeField] public Button batchImportBtn;
		[SerializeField] public Button batchExportBtn;

		public void Clear()
		{
			peLibraryBtn = null;
			insertPelBtn = null;
			deletePelBtn = null;
			batchImportBtn = null;
			batchExportBtn = null;
		}

		public override string ComponentName
		{
			get { return "PelItem";}
		}
	}
}
