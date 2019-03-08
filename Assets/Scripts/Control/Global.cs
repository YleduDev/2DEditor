using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
namespace TDE
{
    public class Global : MonoBehaviour
    {
        //全局，表示当前选中的图元对象
        public static T_Graphic OnSelectedGraphic;

        /// <summary>
        /// ShowPanel 图元及面板点击事件
        /// </summary>
        /// <param name="graphic"></param>
        public static void OnClick(T_Graphic graphic=null)
        {
           if(OnSelectedGraphic.IsNotNull()) OnSelectedGraphic.isSelected.Value = false;
            if (graphic.IsNotNull())
            {
                graphic.isSelected.Value = true;
                OnSelectedGraphic = graphic;
            }
        }
    }
}