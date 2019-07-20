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
        Image[] UIImages;
        RectTransform parent;
        //初始化绑点数据(只做一次）
        bool AssetNodeDataOnInit = false;

        internal void Init(T_Graphic graphicItem)
        {
            line = graphicItem as T_Line;
            rect = transform as RectTransform;
            parent =Global.LineParent;

            if (line.sceneLoaded.IsNotNull()) line.sceneLoaded = null;
            if (line.sceneSaveBefore.IsNotNull()) line.sceneSaveBefore = null;

            line.sceneLoaded += OnSceneLoadEnd;
            line.sceneSaveBefore += OnSceneBefore;

            this.transform.Parent(parent)
               .Show()
               .LocalPosition(line.localPos.Value)
               .LocalScale(line.localScale.Value)
               .LocalRotation(Global.GetQuaternionForQS(line.locaRotation.Value))
               .ApplySelfTo(self => UIImages = GetComponentsInChildren<Image>())
               .SetAsLastSibling();

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
                .ApplySelfTo(self => self.line.localPos.Subscribe(v2 => self.LocalPosition(v2)))
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
                .ApplySelfTo(self => self.line.localOriginPos.Subscribe(_ => self.PointChange()))
                //颜色
                .ApplySelfTo(self => self.line.mainColor.Subscribe(color => UIImages.ForEach(item => item.color = Global.GetColorCS(color))))
               // .ApplySelfTo(self => self.line.AssetNodeData=new ReactiveProperty<WebSocketMessage>())
               // .ApplySelfTo(self => self.line.ColorInit())
                .ApplySelfTo(self => self.line.AssetNodeData.Subscribe(data=> {
                    if (data.IsNotNull())
                    {
                        if (!AssetNodeDataOnInit)
                        {
                            ServerData.GetAssetNodeForID(data.Id, (str) =>
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    AssetNode asset = SerializeHelper.FromJson<AssetNode>(str);
                                    if (asset.IsNull()) { data = null; self.line.ColorInit(); }
                                    else
                                    {
                                        data.Data = asset.valueStr;
                                        data.State = asset.state;
                                    }
                                }
                            });
                            AssetNodeDataOnInit = true;
                        }

                        self.line.mainColor.Value = Global.GetColorForState(data);
                    }
                    else { self.line.ColorInit(); AssetNodeDataOnInit = true; } }))
                    .ApplySelfTo(self => self.line.siblingType.Subscribe(
                     dataType =>
                     {
                         int index;
                         switch (dataType)
                         {
                             case SiblingEditorType.None: break;
                             case SiblingEditorType.UPOne:
                                 index = self.rect.GetSiblingIndex() + 1; self.rect.SetSiblingIndex(index); self.line.siblingType.Value = SiblingEditorType.None;
                                 break;
                             case SiblingEditorType.DonwOne:
                                 index = self.rect.GetSiblingIndex() - 1; self.rect.SetSiblingIndex(index); self.line.siblingType.Value = SiblingEditorType.None;
                                 break;
                             case SiblingEditorType.UpEnd:
                                 self.rect.SetAsLastSibling(); self.line.siblingType.Value = SiblingEditorType.None;
                                 break;
                             case SiblingEditorType.DonwEnd:
                                 self.rect.SetAsFirstSibling(); self.line.siblingType.Value = SiblingEditorType.None;
                                 break;
                         }
                     }));

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

        public void OnSceneLoadEnd()
        {
            this.rect.SetSiblingIndex(this.line.localSiblingIndex);

        }

        public void OnSceneBefore()
        {
            this.line.localSiblingIndex = rect.GetSiblingIndex();   //this.rect.SetSiblingIndex();
        }

        private void Awake(){}

        protected override void OnBeforeDestroy(){}
    }
}