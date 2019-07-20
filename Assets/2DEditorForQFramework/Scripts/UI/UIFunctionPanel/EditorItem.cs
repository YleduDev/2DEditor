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
	public partial class EditorItem : UIElement
	{
		private void Awake()
		{
            //restoreBtn.onClick.AddListener(RestoreBtnClick);
            //forwardBtn.onClick.AddListener(ForwardBtnClick);
            //retreatBtn.onClick.AddListener(retreatBtnClick);
            //copyBtn.onClick.AddListener(CopyBtnClick);
            //pasteBtn.onClick.AddListener(PasteBtnClick);
            //shearBtn.onClick.AddListener(ShearBtnClick);
            //deleteBtn.onClick.AddListener(RestoreBtnClick);
            //arrangeBtn.onClick.AddListener(ArrangeBtnClick);

            restoreBtn.onClick.AddListener(()=> 
            {
                RestoreBtnClick();
                this.Hide();
            });
            forwardBtn.onClick.AddListener(() =>
            {
                ForwardBtnClick();
                this.Hide();
            });
            retreatBtn.onClick.AddListener(() =>
            {
                RetreatBtnClick();
                this.Hide();
            });
            copyBtn.onClick.AddListener(() =>
            {
                CopyBtnClick();
                this.Hide();
            });
            pasteBtn.onClick.AddListener(() =>
            {
                PasteBtnClick();
                this.Hide();
            });
            shearBtn.onClick.AddListener(() =>
            {
                ShearBtnClick();
                this.Hide();
            });
            deleteBtn.onClick.AddListener(() =>
            {
                DeleteBtnClick();
                this.Hide();
            });
            arrangeBtn.onClick.AddListener(() =>
            {
                if (ArrangeContent.isActiveAndEnabled) ArrangeContent.Hide();else ArrangeContent.Show();
            });
        }

		protected override void OnBeforeDestroy()
		{
		}

        private void OnDisable()
        {
            if(ArrangeContent.isActiveAndEnabled)
            ArrangeContent.Hide();
        }

        #region EditorItem

        public static void RestoreBtnClick()
        {

        }

        public static void ForwardBtnClick()
        {

        }

        public static void RetreatBtnClick()
        {

        }


        public static bool isCopy = true;
        static Vector2 offsetPos = new Vector2(10, -10);
        public static void CopyBtnClick()
        {
            isCopy = true;
        }
       
        public static void PasteBtnClick()
        {
            BtnClickGlobal.AddRetreatSceneData();
            if (isCopy)
            {
                if (Global.OnSelectedGraphic.Value.IsNotNull())
                    Global.currentSceneData.Value.Add(CreateGraphic(Global.OnSelectedGraphic.Value));
            }
            else
            {
                if (Global.OnSelectedGraphic.Value.IsNotNull())
                    Global.OnSelectedGraphic.Value.localPos.Value += offsetPos;
            }
        }

        public static void ShearBtnClick()
        {
            isCopy = false;
        }

        public static void DeleteBtnClick()
        {
            Global.currentSceneData.Value.Remove(Global.OnSelectedGraphic.Value);
        }

        public static void ArrangeBtnClick()
        {
            
        }

        static T_Graphic CreateGraphic(T_Graphic graphic)
        {
            T_Graphic newGraphic;
            switch (graphic.graphicType)
            {
                case GraphicType.Image:
                    newGraphic = new T_Image((T_Image)graphic);
                    break;
                case GraphicType.Text:
                    newGraphic = new T_Text((T_Text)graphic);
                    break;
                case GraphicType.Line:
                    newGraphic = new T_Line((T_Line)graphic);
                    break;
                default:
                    return null;
            }
            newGraphic.localPos.Value += offsetPos;
            return newGraphic;
        }
        #endregion
    }
}