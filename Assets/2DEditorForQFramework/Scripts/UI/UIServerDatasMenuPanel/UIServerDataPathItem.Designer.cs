/****************************************************************************
 * 2019.5 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class UIServerDataPathItem
	{
		[SerializeField] public Button Button;
		[SerializeField] public Text Text;
		[SerializeField] public UIServerDataContent UIServerDataContent;

		public void Clear()
		{
			Button = null;
			Text = null;
			UIServerDataContent = null;
		}

		public override string ComponentName
		{
			get { return "UIServerDataPathItem";}
		}
	}
}
