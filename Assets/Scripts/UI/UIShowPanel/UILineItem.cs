/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TDE;
using UniRx;
using UnityEngine.EventSystems;

namespace QFramework.TDE
{
	public partial class UILineItem : UIElement,IPointerDownHandler
	{
        public T_Line line;
        RectTransform rect;
        
        internal void Init(T_Graphic graphicItem, RectTransform parent)
        {
            line = graphicItem as T_Line;
            rect = transform as RectTransform;
            this.transform.Parent(parent)
               .Show()
               .LocalPosition(line.localPos.Value)
               .LocalScale(line.localScale.Value)
               .LocalRotation(Global.GetquaternionForQS(line.locaRotation.Value));

            LineHead.Init(line,parent);
            LineEnd.Init(line, parent);
            LineSegment.Init(line, parent);

            LineSubscribeInit();
        }

        //Model层 订阅
        void LineSubscribeInit()
        {
            //点击选中
            this.ApplySelfTo(self => self.line.isSelected.Subscribe(on => { }))
                //移动
                .ApplySelfTo(self => self.line.localPos.Subscribe(v2 => rect.LocalPosition(v2)))
                //大小
                .ApplySelfTo(self => self.line.localScale.Subscribe(v3 => rect.LocalScale(v3)))
                 //旋转
                 //.ApplySelfTo(self => self.line.locaRotation.Subscribe(qua => rect.LocalRotation(qua)))
                 //高
                 // .ApplySelfTo(self => self.line.height.Subscribe(f => ChangeLine(f)))
                 //宽
                 //.ApplySelfTo(self => self.line.widht.Subscribe(f => { }))
                 .ApplySelfTo(self => self.line.px.Subscribe(_ => ChangeLine()))
                //线段形状
                .ApplySelfTo(self => self.line.lineShapeType.Subscribe(type => LineShapeTypeChange(type)))
                //线段形状
                .ApplySelfTo(self => self.line.lineShapeType.Subscribe(e => LineShapeTypeChange(e)))
                .ApplySelfTo(self => self.line.lineBeginShapeType.Subscribe(_ => ChangeLine()))
                .ApplySelfTo(self => self.line.lineEndShapeType.Subscribe(_ => ChangeLine()))
                //终点
                .ApplySelfTo(self => self.line.localEndPos.Subscribe(_ => self.PointChange()))
                //起点
                .ApplySelfTo(self => self.line.localOriginPos.Subscribe(_ => self.PointChange()));

        }

        internal void PointChange()
        {
            //直线
            if (line.lineShapeType.Value == LineShapeType.Straight)
            {
                //设置线段位置
                line.localPos.Value = line.localOriginPos.Value;
                //获取长度(因为是直线 所以也是线段的总长)
                line.direction = Vector2.Distance(line.localOriginPos.Value, line.localEndPos.Value);
                //获取方向
                Vector2 tdirection = line.localEndPos.Value - line.localOriginPos.Value;

                // LineHead.LocalPosition(line.localOriginPos.Value);
                //设置尾坐标
                LineEnd.LocalPosition(tdirection);

                LineHead.LocalRotation(Quaternion.FromToRotation(Vector3.right, tdirection));
                LineSegment.LocalRotation(Quaternion.FromToRotation(Vector3.right, tdirection));
                LineEnd.LocalRotation(Quaternion.FromToRotation(Vector3.right, tdirection));
                //设置线段坐标
                LineSegment.LocalPosition(/*line.localOriginPos.Value +*/ (Vector2)(LineHead.transform.localRotation * new Vector2(LineHead.Width, 0)));
                LineSegment.Width = line.direction - LineHead.Width - LineEnd.Width;

            }
            //折线
            //曲线
        }

        //线段的形状类型改变 直线-折线 -曲线（改变算法）
        internal void LineShapeTypeChange(LineShapeType sharpType)
        {
            if (line.lineShapeType.Value == sharpType) return;

        }

        internal void ChangeLine()
        {
            //头
            LineHead.ChangeSprite(line);
            //线
            LineSegment.ChangeSprite(line);
            //尾
            LineEnd.ChangeSprite(line);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Global.OnClick(line);
        }

        private void Awake(){}

        protected override void OnBeforeDestroy(){}
    }
}