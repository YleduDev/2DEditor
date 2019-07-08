/****************************************************************************
 * 2019.4 DESKTOP-IVCS95Q
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

using BindingsRx.Bindings;

namespace QFramework.TDE
{
    public partial class CanvasScalePanel : UIElement
    {
        private void Awake()
        {
        }

        string width = "1920";
        string height = "1080";


        public void Init(UIElement documentItem)
        {
            wideInputField.BindTextTo(() => width, text => width = text);
            heightInputField.BindTextTo(() => height, text => height = text);

            canvasEnsureBtn.onClick.AddListener(() =>
            {

                this.Hide();
                documentItem.Hide();
            });

            canvasCancelBtn.onClick.AddListener(() =>
            {

                this.Hide();
                documentItem.Hide();
            });
        }

        //public void RemoveAll()
        //{
        //    while (m_Model.ImageDataList.Count > 0 || m_Model.TextDataList.Count > 0 || m_Model.LineDataList.Count > 0)
        //    {
        //        for (int i = 0; i < m_Model.LineDataList.Count; i++)
        //        {
        //            m_Model.LineDataList.RemoveAt(i);
        //        }

        //        for (int i = 0; i < m_Model.ImageDataList.Count; i++)
        //        {
        //            m_Model.ImageDataList.RemoveAt(i);
        //        }
        //        for (int i = 0; i < m_Model.TextDataList.Count; i++)
        //        {
        //            m_Model.TextDataList.RemoveAt(i);
        //        }
        //    }
        //}

        protected override void OnBeforeDestroy()
        {
        }
    }
}