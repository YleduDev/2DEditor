using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        public Vector3 localPos;
        public Vector3 locaEulerAngle;
        public Vector3 localScale;

        //ѡ��
        public bool isCheck;
        public Color mainColor;
        //��Ⱦ�㼶��0->n��
        public int siblingIndex;
        //�Ƿ��ѡ
        public bool isSelected;

        public GraphicType graphicType= GraphicType .Image;
       
        //ɾ������
        public virtual void DeleteSelf()
        {

        }
      

    }
}
