using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
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
        public Vector2ReactiveProperty  localPos;
        public Vector3 locaEulerAngle;
        public Vector3 localScale;

        public FloatReactiveProperty widht;
        public FloatReactiveProperty height;

        //选中
        public BoolReactiveProperty isSelected=new BoolReactiveProperty(false);
        public Color mainColor;
        //渲染层级（0->n）
        public int siblingIndex;
        //是否框选
        public BoolReactiveProperty isChecking=new BoolReactiveProperty(false);

        public GraphicType graphicType= GraphicType .Image;
       
        //删除自身
        public virtual void DeleteSelf()
        {

        }
      

    }
}
