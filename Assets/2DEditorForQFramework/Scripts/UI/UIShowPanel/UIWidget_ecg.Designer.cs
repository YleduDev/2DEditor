/****************************************************************************
 * 2019.5 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class UIWidget_ecg
	{
		[SerializeField] public Image ImageAmmeter;
		[SerializeField] public Text Ammeter_value;
		[SerializeField] public Text ecgTitle;
		[SerializeField] public Text ElectricCurrenttTxt;
		[SerializeField] public Text A_voltage;
		[SerializeField] public Text B_voltage;
		[SerializeField] public Text C_voltage;
		[SerializeField] public Text A_electric;
		[SerializeField] public Text B_electric;
		[SerializeField] public Text C_electric;
		[SerializeField] public RectTransform UIEditorBox;
		[SerializeField] public Image UIRotate;
		[SerializeField] public Image UILeftUP;
		[SerializeField] public Image UIRigghtUP;
		[SerializeField] public Image UIRightDown;
		[SerializeField] public Image UILeftDown;

		public void Clear()
		{
			ImageAmmeter = null;
			Ammeter_value = null;
			ecgTitle = null;
			ElectricCurrenttTxt = null;
			A_voltage = null;
			B_voltage = null;
			C_voltage = null;
			A_electric = null;
			B_electric = null;
			C_electric = null;
			UIEditorBox = null;
			UIRotate = null;
			UILeftUP = null;
			UIRigghtUP = null;
			UIRightDown = null;
			UILeftDown = null;
		}

		public override string ComponentName
		{
			get { return "UIWidget_ecg";}
		}
	}
}
