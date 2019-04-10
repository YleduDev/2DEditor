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
        //�߶λ�����ֱ���ۣ����ȣ�
        public ReactiveProperty<LineShapeType> lineShapeType = new ReactiveProperty<LineShapeType>(LineShapeType.Straight);
        //�߶���״����ʼ������״��
        public ReactiveProperty<LineBeginShape> lineBeginShapeType = new ReactiveProperty<LineBeginShape>(LineBeginShape.BeginLine);
        public ReactiveProperty<LineEndShape> lineEndShapeType = new ReactiveProperty<LineEndShape>(LineEndShape.EndLine);
        //
        public IntReactiveProperty px = new IntReactiveProperty();
        //�����յ�ľ���
        public float direction; 

        //���ݺ��ڴ��Ż� TODO
        public T_Image bindBeginImage;
        public T_Image bindEndImage;
        //���ڴ��Ż�
        protected void ClearBind()
        {
          //���t_image ����������            
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