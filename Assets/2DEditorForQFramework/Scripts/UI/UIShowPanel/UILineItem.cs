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
        //��ʼ���������(ֻ��һ�Σ�
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

        //Model�� ����
        void LineSubscribeInit()
        {
            //���ѡ��
            this.ApplySelfTo(self => self.line.isSelected.Subscribe(on => { }))
                //�ƶ�
                .ApplySelfTo(self => self.line.localPos.Subscribe(v2 => self.LocalPosition(v2)))
                //��С
                .ApplySelfTo(self => self.line.localScale.Subscribe(v3 => rect.LocalScale(v3)))
                 //��ת
                 //.ApplySelfTo(self => self.line.locaRotation.Subscribe(qua => rect.LocalRotation(qua)))
                 //��
                 // .ApplySelfTo(self => self.line.height.Subscribe(f => ChangeLine(f)))
                 //��
                 //.ApplySelfTo(self => self.line.widht.Subscribe(f => { }))
                 .ApplySelfTo(self => self.line.px.Subscribe(_ => ChangeLine()))
                //�߶���״
                .ApplySelfTo(self => self.line.lineShapeType.Subscribe(type => LineShapeTypeChange(type)))
                //�߶���״
                .ApplySelfTo(self => self.line.lineShapeType.Subscribe(e => LineShapeTypeChange(e)))
                .ApplySelfTo(self => self.line.lineBeginShapeType.Subscribe(_ => ChangeLine()))
                .ApplySelfTo(self => self.line.lineEndShapeType.Subscribe(_ => ChangeLine()))
                //�յ�
                .ApplySelfTo(self => self.line.localEndPos.Subscribe(_ => self.PointChange()))
                //���
                .ApplySelfTo(self => self.line.localOriginPos.Subscribe(_ => self.PointChange()))
                //��ɫ
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
            //ֱ��
            if (line.lineShapeType.Value == LineShapeType.Straight)
            {
                //�����߶�λ��
                line.localPos.Value = line.localOriginPos.Value;
                //��ȡ����(��Ϊ��ֱ�� ����Ҳ���߶ε��ܳ�)
                line.direction = Vector2.Distance(line.localOriginPos.Value, line.localEndPos.Value);
                //��ȡ����
                Vector2 tdirection = line.localEndPos.Value - line.localOriginPos.Value;

                // LineHead.LocalPosition(line.localOriginPos.Value);
                //����β����
                LineEnd.LocalPosition(tdirection);

                LineHead.LocalRotation(Quaternion.FromToRotation(Vector3.right, tdirection));
                LineSegment.LocalRotation(Quaternion.FromToRotation(Vector3.right, tdirection));
                LineEnd.LocalRotation(Quaternion.FromToRotation(Vector3.right, tdirection));
                //�����߶�����
                LineSegment.LocalPosition(/*line.localOriginPos.Value +*/ (Vector2)(LineHead.transform.localRotation * new Vector2(LineHead.Width, 0)));
                LineSegment.Width = line.direction - LineHead.Width - LineEnd.Width;

            }
            //����
            //����
        }

        //�߶ε���״���͸ı� ֱ��-���� -���ߣ��ı��㷨��
        internal void LineShapeTypeChange(LineShapeType sharpType)
        {
            if (line.lineShapeType.Value == sharpType) return;

        }

        internal void ChangeLine()
        {
            //ͷ
            LineHead.ChangeSprite(line);
            //��
            LineSegment.ChangeSprite(line);
            //β
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