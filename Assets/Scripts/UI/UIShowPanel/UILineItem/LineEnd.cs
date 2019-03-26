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
using UniRx;

namespace QFramework.TDE
{
	public partial class LineEnd : UIElement,IBeginDragHandler,IDragHandler,IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        Image endImage;
        RectTransform tf;
        T_Line model;
        RectTransform parent;
        Vector2 localPoint;

        public float Width
        {
            get
            {
                if (!tf) tf = transform as RectTransform;
                return tf.rect.width;
            }
        }
        public float Height
        {
            get
            {
                if (!tf) tf = transform as RectTransform;
                return tf.rect.height;
            }
        }

        public Texture2D cursorTexture;

        ResLoader loader = ResLoader.Allocate();

        private void Awake()
        {
            endImage = GetComponent<Image>();
        }

        internal void Init(T_Line model,RectTransform parent)
        {
            this.model = model;
            this.parent = parent;
        }

        public void ChangeSprite(T_Line model)
        {
            endImage.sprite = loader.LoadSprite(model.px.Value.ToString() + model.lineEndShapeType.Value.ToString());
            endImage.SetNativeSize();
        }

        protected override void OnBeforeDestroy()
        {
            loader.Recycle2Cache();
            loader = null;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //½â³ý°ó¶¨
            if (model.bindEndImage.IsNotNull()) { model.bindEndImage.Remove(model.bindEndData); model.bindEndImage = null;}
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parent,
                eventData.position, eventData.pressEventCamera, out localPoint);

            Debug.Log(Global.currentCanvasWidth + "  " + Global.currentCanvasheight);
            
            if (!Global.GetLocalPointOnCanvas(localPoint)) return;
            model.localEndPos.Value = localPoint;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GameObject go = eventData.pointerCurrentRaycast.gameObject;
            if (go && go.tag == "LinePoint")
            {
                T_Image PointImage = go.GetComponent<UILinePoint>().image;
                model.localEndPos.Value = PointImage.localPos.Value
                    + (Vector2)(Global.GetQuaternionForQS(PointImage.locaRotation.Value) * go.transform.localPosition); ;
                BindData Bind = new BindData()
                {
                    line = model,
                    LinePointType = LinePointType.End,
                    LocalPointForImage = new Vector2ReactiveProperty(go.transform.localPosition),
                    width = PointImage.widht.Value,
                    height = PointImage.height.Value
                };
                PointImage.Add(Bind);

                model.bindEndImage = PointImage;
                model.bindEndData = Bind;
            }
        }


        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width * .5f, cursorTexture.height * 0.5f), CursorMode.Auto);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}