/****************************************************************************
 * 2019.4 DESKTOP-IVCS95Q
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

using TDE;
namespace QFramework.TDE
{
	public partial class Paragraph : UIElement
	{

        public static Dictionary<string, Image> BtnsDic = new Dictionary<string, Image>();

		private void Awake()
		{

            
        }

        public void Init()
        {
            BtnClickGlobal.fontDic.Add("Top", topVerticalImg);
            BtnClickGlobal.fontDic.Add("Middle", middleVerticalImg);
            BtnClickGlobal.fontDic.Add("Bottom", bottomVerticalImg);
            BtnClickGlobal.fontDic.Add("Left", leftHorizontalImg);
            BtnClickGlobal.fontDic.Add("Center", centerHorizontalImg);
            BtnClickGlobal.fontDic.Add("Right", rightHorizontalImg);
            BtnClickGlobal.fontDic.Add("Flush", flushHorizontalImg);

            BtnClickGlobal.textBtnsList.Add(topVerticalBtn);
            BtnClickGlobal.textBtnsList.Add(middleVerticalBtn);
            BtnClickGlobal.textBtnsList.Add(bottomVerticalBtn);
            BtnClickGlobal.textBtnsList.Add(leftHorizontalBtn);
            BtnClickGlobal.textBtnsList.Add(centerHorizontalBtn);
            BtnClickGlobal.textBtnsList.Add(rightHorizontalBtn);
            BtnClickGlobal.textBtnsList.Add(flushHorizontalBtn);

            BtnsClickRegister();
        }

        void BtnsClickRegister()
        {
            topVerticalBtn.onClick.AddListener(() =>
            {
                SetParagraphVertivalAlignmentType(ParagraphVerticalType.Top);
            });
            middleVerticalBtn.onClick.AddListener(() =>
            {
               SetParagraphVertivalAlignmentType(ParagraphVerticalType.Middle);
            });
            bottomVerticalBtn.onClick.AddListener(() =>
            {
               SetParagraphVertivalAlignmentType(ParagraphVerticalType.Bottom);
            });
            leftHorizontalBtn.onClick.AddListener(() =>
            {
               SetParagraphHorizontalAlignmentType(ParagraphHorizontalType.Left);
            });
            centerHorizontalBtn.onClick.AddListener(() =>
            {
               SetParagraphHorizontalAlignmentType(ParagraphHorizontalType.Center);
            });
            rightHorizontalBtn.onClick.AddListener(() =>
            {
                SetParagraphHorizontalAlignmentType(ParagraphHorizontalType.Rigth);
            });
            flushHorizontalBtn.onClick.AddListener(() =>
            {
                SetParagraphHorizontalAlignmentType( ParagraphHorizontalType.Flush);
            });
        }

        public  void SetParagraphVertivalAlignmentType(ParagraphVerticalType verticalType)
        {

            if (Global.OnSelectedGraphic.Value.IsNull()) return;

            BtnClickGlobal.AddRetreatSceneData();

            (Global.OnSelectedGraphic.Value as T_Text).paragraphVerticalAlignmentType.Value = verticalType;
           // Debug.Log(verticalType);
        }

        public  void SetParagraphHorizontalAlignmentType(ParagraphHorizontalType horizontalType)
        {
            if (Global.OnSelectedGraphic.Value.IsNull()) return;

            BtnClickGlobal.AddRetreatSceneData();

            (Global.OnSelectedGraphic.Value as T_Text).paragraphHorizontalAlignMentType.Value = horizontalType;
            //Debug.Log(horizontalType);
        }

        protected override void OnBeforeDestroy()
		{
		}
	}
}