using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Newtonsoft.Json;

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
        public Vector2ReactiveProperty  localPos=new Vector2ReactiveProperty();
        [JsonIgnore]
        public QuaternionReactiveProperty locaRotation=new QuaternionReactiveProperty();
        public Vector3ReactiveProperty localScale=new Vector3ReactiveProperty(Vector3.one);

        public FloatReactiveProperty widht=new FloatReactiveProperty();
        public FloatReactiveProperty height=new FloatReactiveProperty();

        //ѡ��
        public BoolReactiveProperty isSelected=new BoolReactiveProperty(false);
        [JsonIgnore]
        public ColorReactiveProperty mainColor =new ColorReactiveProperty();
        //��Ⱦ�㼶��0->n��
        public int siblingIndex;
        //�Ƿ��ѡ
        public BoolReactiveProperty isChecking=new BoolReactiveProperty(false);

        public GraphicType graphicType= GraphicType .Image;
       
       
      

    }
}
