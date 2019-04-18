using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using System.Linq;
using QFramework;

namespace TDE
{
    //绑点数据（设计方式待优化）
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
       
        //要存放到SceneData
        public void Add(BindData data)
        {
            bindDatas.Add(data);
        }
        public void Remove(T_Line data, LinePointType type)
        {
            //foreach 不能对集合进行修改
            //bindDatas.Where(bindData => bindData != null).ForEach( item => { if (data.Equals(item.line) && type == item.LinePointType) bindDatas.Remove(item); });
            if (bindDatas.IsNull() || bindDatas.Count <= 0) return;
            for (int i = 0; i < bindDatas.Count; i++)
            {
                if (type == bindDatas[i].LinePointType && data.Equals(bindDatas[i].line))
                    bindDatas.Remove(bindDatas[i]);
            }
            
        }
        public void Remove(T_Line data)
        {
            bindDatas.Where(item => item.Equals(data) ).ForEach(item => bindDatas.Remove(item));
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
        //清除绑点信息
        protected void ClearBind()
        {
            bindDatas.Where(data => data != null).ForEach(data=>data.line.DeleteBindForImageData(this));

            bindDatas.Clear();
        }
        public override void Destroy()
        {
            ClearBind();
            base.Destroy();
        }
    }
}