/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TDE;
using System.Linq;
using UniRx;

namespace QFramework.TDE
{
	public partial class UIContent : UIElement
	{
        private Dictionary<T_Graphic,UIImageItem> imageDict=new Dictionary<T_Graphic, UIImageItem>();
        private Dictionary<T_Graphic, UITextItem> textDict = new Dictionary<T_Graphic, UITextItem>();
        private Dictionary<T_Graphic, UILineItem> lineDict = new Dictionary<T_Graphic, UILineItem>();

        private UITItem UITItem;
        private TSceneData model;

        private void Awake()
		{
            BGButton.onClick.AddListener(() => Global.OnClick());
        }

		protected override void OnBeforeDestroy()
		{
		}
               
        public void Add(T_Graphic graphicItem)
        {
            if (graphicItem.IsNull()) return;
            RectTransform rect = null;
            switch (graphicItem.graphicType)
            {
                case GraphicType.Image:
                    //生成
                    UIImageItem UIImageItem = UITItem.UIImageItem.Instantiate();
                    rect = UIImageItem.transform as RectTransform;
                    //初始化
                    UIImageItem.ApplySelfTo(self => { self.Init(graphicItem, ImageParent); })
                        .ApplySelfTo(self =>
                        {
                            //进行订阅
                            graphicItem.isSelected.Subscribe(on =>
                            {
                                if (on) UIImageItem.UIEditorBox.Show();
                                else UIImageItem.UIEditorBox.Hide();
                            });
                        })
                        .ApplySelfTo(self =>
                        {

                            imageDict.Add(graphicItem, UIImageItem);
                        })
                        .ApplySelfTo(self=> { self.UILineSwitch.Init(model, LineParent); });
                    break;
                case GraphicType.Text:
                    //生成
                    UITextItem UITextItem = UITItem.UITextItem.Instantiate();
                    rect = UITextItem.transform as RectTransform;
                    //初始化
                    UITextItem.ApplySelfTo(self => { self.Init(graphicItem, TextParent); })
                        .ApplySelfTo(self =>
                        {//进行订阅
                            graphicItem.isSelected.Subscribe(on =>
                            {
                                if (on) UITextItem.UIEditorBox.Show();
                                else UITextItem.UIEditorBox.Hide();
                            });
                        })
                        .ApplySelfTo(self => { textDict.Add(graphicItem, UITextItem); });
                    
                    break;
                case GraphicType.Line:
                    //生成
                    UILineItem UILineItem = UITItem.UILineItem.Instantiate();
                    rect = UILineItem.transform as RectTransform;
                    //初始化
                    UILineItem.ApplySelfTo(self => { self.Init(graphicItem, LineParent); })
                        .ApplySelfTo(self =>
                        { //进行订阅
                            graphicItem.isSelected.Subscribe(on =>
                            {
                                //线段点击后 效果
                            });
                        })
                    .ApplySelfTo(self=> { lineDict.Add(graphicItem, UILineItem); });                 
                    break;
                default: break;
            }

            graphicItem.localPos.Subscribe(v2 =>
            {
                rect.LocalPosition(v2);
            });
            graphicItem.localScale.Subscribe(v3 =>
            {
                rect.LocalScale(v3);
            });
            graphicItem.locaRotation.Subscribe(qua =>
            {
                rect.LocalRotation(qua);
            });

            graphicItem.height.Subscribe(f => {
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, f);
            });

            graphicItem.widht.Subscribe(f => {
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, f);
            });
        }

        public void GenerateGraphicItem(TSceneData model, UITItem UITItem)
        {
            this.model = model;
            this.UITItem = UITItem;
            model.graphicDataList.ForEach(item => Add(item));
        }
    }
}