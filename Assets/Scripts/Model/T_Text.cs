using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace TDE
{
    //字体
    public enum Font
    {
        Default
    }
    //字体的状态
    public enum FontStyle
    {
        Normal,
        Bold,
        Ltalic,
        BoldAndLtalic
    }
    //字体的对齐方式
    public enum FontAlignMentType
    {
        FontLeft,
        FontCentre,
        FontRight,
    }
    //段落的对齐方式
    public enum ParagraphAlignMentType
    {
        FontLeft,
        FontCentre,
        FontRight,
    }
    //[Serializable]
    public class T_Text : T_Graphic
    {      
        public int fontSize;
        public Font font;
        public FontStyle fontStyle;
        public FontAlignMentType fontAlignMentType;
        public ParagraphAlignMentType paragraphAlignMentType;
        
        public T_Text():base() { graphicType = GraphicType.Text; }

    }
}
