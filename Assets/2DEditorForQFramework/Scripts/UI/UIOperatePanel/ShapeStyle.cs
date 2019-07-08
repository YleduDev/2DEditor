/****************************************************************************
 * 2019.4 DESKTOP-IVCS95Q
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
	public partial class ShapeStyle : UIElement
	{
		private void Awake()
		{
		}

        public ReactiveProperty<Color> fillColor;
        public ReactiveProperty<Color> borderColor;

        public void Init(ColorItem colorItem)
        {
            fillColor = new ReactiveProperty<Color>(fillColorShowBtn.image.color);
            borderColor = new ReactiveProperty<Color>(borderColorShowBtn.image.color);

            fillColor.Subscribe(color=> 
            {
                fillColorShowBtn.image.color = color;
                SetFillColor();
            });


            borderColor.Subscribe(color =>
            {
                borderColorShowBtn.image.color = color;
            });


            fillColorBtn.onClick.AddListener(SetFillColor);

            fillColorShowBtn.onClick.AddListener(() =>
            {
                colorItem.ColorItemShow(fillColor, fillColorShowBtn.transform.position);
            });

            borderColorBtn.onClick.AddListener(() =>
            {
                Log.I("");
            });

            borderColorShowBtn.onClick.AddListener(() =>
            {
                colorItem.ColorItemShow(borderColor, borderColorShowBtn.transform.position);
            });
        }

        void SetFillColor()
        {
            if (Global.OnSelectedGraphic.Value.IsNotNull())
            {
                BtnClickGlobal.AddRetreatSceneData();
                Global.OnSelectedGraphic.Value.mainColor.Value = new ColorSerializer(fillColorShowBtn.image.color);
            }
        }

		protected override void OnBeforeDestroy()
		{
		}
	}
}