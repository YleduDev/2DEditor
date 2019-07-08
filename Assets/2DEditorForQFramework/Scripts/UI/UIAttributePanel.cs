//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QFramework.TDE
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    using QFramework;
    using UniRx;
    using global::TDE;

    public class UIAttributePanelData : QFramework.UIPanelData
    {
        public TSceneData model;
        public static int colorJudge = 0;
    }

    public partial class UIAttributePanel : QFramework.UIPanel
    {
        protected override void ProcessMsg(int eventId, QFramework.QMsg msg)
        {

        }

        protected override void OnInit(QFramework.IUIData uiData)
        {
            mData = uiData as UIAttributePanelData ?? new UIAttributePanelData();
            Global.OnSelectedGraphic.Subscribe(data =>  ChangeValue(data));
            UISize.Mask_UISize.Show();
            UIFill.Mask_UIFill.Show();
            UILine.Mask_UILine.Show();
            ImageMask.Hide();
            ColorItemPanel.Hide();
            ColorItemPanel.OtherColor.Hide();
            Image buttonUITitle = UITitle.Title_Button.GetComponent<Image>();
            //��ťע���¼�
            UITitle.Title_Button.onClick.AddListener(() =>
            {
                if (UITitle.Title_Content.IsActive())
                {
                    UITitle.Title_Content.Hide(); buttonUITitle?.LocalRotation(Quaternion.Euler(new Vector3(0, 0, 90)));
                }
                else { UITitle.Title_Content.Show(); buttonUITitle?.LocalRotation(Quaternion.Euler(new Vector3(0, 0, 0))); }
            });
            Image buttonUISize = UISize.Size_Button.GetComponent<Image>();
            //��ťע���¼�
            UISize.Size_Button.onClick.AddListener(() =>
            {
                if (UISize.Size_Content.IsActive())
                {
                    UISize.Size_Content.Hide(); buttonUISize?.LocalRotation(Quaternion.Euler(new Vector3(0, 0, 90)));
                }
                else { UISize.Size_Content.Show(); buttonUISize?.LocalRotation(Quaternion.Euler(new Vector3(0, 0, 0))); }
            });
            Image buttonUIFill = UIFill.Fill_Button.GetComponent<Image>();
            //��ťע���¼�
            UIFill.Fill_Button.onClick.AddListener(() =>
            {
                if (UIFill.Fill_Content.IsActive())
                {
                    UIFill.Fill_Content.Hide(); buttonUIFill?.LocalRotation(Quaternion.Euler(new Vector3(0, 0, 90)));
                }
                else { UIFill.Fill_Content.Show(); buttonUIFill?.LocalRotation(Quaternion.Euler(new Vector3(0, 0, 0))); }
            });
            Image buttonUILine = UILine.Line_Button.GetComponent<Image>();
            //��ťע���¼�
            UILine.Line_Button.onClick.AddListener(() =>
            {
                if (UILine.Line_Content.IsActive())
                {
                    UILine.Line_Content.Hide(); buttonUILine?.LocalRotation(Quaternion.Euler(new Vector3(0, 0, 90)));
                }
                else { UILine.Line_Content.Show(); buttonUILine?.LocalRotation(Quaternion.Euler(new Vector3(0, 0, 0))); }
            });
            //��ťע���¼�
            UIFill.down_Fill.onClick.AddListener(() =>
            {
                UIAttributePanelData.colorJudge = 0;
                ColorItemPanel.transform.position = new Vector3(ColorItemPanel.transform.position.x, UIFill.down_Fill.transform.position.y - 0.2f, ColorItemPanel.transform.position.z);
                ColorItemPanel.Show();
                ImageMask.Show();
                ColorItemPanel.OtherColor.Hide();
            });
            //��ťע���¼�
            UIFill.down_Frame.onClick.AddListener(() =>
            {
                UIAttributePanelData.colorJudge = 1;
                ColorItemPanel.transform.position = new Vector3(ColorItemPanel.transform.position.x, UIFill.down_Frame.transform.position.y - 0.2f, ColorItemPanel.transform.position.z);
                ColorItemPanel.Show();
                ImageMask.Show();
                ColorItemPanel.OtherColor.Hide();
            });
            //��ťע���¼�
            UILine.down.onClick.AddListener(() =>
            {
                UIAttributePanelData.colorJudge = 2;
                if (UITitle.Title_Content.IsActive() && UISize.Size_Content.IsActive() && UIFill.Fill_Content.IsActive())
                {
                    ColorItemPanel.transform.position = new Vector3(ColorItemPanel.transform.position.x, UILine.down.transform.position.y + 1.8f, ColorItemPanel.transform.position.z);
                }
                else
                {
                    ColorItemPanel.transform.position = new Vector3(ColorItemPanel.transform.position.x, UILine.down.transform.position.y - 0.2f, ColorItemPanel.transform.position.z);
                }
                ColorItemPanel.Show();
                ImageMask.Show();
                ColorItemPanel.OtherColor.Hide();
            });
            ImageMask.onClick.AddListener(() =>
            {
                ColorItemPanel.Hide();
                ImageMask.Hide();
            });

            ColorButten();
        }

        public void ChangeColor(Color color)
        {
            switch (UIAttributePanelData.colorJudge)
            {
                case 0:
                    UIFill.color_Fill.color = color;
                    if (Global.OnSelectedGraphic.Value.IsNotNull())
                    {
                        Global.OnSelectedGraphic.Value.mainColor.Value = new ColorSerializer(UIFill.color_Fill.color);
                    }
                    break;
                case 1:
                    UIFill.color_Frame.color = color;
                    break;
                case 2:
                    UILine.color.color = color;
                    if (Global.OnSelectedGraphic.Value.IsNotNull())
                    {
                        Global.OnSelectedGraphic.Value.mainColor.Value = new ColorSerializer(UILine.color.color);
                    }
                    break;
                default:
                    break;
            }
        }

        private void ChangeValue(T_Graphic data)
        {
            if (data.IsNull()) return;
            switch (data.graphicType)
            {
                case GraphicType.Image:
                    UISize.Mask_UISize.Hide();
                    UIFill.Mask_UIFill.Hide();
                    UILine.Mask_UILine.Show();
                    break;
                case GraphicType.Text:
                    UISize.Mask_UISize.Hide();
                    UIFill.Mask_UIFill.Show();
                    UILine.Mask_UILine.Show();
                    break;
                case GraphicType.Line:
                    UISize.Mask_UISize.Show();
                    UIFill.Mask_UIFill.Show();
                    UILine.Mask_UILine.Hide();
                    break;
                default:
                    UISize.Mask_UISize.Show();
                    UIFill.Mask_UIFill.Show();
                    UILine.Mask_UILine.Show();
                    break;
            }
        }

        public void ColorButten()
        {
            for (int i = 0; i < ColorItemPanel.colorButten.transform.childCount; i++)
            {
                for (int j = 0; j < ColorItemPanel.colorButten.transform.GetChild(i).childCount; j++)
                {
                    for (int z = 0; z < ColorItemPanel.colorButten.transform.GetChild(i).GetChild(j).childCount; z++)
                    {
                        Image image = ColorItemPanel.colorButten.transform.GetChild(i).GetChild(j).GetChild(z).GetComponent<Image>();
                        ColorItemPanel.colorButten.transform.GetChild(i).GetChild(j).GetChild(z).GetComponent<Button>().onClick.AddListener(() =>
                        {
                            ChangeColor(image.color);
                        });
                    }
                }
            }

            ColorItemPanel.ensureButten.onClick.AddListener(() =>
            {
                ChangeColor(ColorItemPanel.ColorShowButten.color);
                ColorItemPanel.Hide();
                ImageMask.Hide();
            });
        }

        protected override void OnOpen(QFramework.IUIData uiData)
        {
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {

        }

        protected override void OnClose()
        {
        }
    }
}