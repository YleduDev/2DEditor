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
    public partial class UIFill : UIElement
    {
        private Color colorLast;
        private void Awake()
        {
            InputField_transparent_Fill.onValueChanged.AddListener(OutputTransparent);
            Global.OnSelectedGraphic.Subscribe(data => { ChangeValue(); });
        }

        protected override void OnBeforeDestroy()
        {
        }

        protected void OnInit(QFramework.IUIData uiData)
        {

        }

        private void ChangeValue()
        {
            if (Global.OnSelectedGraphic.Value.IsNotNull())
            {
                if (Global.OnSelectedGraphic.Value.graphicType == GraphicType.Image)
                {
                    color_Fill.color = new Color
                        (Global.OnSelectedGraphic.Value.mainColor.Value.r,
                        Global.OnSelectedGraphic.Value.mainColor.Value.g,
                        Global.OnSelectedGraphic.Value.mainColor.Value.b,
                        Global.OnSelectedGraphic.Value.mainColor.Value.a);

                    InputField_transparent_Fill.text = ((Global.OnSelectedGraphic.Value.mainColor.Value.a)*100f).ToString();
                   
                }
            }
        }

        private void OutputWide(string wide)
        {
            if (Global.OnSelectedGraphic.Value.IsNotNull())
            {
                Global.OnSelectedGraphic.Value.widht.Value = float.Parse(InputField_wide_Fill.text);
            }
        }

        private void OutputTransparent(string transparent)
        {
            if (Global.OnSelectedGraphic.Value.IsNotNull())
            {
                Global.OnSelectedGraphic.Value.mainColor.Value =
                    new ColorSerializer(new Color(color_Fill.color.r, color_Fill.color.g, color_Fill.color.b, float.Parse(InputField_transparent_Fill.text)/100f));
            }
        }
    }
}