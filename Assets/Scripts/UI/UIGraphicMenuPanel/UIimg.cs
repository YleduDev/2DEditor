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
        string spriteKey;
        RectTransform rectGraphicView;
        RectTransform tParent;
        Vector2 localPoint;
        Vector2 tLocalPoint;

        //生成标识符
        bool bo;
        //生成锁
        bool generateLock = false;

        float widht;
        float height;
        string spritrsStr;

        T_Graphic T_Graphic = null;
        private string textrueData;
        //是否是本地配置的图元
        public bool isConfigImg = false;

        ResLoader loader = ResLoader.Allocate();
        private void Awake(){}
		protected override void OnBeforeDestroy(){

            loader.Recycle2Cache();
            loader = null;
        }
        internal void Init(string key,RectTransform Viewport)
        {
            this.spriteKey = key;
            this.rectGraphicView = Viewport;
            Sprite sprite = Global.GetSprite(spriteKey);
            //graghic data 数据
            spritrsStr = spriteKey;
            widht = sprite.rect.width;
            height =sprite.rect.height;        
        }

        public void SettextrueData(string data)
        {
            if (data != null && data.Length > 0) textrueData = data;
           // Log.I(textrueData.Length);
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
            UIGraphicMenuPanel.TitleSprite.Value = Global.GetSprite(spriteKey);
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
                if (bo&&!generateLock)
                {
                    generateLock = true;
                   T_Graphic =GenerationGraohic();
                    Global.CurrentSceneDataAddGraphic(T_Graphic);
                }
                if (bo&&T_Graphic.IsNotNull())
                {
                    if (T_Graphic.graphicType == GraphicType.Image) tParent = Global.imageParent;
                    else tParent = Global.textParent;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(tParent, eventData.position, eventData.pressEventCamera, out tLocalPoint);
                    if (Global.GetLocalPointOnCanvas(tLocalPoint))
                        T_Graphic.localPos.Value = tLocalPoint;
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
            //判断不是本地图片  //添加缓存
            if (!textrueData.IsNullOrEmpty())
               Global.CurrentSceneDataAddTextrueData(spritrsStr, textrueData);
            //Text的约定
            if (spritrsStr.Contains(Global.TextItemContainName))
            {
                return new T_Text() { spritrsStr = new StringReactiveProperty( spritrsStr), widht = new FloatReactiveProperty( widht), height = new FloatReactiveProperty( height) };
            }
            return new T_Image(){
                spritrsStr =new StringReactiveProperty( spritrsStr),
                widht = new FloatReactiveProperty( widht),
                height = new FloatReactiveProperty( height),
            };
        }
    }
}