using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
namespace TDE
{
   // [Serializable]
    public class T_Line : T_Graphic
    {
        public T_Line() : base()
        {
            graphicType = GraphicType.Line;
            mainColor = new ReactiveProperty<ColorSerializer>(new ColorSerializer(Color.black));
        }
        public Vector2ReactiveProperty localEndPos = new Vector2ReactiveProperty();
        public Vector2ReactiveProperty localOriginPos = new Vector2ReactiveProperty();
        //线段画法（直，折，曲等）
        public ReactiveProperty<LineShapeType> lineShapeType = new ReactiveProperty<LineShapeType>(LineShapeType.Straight);
        //线段形状（起始各种形状）
        public ReactiveProperty<LineBeginShape> lineBeginShapeType = new ReactiveProperty<LineBeginShape>(LineBeginShape.BeginLine);
        public ReactiveProperty<LineEndShape> lineEndShapeType = new ReactiveProperty<LineEndShape>(LineEndShape.EndLine);
        //
        public IntReactiveProperty px = new IntReactiveProperty();
        //起点和终点的距离
        public float direction; 

        //数据后期待优化 TODO
        public T_Image bindBeginImage;
        public T_Image bindEndImage;
        //后期待优化
        protected void ClearBind()
        {
          //清除t_image 对象绑点数据            
          bindBeginImage?.Remove(this, LinePointType.Origin);
          bindEndImage?.Remove(this, LinePointType.End);
            
          bindBeginImage = null;
          bindEndImage = null;
        }
        public void  DeleteBindForImageData(T_Image bindImageData)
        {
            if (bindImageData != null && bindImageData.Equals(bindBeginImage)) bindBeginImage = null; 
            if (bindImageData != null && bindImageData.Equals(bindEndImage)) bindEndImage = null; 
        }

        public override void Destroy()
        {
            base.Destroy();
            ClearBind();
        }
        public override void ColorInit()
        {
            mainColor .Value = new ColorSerializer(Color.black);
        }
    }
}