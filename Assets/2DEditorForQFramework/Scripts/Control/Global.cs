using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UniRx;
using System;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Text;

namespace TDE
{
    public class Global 
    {
        //当前场景data
        public static ReactiveProperty< TSceneData> currentSceneData=new ReactiveProperty<TSceneData>();
        //线段像素
        public static int LinePx = 3;
        //线段的起 末
        public static LineBeginShape beginShape = LineBeginShape.BeginLine;
        public static LineEndShape endShape = LineEndShape.EndArrows;
        //划线时最小有效距离
        public static float minLineLength = 10f;
        //线段的算法，类型  -- 直线 - 折线 -- 曲线
        public static LineShapeType lineShapeType = LineShapeType.Straight;
        //画布的宽高
        public static FloatReactiveProperty currentCanvasWidth=new FloatReactiveProperty();
        public static FloatReactiveProperty currentCanvasheight=new FloatReactiveProperty();

        //全屏
        public static BoolReactiveProperty fullScreen = new BoolReactiveProperty(false);

        public static BoolReactiveProperty WebglShowScene = new BoolReactiveProperty(false);

        //图元父物体
        public static RectTransform LineParent;
        public static RectTransform imageParent;
        public static RectTransform textParent;
#if UNITY_EDITOR
        //所有图元存放路径
        public static string allGraphicsFillName = "2DEditorGraphics";
#endif
        //控件存放路径
        public static string allWidgetsFillName = "Widget";
        //所有图元配置文档
        public static string GraphisMenuConfigPathName = "GraphicsMenmConfig";
        //所有控件配置文档
        public static string WidgetConfigPathName = "WidgetConfig";
        //所有控件配对预制体配置文档
        public static string WidgetPrefabConfigPathName = "WidgetPrefabConfig";

        public static string CustomCinfigGraphicsPathName = "Graphics";
        //配置 文本文件命名中 包含的约定
        public static string TextItemContainName = "文本";
        //全局，表示当前选中的图元对象
        public static ReactiveProperty< T_Graphic> OnSelectedGraphic=new ReactiveProperty<T_Graphic>();      

        public static Dictionary<string, ReactiveProperty< WebSocketMessage>> bindDataDict = new Dictionary<string, ReactiveProperty<WebSocketMessage>>();

        public static void AddBindData(ReactiveProperty<WebSocketMessage> data)
        {
            if (data.Value!=null&&!bindDataDict.ContainsKey(data.Value.Id))
            {
               // Log.I("绑点数据:"+ data.Value.Id);
                bindDataDict.Add(data.Value.Id,  data);
            }
        }
        public static void RemoveBindData(ReactiveProperty<WebSocketMessage> data)
        {
            if (data.Value != null && bindDataDict.ContainsKey(data.Value.Id))
            {
                bindDataDict.Remove(data.Value.Id);
            }
        }

        public static void UpdataBindData(WebSocketMessage data)
        {
            if (data != null && bindDataDict.ContainsKey(data.Id))
            {
                bindDataDict[data.Id].Value = data;
            }
        }

        /// <summary>
        /// 场景添加纹理对象数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void CurrentSceneDataAddTextrueData(string key,string value)
        {
            currentSceneData.Value.AddTextrueData(key, value);
        }

        public static bool CurrentSceneDataAddGraphic(T_Graphic graphic)
        {
            return currentSceneData.Value.Add(graphic);
        }
        /// <summary>
        /// ShowPanel 图元及面板点击事件
        /// </summary>
        /// <param name="graphic"></param>
        public static void OnClick(T_Graphic graphic = null)
        {
            if (graphic.IsNotNull() && graphic.Equals(OnSelectedGraphic))  return; 
            if (OnSelectedGraphic.Value.IsNotNull()) OnSelectedGraphic.Value.isSelected.Value = false;             
             OnSelectedGraphic.Value = graphic;
            if (graphic != null) graphic.isSelected.Value = true;
            
        }
        
        //存储当前所有的Sprite
        public static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

        public static Sprite GetSprite(string spriteName)
        {
            if (sprites != null && sprites.ContainsKey(spriteName)) return sprites[spriteName];
            return null;
        }
        //获取Sprite
        //注意：所有不一样的Texture2D的名称不能一致，不然获取的sprites可能不一样(第一个存入相同名称的sprite)
        public static Sprite GetSprite(Texture2D tex)
        {
            //int spriteKey;
            if (!tex) return null;
            if (sprites != null && sprites.ContainsKey(tex.name)) return sprites[tex.name];
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            sprite.name = tex.name;
            sprites.Add(tex.name, sprite);
            return sprite;
        }


        #region 序列化处理工具
        public static Quaternion GetQuaternionForQS(QuaternionSerializer data)
        {
            return new Quaternion(data.x, data.y, data.z, data.w);
        }
        public static Color GetColorCS(ColorSerializer data)
        {
            return new Color(data.r, data.g, data.b, data.a);
        }
        public static ColorSerializer GetColorForState(WebSocketMessage message )
        {
            MessageState State = MessageState.NORMAL;
            if (!string.IsNullOrEmpty(message.State))
             State = (MessageState)Enum.Parse(typeof(MessageState), message.State);
            switch (State)
            {
                case MessageState.NORMAL: return new ColorSerializer( Color.green);
                case MessageState.ERROR: return new ColorSerializer(Color.red);
                case MessageState.WARNING:  return new ColorSerializer(Color.yellow);
                default: return new ColorSerializer(Color.white);
            }
        }
        #endregion
        //获取点位是否在画布内，ture->在画布内
        public static bool GetLocalPointOnCanvas(Vector2 loaclPoint)
        {
            float left = 0;
            float right = currentCanvasWidth.Value ;
            float up =0;
            float donw = -currentCanvasheight.Value ;
            return loaclPoint.x > left && loaclPoint.x < right && loaclPoint.y > donw && loaclPoint.y < up;
        }

        //该方法后续需要提取到特定类中
        /// <summary>
        /// 处理group的刷新方法
        /// </summary>
        /// <param name="rect"> group组件挂载的rect</param>
        /// <returns></returns>
        public static  IEnumerator UpdateLayout(RectTransform rect)
        {
            //刷新
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            yield return new WaitForEndOfFrame();
            float width = rect.rect.width;
            //防止一帧完成不了
            while (rect.rect.width == 0)
            {
                Log.I(rect.rect.width);
                LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
                yield return new WaitForEndOfFrame();
            }
        }    
    }
}