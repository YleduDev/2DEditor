using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TDE
{
    /// <summary>
    /// 2维编辑中图像图像文本的基类
    /// Model
    /// </summary>
 public enum GraphicType
    {
        Image,
        Text,
        Line
    }

public class T_Graphic
{
        public Vector3 localPos;
        public Vector3 locaEulerAngle;
        public Vector3 localScale;

        //选中
        public bool isCheck;
        public Color mainColor;
        //渲染层级（0->n）
        public int siblingIndex;
        //是否框选
        public bool isSelected;

        public GraphicType graphicType= GraphicType .Image;
       
        //删除自身
        public virtual void DeleteSelf()
        {

        }
      

    }
}
