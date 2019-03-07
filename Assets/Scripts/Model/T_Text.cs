using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TDE
{
    //����
    public enum Font
    {
        Default
    }
    //�����״̬
    public enum FontStyle
    {
        Normal,
        Bold,
        Ltalic,
        BoldAndLtalic
    }
    //����Ķ��뷽ʽ
    public enum FontAlignMentType
    {
        FontLeft,
        FontCentre,
        FontRight,
    }
    //����Ķ��뷽ʽ
    public enum ParagraphAlignMentType
    {
        FontLeft,
        FontCentre,
        FontRight,
    }
    public class T_Text : T_Graphic
    {
        public int fontSize;
        public Font font;
        public FontStyle fontStyle;
        public FontAlignMentType fontAlignMentType;
        public ParagraphAlignMentType paragraphAlignMentType;

        public new GraphicType graphicType = GraphicType.Text;

    }
}
