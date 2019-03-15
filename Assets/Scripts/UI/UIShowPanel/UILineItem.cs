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

namespace QFramework.TDE
{
	public partial class UILineItem : UIElement
	{
        public T_Line line;
        RectTransform rect;

        ResLoader loader =  ResLoader.Allocate();
        internal void Init(T_Graphic graphicItem, RectTransform parent)
        {
            line = graphicItem as T_Line;
            rect = transform as RectTransform;
            this.transform.Parent(parent)
               .Show()
               .LocalPosition(line.localPos.Value)
               .LocalScale(line.localScale.Value)
               .LocalRotation(line.locaRotation.Value);
            

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
                //宽
                // .ApplySelfTo(self => self.line.height.Subscribe(f => rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, f)))
                //高
                .ApplySelfTo(self => self.line.widht.Subscribe(f => { }))
                //线段形状
                .ApplySelfTo(self => self.line.lineShapeType.Subscribe(type => LineShapeTypeChange(type)))
                //终点
                .ApplySelfTo(self => self.line.localEndPos.Subscribe(_ => self.EndPointChange()))
                //起点
                .ApplySelfTo(self => self.line.localOriginPos.Subscribe(_ => self.OriginPointChange()))
                //线段形状
                .ApplySelfTo(self => self.line.lineShapeType.Subscribe(e => LineShapeTypeChange(e)));
        }

        internal void EndPointChange()
        {
            if (line.lineShapeType.Value == LineShapeType.Straight)
            {
                float widht = Vector2.Distance(line.localOriginPos.Value, line.localEndPos.Value);
                Vector2 tdirection = line.localEndPos.Value - line.localOriginPos.Value;
                line.locaRotation.Value = Quaternion.FromToRotation(Vector3.right, tdirection);
            }
        }

        internal void OriginPointChange()
        {
            //直线
            if (line.lineShapeType.Value == LineShapeType.Straight)
            {
                line.localPos.Value = line.localOriginPos.Value;
                float widht = Vector2.Distance(line.localOriginPos.Value, line.localEndPos.Value);
                Vector2 tdirection = line.localEndPos.Value - line.localOriginPos.Value;
                line.locaRotation.Value = Quaternion.FromToRotation(Vector3.right, tdirection);
            }
        }

        //线段的形状类型改变 直线-折线 -曲线（改变算法）
        internal void LineShapeTypeChange(LineShapeType sharpType)
        {
            if (line.lineShapeType.Value == sharpType) return;

        }

        private void Awake(){
        }

        protected override void OnBeforeDestroy(){
            loader.Recycle2Cache();
            loader = null;
        }
    }
}