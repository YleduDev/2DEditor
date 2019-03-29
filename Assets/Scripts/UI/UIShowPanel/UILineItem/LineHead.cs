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
    public partial class LineHead : UIElement, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        Image beginImage;
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

        internal void Init(T_Line model, RectTransform parent)
        {
            this.model = model;
            this.parent = parent;
        }

        private void Awake()
        {
            beginImage = GetComponent<Image>();

        }

        public void ChangeSprite(T_Line model)
        {
            beginImage.sprite = loader.LoadSprite(  model.lineBeginShapeType.Value.ToString()+ model.px.Value.ToString());
            beginImage.SetNativeSize();
        }

        protected override void OnBeforeDestroy()
        {
            loader.Recycle2Cache();
            loader = null;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parent,
                eventData.position, eventData.pressEventCamera, out localPoint);
            if (!Global.GetLocalPointOnCanvas(localPoint)) return;
            model.localOriginPos.Value = localPoint;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GameObject go = eventData.pointerCurrentRaycast.gameObject;
            if (go && go.tag == "LinePoint")
            {
                T_Image PointImage = go.GetComponent<UILinePoint>().image;
                model.localOriginPos.Value = PointImage.localPos.Value 
                    + (Vector2)(Global.GetQuaternionForQS(PointImage.locaRotation.Value) * go.transform.localPosition); ;
                BindData Bind = new BindData()
                {
                    line = model,
                    LinePointType = LinePointType.Origin,
                    LocalPointForImage = new Vector2ReactiveProperty(go.transform.localPosition),
                    width = PointImage.widht.Value,
                    height = PointImage.height.Value
                };
                PointImage.Add(Bind);

                model.bindBeginImage = PointImage;
               // model.bindBeginData = Bind;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //½â³ý°ó¶¨
            if (model.bindBeginImage.IsNotNull()) { model.bindBeginImage.Remove(model, LinePointType.Origin); model.bindBeginImage = null; }
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width * .5f, cursorTexture.height * .5f), CursorMode.Auto);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}