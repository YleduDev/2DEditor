using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
namespace TDE
{
    /// <summary>
    /// 2ά�༭��ͼ��ͼ���ı��Ļ���
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

        //ѡ��
        public BoolReactiveProperty isSelected=new BoolReactiveProperty(false);
        public Color mainColor;
        //��Ⱦ�㼶��0->n��
        public int siblingIndex;
        //�Ƿ��ѡ
        public BoolReactiveProperty isChecking=new BoolReactiveProperty(false);

        public GraphicType graphicType= GraphicType .Image;
       
        //ɾ������
        public virtual void DeleteSelf()
        {

        }
      

    }
}
