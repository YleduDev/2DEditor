
namespace QFramework.TDE
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    using QFramework;

    public class Resize : MonoBehaviour
    {
        public float num;
        public InputField currentNum;
        public Text placeholder;

        public void UpwardAdjustment()
        {
            if (placeholder.GetComponent<Text>().enabled)
            {
                float siz = float.Parse(placeholder.text);
                placeholder.text = (siz + num).ToString();
            }
            else
            {
                float siz = float.Parse(currentNum.text);
                currentNum.text = (siz + num).ToString();
            }
        }

        public void DownAdjustment()
        {
            if (placeholder.GetComponent<Text>().enabled)
            {
                float siz = float.Parse(placeholder.text);
                placeholder.text = (siz - num).ToString();
            }
            else
            {
                float siz = float.Parse(currentNum.text);
                currentNum.text = (siz - num).ToString();
            }
        }
    }
}
