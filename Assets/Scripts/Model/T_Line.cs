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

        public T_Image bindBeginImage;
        public T_Image bindEndImage;
        public BindData bindBeginData;
        public BindData bindEndData;

        public T_Line() : base()
        {
            graphicType = GraphicType.Line;
        }

    }
}