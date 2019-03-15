/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using TDE;
using UnityEngine;

namespace QFramework.TDE
{
    public partial class UILineSwitch : UIElement
	{
		private void Awake()
		{
		}

		protected override void OnBeforeDestroy()
		{
		}

        internal void Init(TSceneData model, RectTransform lineParent, T_Image image)
        {
            UILinePoint[] UILinePoints = GetComponentsInChildren<UILinePoint>();
            if (UILinePoints.IsNotNull() && UILinePoints.Length > 0) UILinePoints.ForEach(item => item.Init(model, lineParent, image));
        }
    }
}