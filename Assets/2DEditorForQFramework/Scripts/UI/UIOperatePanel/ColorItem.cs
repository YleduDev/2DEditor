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
	public partial class ColorItem : UIElement
	{
		private void Awake()
		{
            BtnsClickRegister();
        }


        T_Text text = Global.OnSelectedGraphic.Value as T_Text;

        void BtnsClickRegister()
        {
            colorBtnContent.GetComponentsInChildren<Button>()
                .ForEach(
                         item =>
                         {
                             item.onClick.AddListener(() =>
                             {
                                 if (changeColorTemp.IsNotNull())
                                 {
                                     changeColorTemp.Value = item.image.color;
                                 }
                                 this.Hide();
                             });
                         }
                        );
            selfMotionBtn.onClick.AddListener(() =>
            {
                changeColorTemp.Value = selfMotionImg.color;
                this.Hide();
            });

            otherColorBtn.onClick.AddListener(() =>
            {
                OtherColorPanel.transform.position = Vector3.zero;
                OtherColorPanel.Show();
            });

            ensureBtn.onClick.AddListener(() =>
            {
                changeColorTemp.Value = ColorShow.color;
                CloseItem();
            });

            cancelBtn.onClick.AddListener(() =>
            {
                CloseItem();
            });

            closeBtn.onClick.AddListener(() =>
            {
                CloseItem();
            });
        }

        void CloseItem()
        {
            OtherColorPanel.Hide();
            this.Hide();
        }


        private ReactiveProperty<Color> changeColorTemp;

        public void ColorItemShow(ReactiveProperty<Color> colorTemp, Vector3 pos)
        {
            changeColorTemp = colorTemp;
            this.transform.position = pos;
            this.Show();
        }

        protected override void OnBeforeDestroy()
		{
		}
	}
}