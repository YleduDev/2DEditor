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
        Line,
        Widget
    }
  public enum SiblingEditorType
    {
        None,
        UPOne,
        DonwOne,
        UpEnd,
        DonwEnd
    }

   
    public delegate void OnSceneLoaded();
    
    public delegate void OnSceneSaveBefore();

    public abstract class T_Graphic
    {
        [JsonIgnore]
        public OnSceneLoaded sceneLoaded;
        [JsonIgnore]
        public OnSceneSaveBefore sceneSaveBefore;
        

        public Vector2ReactiveProperty  localPos=new Vector2ReactiveProperty();
        //Unity本身Quat对象序列化有误，需要封装一层
        public ReactiveProperty<QuaternionSerializer> locaRotation=new ReactiveProperty<QuaternionSerializer>();
        public Vector3ReactiveProperty localScale=new Vector3ReactiveProperty(Vector3.one);
          
        public FloatReactiveProperty widht=new FloatReactiveProperty();
        public FloatReactiveProperty height=new FloatReactiveProperty();

        //选中
        public BoolReactiveProperty isSelected=new BoolReactiveProperty(false);

        public string lastSpritrsStr;

        public StringReactiveProperty spritrsStr=new StringReactiveProperty();
      
        public ReactiveProperty<ColorSerializer> mainColor =new ReactiveProperty<ColorSerializer>(new ColorSerializer(Color.white));
        //渲染层级
        public IntReactiveProperty siblingIndex = new IntReactiveProperty();

        //层级设置 枚举
        public ReactiveProperty<SiblingEditorType> siblingType = new ReactiveProperty<SiblingEditorType>();

        public int localSiblingIndex;

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
        public virtual void ClearAssetNodeData()
        {
            Global.RemoveBindData(AssetNodeData);
            if (AssetNodeData.Value.IsNotNull())
            {
                AssetNodeData.Value = null;
                ColorInit();
            }
        }
  
        public T_Graphic():base()
        {

        }
       

        public T_Graphic(T_Graphic graphic):base()
        {
            graphicType = graphic.graphicType;
            localPos = new Vector2ReactiveProperty(graphic.localPos.Value);
            locaRotation = new ReactiveProperty<QuaternionSerializer>(graphic.locaRotation.Value);
            localScale = new Vector3ReactiveProperty(graphic.localScale.Value);
            widht = new FloatReactiveProperty(graphic.widht.Value);
            height = new FloatReactiveProperty(graphic.height.Value);
            isSelected = new BoolReactiveProperty(false);
            spritrsStr = new StringReactiveProperty(graphic.spritrsStr.Value);
            mainColor = new ReactiveProperty<ColorSerializer>(graphic.mainColor.Value);
            siblingIndex = new IntReactiveProperty(graphic.siblingIndex.Value);
        }
    }
}
