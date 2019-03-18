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
        //�߶λ�����ֱ���ۣ����ȣ�
        public ReactiveProperty<LineShapeType> lineShapeType = new ReactiveProperty<LineShapeType>(LineShapeType.Straight);
        //�߶���״����ʼ������״��
        public ReactiveProperty<LineBeginShape> lineBeginShapeType = new ReactiveProperty<LineBeginShape>(LineBeginShape.BeginLine);
        public ReactiveProperty<LineEndShape> lineEndShapeType = new ReactiveProperty<LineEndShape>(LineEndShape.EndLine);
        //
        public IntReactiveProperty px = new IntReactiveProperty();
        //�����յ�ľ���
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