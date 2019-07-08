/****************************************************************************
 * 2019.5 LAPTOP-R0ONNKOC
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.EventSystems;
using TDE;
using UniRx;

namespace QFramework.TDE
{
	public partial class UIWidgetImg : UIElement, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        string spriteKey;
        RectTransform rectGraphicView;
        RectTransform tParent;
        Sprite sprite;
        T_Widget T_Widget = null;
        Vector2 localPoint;
        Vector2 tLocalPoint;
        //生成标识符
        bool bo;
        //生成锁
        bool generateLock = false;

        float widht;
        float height;
        string spritrsStr;


        private void Awake(){}

		protected override void OnBeforeDestroy(){}

        internal void Init(string spriteKey, RectTransform viewport)
        {
            this.spriteKey = spriteKey;
            this.rectGraphicView = viewport;
            sprite = Global.GetSprite(spriteKey);

            //T_Widget data 数据
            spritrsStr = spriteKey;
            widht = sprite.rect.width;
            height = sprite.rect.height;
        }

        float UIGraphicViewHeight = 0;
        float UIGraphicViewWidth = 0;

        bool FilterLocalPosForUIGraphicVw(Vector2 ve, out bool rightOut)
        {
            if (UIGraphicViewHeight == 0) UIGraphicViewHeight = rectGraphicView.rect.height;
            if (UIGraphicViewWidth == 0) UIGraphicViewWidth = rectGraphicView.rect.width;
            float minX = -0.5f * UIGraphicViewWidth; float maxX = 0.5f * UIGraphicViewWidth;
            float minY = -0.5f * UIGraphicViewHeight; float maxY = 0.5f * UIGraphicViewHeight;
            rightOut = ve.x > maxX;
            return ve.x > minX && ve.x < maxX && ve.y > minY && ve.y < maxY;
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            UIGraphicMenuPanel.TitleImgActive.Value = true;
            UIGraphicMenuPanel.TitleSprite.Value = sprite;
            bo = false;
            generateLock = false;
            T_Widget = null;

        }
        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectGraphicView, eventData.position, eventData.pressEventCamera, out localPoint);

            //过滤
            if (!FilterLocalPosForUIGraphicVw(localPoint, out bo))
            {
                //可以生成
                if (bo && !generateLock)
                {
                    generateLock = true;
                    T_Widget = GenerationGraohic();
                    Global.CurrentSceneDataAddGraphic(T_Widget);
                }
                if (bo && T_Widget.IsNotNull())
                {
                    tParent = Global.imageParent;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(tParent, eventData.position, eventData.pressEventCamera, out tLocalPoint);
                    if (Global.GetLocalPointOnCanvas(tLocalPoint))
                        T_Widget.localPos.Value = tLocalPoint;
                }
                if (bo && UIGraphicMenuPanel.TitleImgActive.Value) UIGraphicMenuPanel.TitleImgActive.Value = false;
                return;
            }
            UIGraphicMenuPanel.TitleImgLocalPos.Value = localPoint;
            if (!UIGraphicMenuPanel.TitleImgActive.Value) UIGraphicMenuPanel.TitleImgActive.Value = true;

        }

        private T_Widget GenerationGraohic()
        {
            T_Image img= new T_Image()
            {
                spritrsStr = new StringReactiveProperty(spritrsStr),
                widht = new FloatReactiveProperty(widht),
                height = new FloatReactiveProperty(height),
            };
            string name = "";
            if (UIGraphicControlContent.WidgetPrefabDict.ContainsKey(spriteKey))
                name = UIGraphicControlContent.WidgetPrefabDict[spriteKey];
            return new T_Widget(img, name);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            UIGraphicMenuPanel.TitleImgActive.Value = false;
        }
    }
}