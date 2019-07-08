/****************************************************************************
 * 2019.5 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.TDE
{
	public partial class Font
	{
		[SerializeField] public Image fontTypeDropdown;
		[SerializeField] public Text Label;
		[SerializeField] public Image fontSizeDropdown;
		[SerializeField] public Button boldBtn;
		[SerializeField] public Image boldImg;
		[SerializeField] public Button italicBtn;
		[SerializeField] public Image italicImg;
		[SerializeField] public Button fontVerticalBtn;
		[SerializeField] public Image fontVerticalImg;
		[SerializeField] public Button fontHorizontalBtn;
		[SerializeField] public Image fontHorizontalImg;
		[SerializeField] public Button fontColorBtn;
		[SerializeField] public Image colorChangeImg;
		[SerializeField] public Button colorShowBtn;
		[SerializeField] public Button specificFontBtn;

		public void Clear()
		{
			fontTypeDropdown = null;
			Label = null;
			fontSizeDropdown = null;
			boldBtn = null;
			boldImg = null;
			italicBtn = null;
			italicImg = null;
			fontVerticalBtn = null;
			fontVerticalImg = null;
			fontHorizontalBtn = null;
			fontHorizontalImg = null;
			fontColorBtn = null;
			colorChangeImg = null;
			colorShowBtn = null;
			specificFontBtn = null;
		}

		public override string ComponentName
		{
			get { return "Font";}
		}
	}
}
