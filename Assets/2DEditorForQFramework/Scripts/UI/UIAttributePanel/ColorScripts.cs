using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TDE;

namespace QFramework.TDE
{
    public class ColorScripts : MonoBehaviour
    {
        public GameObject OtherColorPanel;
        public GameObject ColorItem;
        public Transform ColorsButten;

        private void Start()
        {
            
        }

        public void OtherColorShow()
        {
            OtherColorPanel.Show();
        }

        public void OtherColorHide()
        {
            OtherColorPanel.Hide();
        }
    }
}