using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UniRx;
namespace TDE
{
    public class Global : MonoBehaviour
    {
        public static int LinePx = 3;
        public static LineBeginShape beginShape= LineBeginShape.BeginLine;
        public static LineEndShape endShape = LineEndShape.EndArrows;
        public static float minLineLength = 10f;
        public static LineShapeType lineShapeType = LineShapeType.Straight;
        public static float currentCanvasWidth;
        public static float currentCanvasheight;

        public static RectTransform LineParent;
        public static RectTransform imageParent;
        public static RectTransform textParent;

        public static string allGraphicsFillName= "2DEditorGraphics";
        public static string GraphisMenuConfigPathName = "GraphicsMenmConfig";
        //配置 文本文件命名中 包含的约定
        public static string TextItemContainName = "Text";
        //全局，表示当前选中的图元对象
        public static T_Graphic OnSelectedGraphic;

        private static Vector2 ratio= Vector2.zero;
        /// <summary>
        /// ShowPanel 图元及面板点击事件
        /// </summary>
        /// <param name="graphic"></param>
        public static void OnClick(T_Graphic graphic=null)
        {
           if(OnSelectedGraphic.IsNotNull()) OnSelectedGraphic.isSelected.Value = false;
            if (graphic.IsNotNull())
            {
                graphic.isSelected.Value = true;
                OnSelectedGraphic = graphic;
            }
        }
        //存储当前所有的Sprite
        public static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

        //获取Sprite
        public static Sprite GetSprite(string key)
        {
            key = key.Trim();
            //int spriteKey;
            if (sprites != null && sprites.ContainsKey(key)) return sprites[key];
            return null;
        }

        public static Quaternion GetQuaternionForQS(QuaternionSerializer data)
        {
            return new Quaternion(data.x, data.y, data.z, data.w);
        }
        public static Color GetColorQS(ColorSerializer data)
        {
            return new Color(data.r, data.g, data.b, data.a);
        }

        public  static bool GetLocalPointOnCanvas(Vector2 loaclPoint)
        {
            float left = -currentCanvasWidth / 2;
            float right= currentCanvasWidth / 2;
            float up = currentCanvasheight / 2;
            float donw = -currentCanvasheight / 2;
            return loaclPoint.x > left && loaclPoint.x < right && loaclPoint.y > donw && loaclPoint.y < up;
        }

        /// 转换为Sprite
        public static Sprite ChangeToSprite(Texture2D tex)
        {
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            sprite.name = tex.name;
            return sprite;
        }
    }
}