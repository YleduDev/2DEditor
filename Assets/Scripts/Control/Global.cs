using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UniRx;
namespace TDE
{
    public class Global : MonoBehaviour
    {
        public static int LinePx = 1;
        public readonly static float MainScreenWidth = 1920f;
        public readonly static float MainScreenHeight = 1080f;
        public static LineBeginShape beginShape= LineBeginShape.BeginArrows;
        public static LineEndShape endShape = LineEndShape.EndArrows;
        public static float minLineLength = 10f;
        public static LineShapeType lineShapeType = LineShapeType.Straight;
        public static float currentCanvasWidth;
        public static float currentCanvasheight;
        //全局，表示当前选中的图元对象
        public static T_Graphic OnSelectedGraphic;

        public static float ScreenWidthForMainScr
        {
            get
            {
                return MainScreenWidth / (float)Screen.width;
            }
        }

        public static float ScreeRatio
        {
            get { return Screen.width / Screen.height; }
        }
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


        public static Quaternion GetquaternionForQS(QuaternionSerializer data)
        {
            return new Quaternion(data.x, data.y, data.z, data.w);
        }

        public  static bool GetLocalPointOnCanvas(Vector2 loaclPoint)
        {
            float left = -currentCanvasWidth / 2;
            float right= currentCanvasWidth / 2;
            float up = currentCanvasheight / 2;
            float donw = -currentCanvasheight / 2;
            return loaclPoint.x > left && loaclPoint.x < right && loaclPoint.y > donw && loaclPoint.y < up;
        }
    }
}