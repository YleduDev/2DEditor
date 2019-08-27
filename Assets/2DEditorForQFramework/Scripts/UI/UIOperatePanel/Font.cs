/****************************************************************************
 * 2019.4 DESKTOP-IVCS95Q
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TDE;
using BindingsRx.Bindings;
using UniRx;

namespace QFramework.TDE
{
	public partial class Font : UIElement
	{

        public ReactiveProperty<Color> fontColor;
        
        private void Awake()
		{
            
        }

        public void Init(ColorItem colorItem)
        {
            BtnClickGlobal.fontDic.Add("Bold", boldImg);
            BtnClickGlobal.fontDic.Add("Italic", italicImg);


            


            fontColor = new ReactiveProperty<Color>(colorChangeImg.color);

            fontColor.Subscribe(color =>
            {
                BtnClickGlobal.AddRetreatSceneData();
                colorChangeImg.color = color;
                SetFontColor();
            });

            Dropdown fontType = fontTypeDropdown.GetComponent<Dropdown>();
            fontType.onValueChanged.AddListener(type =>
            {
                
            });
            Dropdown fontSize = fontSizeDropdown.GetComponent<Dropdown>();
            fontSize.onValueChanged.AddListener(value =>
            {
                BtnClickGlobal.AddRetreatSceneData();

                (Global.OnSelectedGraphic.Value as T_Text).fontSize.Value = int.Parse(fontSize.options[value].text);
            });

            BtnClickGlobal.textBtnsList.Add(fontType);
            BtnClickGlobal.textBtnsList.Add(fontSize);
            BtnClickGlobal.textBtnsList.Add(boldBtn);
            BtnClickGlobal.textBtnsList.Add(italicBtn);
            BtnClickGlobal.textBtnsList.Add(fontVerticalBtn);
            BtnClickGlobal.textBtnsList.Add(fontHorizontalBtn);
            BtnClickGlobal.textBtnsList.Add(fontColorBtn);
            BtnClickGlobal.textBtnsList.Add(colorShowBtn);
            BtnClickGlobal.textBtnsList.Add(specificFontBtn);


            boldBtn.onClick.AddListener(() =>
            {
                BtnClickGlobal.AddRetreatSceneData();
                SetFontStyle(FontStyleSelf.Bold);
            });

            italicBtn.onClick.AddListener(() =>
            {
                BtnClickGlobal.AddRetreatSceneData();

                SetFontStyle(FontStyleSelf.Italic);
            });

            fontVerticalBtn.onClick.AddListener(() =>
            {
                //BtnClickGlobal.AddRetreatSceneData();
            });

            fontHorizontalBtn.onClick.AddListener(() =>
            {
                //BtnClickGlobal.AddRetreatSceneData();
            });

            fontColorBtn.onClick.AddListener(SetFontColor);

            colorShowBtn.onClick.AddListener(() =>
            {
                colorItem.ColorItemShow(fontColor, colorShowBtn.transform.position);
            });
            
            specificFontBtn.onClick.AddListener(() =>
            {
                //Debug.Log((Global.OnSelectedGraphic.Value as T_Text).fontStyle);
                //Debug.Log((Global.OnSelectedGraphic.Value as T_Text).paragraphHorizontalAlignMentType);
                //Debug.Log((Global.OnSelectedGraphic.Value as T_Text).paragraphVerticalAlignmentType);
            });
        }

        void SetFontColor()
        {
            if (Global.OnSelectedGraphic.Value.IsNull()) return;
            (Global.OnSelectedGraphic.Value as T_Text).fontColor.Value =new  ColorSerializer(colorChangeImg.color);
        }

        void SetFontStyle(FontStyleSelf style)
        {
            if (Global.OnSelectedGraphic.Value.IsNull()) return;

            T_Text fontStyle = Global.OnSelectedGraphic.Value as T_Text;

            switch (fontStyle.fontStyle.Value)
            {
                case FontStyleSelf.Normal:
                    fontStyle.fontStyle.Value = style;
                    break;
                case FontStyleSelf.Bold:
                    if (style != FontStyleSelf.Bold)
                    {
                        fontStyle.fontStyle.Value = FontStyleSelf.BoldAndItalic;
                    }
                    else
                    {
                        fontStyle.fontStyle.Value = FontStyleSelf.Normal;
                    }
                    break;
                case FontStyleSelf.Italic:
                    if (style != FontStyleSelf.Italic)
                    {
                        fontStyle.fontStyle.Value = FontStyleSelf.BoldAndItalic;
                    }
                    else
                    {
                        fontStyle.fontStyle.Value = FontStyleSelf.Normal;
                    }
                    break;
                case FontStyleSelf.BoldAndItalic:
                    if (style == FontStyleSelf.Bold)
                    {
                        fontStyle.fontStyle.Value = FontStyleSelf.Italic;
                    }
                    else
                    {
                        fontStyle.fontStyle.Value = FontStyleSelf.Bold;
                    }
                    break;
                default:
                    break;
            }

        }


        protected override void OnBeforeDestroy()
		{
		}
	}
}