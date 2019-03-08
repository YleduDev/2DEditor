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
	public partial class UIImageContent : UIElement
	{
        public TSceneData model;

        private void Awake()
		{
            BGButton.onClick.AddListener(()=>Global.OnClick());
		}

		protected override void OnBeforeDestroy()
		{
		}

        public void Add(T_Graphic graphicItem, UITItem UITItem)
        {
            if (graphicItem.IsNull()) return;     

            RectTransform rect=null;
            switch (graphicItem.graphicType)
            {
                case GraphicType.Image:
                    //����
                    UIImageItem UIImageItem = UITItem.UIImageItem.Instantiate();
                    rect = UIImageItem.transform as RectTransform;
                    //��ʼ��
                    UIImageItem.Init(graphicItem,ImageParent);
                    //���ж���
                    graphicItem.isSelected.Subscribe(on => {
                        if (on) UIImageItem.UIImageEditorBox.Show();
                        else UIImageItem.UIImageEditorBox.Hide();
                    });

                    break;
                case GraphicType.Text:
                    //����
                    UITextItem UITextItem = UITItem.UITextItem.Instantiate();
                    rect = UITextItem.transform as RectTransform;
                    //��ʼ��
                    UITextItem.Init(graphicItem,TextParent);
                    //���ж���
                    graphicItem.isSelected.Subscribe(on => {
                        if (on) UITextItem.UITextEditorBox.Show();
                        else UITextItem.UITextEditorBox.Hide();
                    });
                    break;
                case GraphicType.Line:
                    //����
                    UILineItem UILineItem = UITItem.UILineItem.Instantiate();
                    rect = UILineItem.transform as RectTransform;
                    //��ʼ��
                    UILineItem.Init(graphicItem,LineParent);
                    //���ж���
                    graphicItem.isSelected.Subscribe(on => {
                       //�߶ε���� Ч��
                    });
                    break;
                default:break;
            }

            graphicItem.localPos.Subscribe(v2 =>
                {
                    rect.LocalPosition(v2);
                });

            graphicItem.height.Subscribe(f => {
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, f);
            });

            graphicItem.widht.Subscribe(f => {
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, f);
            });
        }
     
        public void GenerateGraphicItem(TSceneData model,UITItem UITItem)
        {
            this.model = model;
            model.graphicDataList.ForEach(item => Add(item, UITItem));
        }
    }
}