﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UniRx;
using System;

namespace TDE
{
    public class Global 
    {
        public static int LinePx = 3;
        public static LineBeginShape beginShape = LineBeginShape.BeginLine;
        public static LineEndShape endShape = LineEndShape.EndArrows;
        public static float minLineLength = 10f;
        public static LineShapeType lineShapeType = LineShapeType.Straight;
        public static float currentCanvasWidth;
        public static float currentCanvasheight;

        public static RectTransform LineParent;
        public static RectTransform imageParent;
        public static RectTransform textParent;

        public static string allGraphicsFillName = "2DEditorGraphics";
        public static string GraphisMenuConfigPathName = "GraphicsMenmConfig";
        //配置 文本文件命名中 包含的约定
        public static string TextItemContainName = "Text";
        //全局，表示当前选中的图元对象
        public static ReactiveProperty< T_Graphic> OnSelectedGraphic=new ReactiveProperty<T_Graphic>();

        private static Vector2 ratio = Vector2.zero;

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
        public static Dictionary<Texture2D, Sprite> sprites = new Dictionary<Texture2D, Sprite>();

        //获取Sprite
        public static Sprite GetSprite(Texture2D tex)
        {
            //int spriteKey;
            if (sprites != null && sprites.ContainsKey(tex)) return sprites[tex];
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            sprite.name = tex.name;
            sprites.Add(tex, sprite);
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
                case MessageState.NORMAL: Log.I("绿"); return new ColorSerializer( Color.green);
                case MessageState.ERROR: Log.I("红"); return new ColorSerializer(Color.red);
                case MessageState.WARNING: Log.I("黄"); return new ColorSerializer(Color.yellow);
                default: Log.I("白"); return new ColorSerializer(Color.white);
            }
        }

        public static bool GetLocalPointOnCanvas(Vector2 loaclPoint)
        {
            float left = -currentCanvasWidth * 0.5f;
            float right = currentCanvasWidth * 0.5f;
            float up = currentCanvasheight * 0.5f;
            float donw = -currentCanvasheight * 0.5f;
            return loaclPoint.x > left && loaclPoint.x < right && loaclPoint.y > donw && loaclPoint.y < up;
        }


    }
}