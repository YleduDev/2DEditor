/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TDE;
using UnityEngine.EventSystems;

namespace QFramework.TDE
{
	public partial class LineSegment : UIElement,IBeginDragHandler,IDragHandler,IPointerEnterHandler,IPointerExitHandler
	{
        public Image segmentImage;
        RectTransform tf;
        private T_Line model;
        RectTransform parent;

        public float Width
        {
            get
            {
                if (!tf) tf = transform as RectTransform;
                return tf.rect.width;
            }
            set
            {
                if (!tf) tf = transform as RectTransform;
                tf.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);
            }
        }
        public float Height
        {
            get
            {
                if (!tf) tf = transform as RectTransform;
                return tf.rect.height;
            }
            set
            {
                if (!tf) tf = transform as RectTransform;
                tf.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value);
            }
        }

        public Texture2D cursorTexture;
        internal void Init(T_Line line, RectTransform parent)
        {
            model = line;
            this.parent = parent;
        }

        ResLoader loader = ResLoader.Allocate();

        private void Awake()
        {
            segmentImage = GetComponent<Image>();
        }

        public void ChangeSprite(T_Line model)
        {
            segmentImage.sprite = Global.GetSprite(loader.LoadSync<Texture2D>(  "Segment"+ model.px.Value.ToString()));
            segmentImage.SetNativeSize();
        }

        protected override void OnBeforeDestroy()
        {
            loader.Recycle2Cache();
            loader = null;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        Vector2 beginLocalPoint;
        Vector2 dragLocalPoint;
        Vector2 beginDragOriginPosValue;
        Vector2 beginDragEndPosValue;
        public void OnBeginDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, eventData.position,
                eventData.pressEventCamera,out beginLocalPoint);
            beginDragOriginPosValue = model.localOriginPos.Value;
            beginDragEndPosValue = model.localEndPos.Value;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (model.bindBeginImage.IsNull() && model.bindEndImage.IsNull())
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, eventData.position,
                eventData.pressEventCamera, out dragLocalPoint);

               if (!Global.GetLocalPointOnCanvas(dragLocalPoint)) return;

                model.localOriginPos.Value = beginDragOriginPosValue + (dragLocalPoint - beginLocalPoint);
                model.localEndPos.Value = beginDragEndPosValue + (dragLocalPoint - beginLocalPoint);
            }
        }



        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width * .4f, cursorTexture.height * 0.14f), CursorMode.Auto);        
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

    }
}