/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using TDE;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace QFramework.TDE
{
    public partial class UIimg : UIElement,IDragHandler,IBeginDragHandler,IEndDragHandler
	{
        TSceneData model;
        string spriteFullName;
        RectTransform rectGraphicView;
        RectTransform tParent;
        Vector2 localPoint;
        Vector2 tLocalPoint;
        bool bo;
        //生成锁
        bool generateLock = false;

        FloatReactiveProperty widht;
        FloatReactiveProperty height;
        StringReactiveProperty spritrsStr;

        T_Graphic T_Graphic = null;
        private void Awake(){}
		protected override void OnBeforeDestroy(){}
        internal void Init(string spriteFullName,RectTransform Viewport,TSceneData model)
        {
            this.spriteFullName = spriteFullName;
            this.rectGraphicView = Viewport;
            this.model = model;
            Sprite sprite= Global.GetSprite(spriteFullName);
            //graghic data 数据
            spritrsStr = new UniRx.StringReactiveProperty(spriteFullName);
            widht = new UniRx.FloatReactiveProperty(sprite.rect.width);
            height = new UniRx.FloatReactiveProperty(sprite.rect.height);        
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
            UIGraphicMenuPanel.TitleSprite.Value = Global.GetSprite(spriteFullName);
            bo = false;
            generateLock = false;
            T_Graphic = null;
            
        }
        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectGraphicView, eventData.position, eventData.pressEventCamera, out localPoint);

            //过滤
            if (!FilterLocalPosForUIGraphicVw(localPoint, out bo))
            {
                //可以生成
                if (bo&&!generateLock) { T_Graphic=GenerationGraohic(); model.Add(T_Graphic); generateLock = true; }
                if (bo&&T_Graphic.IsNotNull())
                {

                    if (T_Graphic.graphicType == GraphicType.Image) tParent = Global.imageParent;
                    else tParent = Global.textParent;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(tParent, eventData.position, eventData.pressEventCamera, out tLocalPoint);
                    if (Global.GetLocalPointOnCanvas(tLocalPoint)) T_Graphic.localPos.Value = tLocalPoint;
                }
                if (bo&&UIGraphicMenuPanel.TitleImgActive.Value) UIGraphicMenuPanel.TitleImgActive.Value = false;
                return;
            }
            UIGraphicMenuPanel.TitleImgLocalPos.Value = localPoint;
            if (!UIGraphicMenuPanel.TitleImgActive.Value) UIGraphicMenuPanel.TitleImgActive.Value = true;
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            UIGraphicMenuPanel.TitleImgActive.Value = false;
        }

        private T_Graphic GenerationGraohic()
        {
            if (spritrsStr.Value.Contains(Global.TextItemContainName))
            {
                return new T_Text() { spritrsStr = spritrsStr, widht = widht, height = height };
            }
            return new T_Image() { spritrsStr = spritrsStr, widht = widht, height = height };
        }
    }
}