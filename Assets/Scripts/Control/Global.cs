using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UniRx;
using System;
using UnityEngine.UI;
using Newtonsoft.Json;

namespace TDE
{
    public class Global 
    {
        public static ReactiveProperty< TSceneData> currentSceneData=new ReactiveProperty<TSceneData>();

        public static int LinePx = 3;
        //线段的起 末
        public static LineBeginShape beginShape = LineBeginShape.BeginLine;
        public static LineEndShape endShape = LineEndShape.EndArrows;
        //划线时最小有效距离
        public static float minLineLength = 10f;
        //线段的算法，类型  -- 直线 - 折线 -- 曲线
        public static LineShapeType lineShapeType = LineShapeType.Straight;
        //画布的宽高
        public static float currentCanvasWidth;
        public static float currentCanvasheight;

        //图元父物体
        public static RectTransform LineParent;
        public static RectTransform imageParent;
        public static RectTransform textParent;
#if UNITY_EDITOR
        //所有图元存放路径
        public static string allGraphicsFillName = "2DEditorGraphics";
#endif
        //所有图元配置文档
        public static string GraphisMenuConfigPathName = "GraphicsMenmConfig";
        public static string CustomCinfigGraphicsPathName = "Graphics";
        //配置 文本文件命名中 包含的约定
        public static string TextItemContainName = "Text";
        //全局，表示当前选中的图元对象
        public static ReactiveProperty< T_Graphic> OnSelectedGraphic=new ReactiveProperty<T_Graphic>();      

        public static Dictionary<string, ReactiveProperty< WebSocketMessage>> bindDataDict = new Dictionary<string, ReactiveProperty<WebSocketMessage>>();

        public static void AddBindData(ReactiveProperty<WebSocketMessage> data)
        {
            if (data.Value!=null&&!bindDataDict.ContainsKey(data.Value.Id))
            {
                Log.I("绑点数据:"+ data.Value.Id);
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
            if (OnSelectedGraphic.Value.IsNotNull()) OnSelectedGraphic.Value.isSelected.Value = false;
            if (graphic.IsNotNull())
            {
                graphic.isSelected.Value = true;
                OnSelectedGraphic.Value = graphic;
            }
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

        //获取点位是否在画布内，ture->在画布内
        public static bool GetLocalPointOnCanvas(Vector2 loaclPoint)
        {
            float left = -currentCanvasWidth * 0.5f;
            float right = currentCanvasWidth * 0.5f;
            float up = currentCanvasheight * 0.5f;
            float donw = -currentCanvasheight * 0.5f;
            return loaclPoint.x > left && loaclPoint.x < right && loaclPoint.y > donw && loaclPoint.y < up;
        }

        //该方法后续需要提取到特定类中
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
        
        public static void ClearChildForTransform(Transform tf)
        {
            if (tf.childCount > 0)
                for (int i = 0; i < tf.childCount; i++)
                {
                    GameObject.Destroy(tf.GetChild(i).gameObject);
                }
        }

    }
}