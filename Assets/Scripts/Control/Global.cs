using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
namespace TDE
{
    public class Global : MonoBehaviour
    {
        public static float LineHeight = 3f;
        public static LineBothEndsShape beginShape= LineBothEndsShape.Line;
        public static LineBothEndsShape endShape = LineBothEndsShape.Line;
        public static float minLineLength = 10f;
        public static LineShapeType lineShapeType = LineShapeType.Straight;
        //ȫ�֣���ʾ��ǰѡ�е�ͼԪ����
        public static T_Graphic OnSelectedGraphic;

        /// <summary>
        /// ShowPanel ͼԪ��������¼�
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
    }
}