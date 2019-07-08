namespace QFramework.TDE
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    using QFramework;

    public class Roller : MonoBehaviour
    {
        public InputField currentNum;
        public Scrollbar transparent;

        void Start()
        {
            currentNum.text = (transparent.value * 100f).ToString();
        }

        public void ModifiedValue()
        {
            transparent.value = (float.Parse(currentNum.text)) / 100f;
        }

        public void ChangedValue()
        {
            currentNum.text = (transparent.value * 100f).ToString("f0");
        }
    }
}