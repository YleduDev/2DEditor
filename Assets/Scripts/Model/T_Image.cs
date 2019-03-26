using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using System.Linq;
using QFramework;

namespace TDE
{
    //[Serializable]
    public class BindData
    {
        public T_Line line ;
        public LinePointType LinePointType;
        public Vector2ReactiveProperty LocalPointForImage;
        public float width;
        public float height;
    }
    //[Serializable]
    public class T_Image : T_Graphic
    {
        //绑点集合
        public ReactiveCollection<BindData>bindDatas=new ReactiveCollection<BindData>();

        public void Add(BindData data)
        {
            bindDatas.Add(data);
        }
        public void Remove(BindData data)
        {
            bindDatas.Remove(data);
        }
        //image对象矩阵改变
        public void TransformChange()
        {
            bindDatas.Where(data => data != null).ForEach(data => {

                float wDelte = widht.Value / data.width;
                float hDelte = height.Value / data.height;
                
                Vector2 newPos =Global.GetQuaternionForQS(locaRotation.Value)* new Vector2(data.LocalPointForImage.Value.x
                    * wDelte, data.LocalPointForImage.Value.y *hDelte);
                if (data.LinePointType == LinePointType.Origin) data.line.localOriginPos.Value 
                    = newPos+localPos.Value;
                else   data.line.localEndPos.Value = newPos+ localPos.Value;

            });
        }
    }
}