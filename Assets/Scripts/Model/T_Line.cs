using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
namespace TDE
{
    public class T_Line : T_Graphic
    {
        public Vector2ReactiveProperty localEndPos = new Vector2ReactiveProperty();
        public Vector2ReactiveProperty localOriginPos = new Vector2ReactiveProperty();
        public ReactiveProperty<LineShapeType> lineShapeType = new ReactiveProperty<LineShapeType>(LineShapeType.Straight);
        public ReactiveProperty<LineBothEndsShape> lineBeginShapeType = new ReactiveProperty<LineBothEndsShape>(LineBothEndsShape.Line);
        public ReactiveProperty<LineBothEndsShape> lineEndShapeType = new ReactiveProperty<LineBothEndsShape>(LineBothEndsShape.Line);

        public T_Line() : base()
        {
            graphicType = GraphicType.Line;
        }

    }
}