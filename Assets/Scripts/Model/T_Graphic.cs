using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Newtonsoft.Json;
using QFramework;

namespace TDE
{

    [Serializable]
    public struct QuaternionSerializer
    {
        public float x;
        public float y;
        public float z;
        public float w;
        public QuaternionSerializer(Quaternion q)
        { x = q.x; y = q.y; z = q.z; w = q.w; }
    }

    [Serializable]
    public struct ColorSerializer
    {
        public float r;
        public float g;
        public float b; 
        public float a;
        public ColorSerializer(Color color)
        { r = color.r; g = color.g; b = color.b; a = color.a; }
    }
    /// <summary>
    /// 
    /// Model
    /// </summary>
    public enum GraphicType
    {
        Image,
        Text,
        Line
    }
  
    public abstract class T_Graphic
{
        public Vector2ReactiveProperty  localPos=new Vector2ReactiveProperty();
        //Unity本身Quat对象序列化有误，需要封装一层
        public ReactiveProperty<QuaternionSerializer> locaRotation=new ReactiveProperty<QuaternionSerializer>();
        public Vector3ReactiveProperty localScale=new Vector3ReactiveProperty(Vector3.one);

        public FloatReactiveProperty widht=new FloatReactiveProperty();
        public FloatReactiveProperty height=new FloatReactiveProperty();

        //选中
        public BoolReactiveProperty isSelected=new BoolReactiveProperty(false);

        public StringReactiveProperty spritrsStr=new StringReactiveProperty();
      
        public ReactiveProperty<ColorSerializer> mainColor =new ReactiveProperty<ColorSerializer>(new ColorSerializer(Color.white));
        //渲染层级
        public int siblingIndex;
        //框选
        public BoolReactiveProperty isChecking=new BoolReactiveProperty(false);

        public GraphicType graphicType= GraphicType.Image;

        public ReactiveProperty<WebSocketMessage> AssetNodeData =new ReactiveProperty<WebSocketMessage>() ;
   
        public virtual void Destroy()
        {
            isSelected.Value=false;
            isChecking.Value = false;
            Global.RemoveBindData(AssetNodeData);
        }

        public virtual void ColorInit()
        {
            mainColor .Value = new ColorSerializer(Color.white);
        }
        public virtual void SetAssetNodeData(WebSocketMessage message)
        {
            if (AssetNodeData == null) Log.I("AssetNodeData IS Null");
             AssetNodeData.Value = message;
            Global.AddBindData(AssetNodeData);
        }
        //public void OnAfterDeserialize()
        //{

        //    Debug.Log("序列化开始");
        //}

        //public void OnBeforeSerialize()
        //{
        //    Debug.Log("反序列化结束后");
        //    if (AssetNodeData != null && AssetNodeData.Value != null) Global.AddBindData(AssetNodeData.Value);
        //}
    }
}
