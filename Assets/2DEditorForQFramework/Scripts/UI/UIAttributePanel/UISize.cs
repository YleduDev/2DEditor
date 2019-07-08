/****************************************************************************
 * 2019.4 LAPTOP-R0ONNKOC
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TDE;
using UniRx;

namespace QFramework.TDE
{
    public partial class UISize : UIElement
    {
        private void Awake()
        {
            InputField_wide_Size.onValueChanged.AddListener(OutputWide);
            InputField_high_Size.onValueChanged.AddListener(OutputHigh);
            InputField_angle_Size.onValueChanged.AddListener(OutputAngle);
            Global.OnSelectedGraphic.Subscribe(data => { ChangeValue(); });
        }

        protected override void OnBeforeDestroy()
        {

        }

        private void ChangeValue()
        {
            if (Global.OnSelectedGraphic.Value.IsNotNull())
            {
                InputField_wide_Size.text = Global.OnSelectedGraphic.Value.widht.Value.ToString();
                InputField_high_Size.text = Global.OnSelectedGraphic.Value.height.Value.ToString();
                InputField_angle_Size.text = ((Global.OnSelectedGraphic.Value.locaRotation.Value.z )* 180f).ToString();
            }
        }

        private void OutputWide(string wide)
        {
            if (Global.OnSelectedGraphic.Value.IsNotNull())
            {
                Global.OnSelectedGraphic.Value.widht.Value = float.Parse(InputField_wide_Size.text);
            }
        }
        private void OutputHigh(string high)
        {
            if (Global.OnSelectedGraphic.Value.IsNotNull())
            {
                Global.OnSelectedGraphic.Value.height.Value = float.Parse(InputField_high_Size.text);
            }
        }
        private void OutputAngle(string angle)
        {
            if (Global.OnSelectedGraphic.Value.IsNotNull())
            {
                Global.OnSelectedGraphic.Value.locaRotation.Value = new QuaternionSerializer(
                    new Quaternion(Global.OnSelectedGraphic.Value.locaRotation.Value.x,
                    Global.OnSelectedGraphic.Value.locaRotation.Value.y,
                    (float.Parse(InputField_angle_Size.text)) / 180f,
                    Global.OnSelectedGraphic.Value.locaRotation.Value.w));
            }
        }
    }
}