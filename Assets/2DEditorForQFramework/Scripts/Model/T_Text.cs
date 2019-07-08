using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace TDE
{
    //字体
    public enum Font
    {
        Default
    }
    //字体的状态
    public enum FontStyleSelf
    {
        Normal,
        Bold,
        Italic,
        BoldAndItalic
    }
    //字体的对齐方式
    public enum ParagraphVerticalType
    {
        Top,
        Middle,
        Bottom
    }
    //段落的对齐方式
    public enum ParagraphHorizontalType
    {
        Left,
        Center,
        Rigth,
        Flush
    }
    //[Serializable]
    public class T_Text : T_Graphic
    {
        public ReactiveProperty<Font> font=new ReactiveProperty<Font>();
        public IntReactiveProperty fontSize=new IntReactiveProperty();
        public ReactiveProperty<FontStyleSelf> fontStyle=new ReactiveProperty<FontStyleSelf>();
        public ReactiveProperty<ParagraphVerticalType> paragraphVerticalAlignmentType=new ReactiveProperty<ParagraphVerticalType>();
        public ReactiveProperty<ParagraphHorizontalType> paragraphHorizontalAlignMentType=new ReactiveProperty<ParagraphHorizontalType>();
        public StringReactiveProperty content = new StringReactiveProperty();
        public ReactiveProperty<ColorSerializer> fontColor=new ReactiveProperty<ColorSerializer>(new ColorSerializer(Color.black));

        public T_Text():base() { graphicType = GraphicType.Text; }

       
        public T_Text(T_Text text) : base(text)
        {
            fontSize = new IntReactiveProperty(text.fontSize.Value);
            font = new ReactiveProperty<Font>(text.font.Value);
            fontStyle = new ReactiveProperty<FontStyleSelf>(text.fontStyle.Value);
            paragraphVerticalAlignmentType = new ReactiveProperty<ParagraphVerticalType>(text.paragraphVerticalAlignmentType.Value);
            paragraphHorizontalAlignMentType = new ReactiveProperty<ParagraphHorizontalType>(text.paragraphHorizontalAlignMentType.Value);
            content = new StringReactiveProperty(text.content.Value);
            fontColor = new ReactiveProperty<ColorSerializer>(text.fontColor.Value);
        }
    }
}
