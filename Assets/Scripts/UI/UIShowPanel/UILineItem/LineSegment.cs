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
	public partial class LineSegment : UIElement,IBeginDragHandler,IDragHandler
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
            segmentImage.sprite = loader.LoadSprite(model.px.Value.ToString() + "Segment");
            segmentImage.SetNativeSize();
        }

        protected override void OnBeforeDestroy()
        {
            loader.Recycle2Cache();
            loader = null;
        }


        public void OnBeginDrag(PointerEventData eventData){}

        public void OnDrag(PointerEventData eventData)
        {
            if (model.bindBeginImage.IsNull() && model.bindEndImage.IsNull())
            {
                model.localOriginPos.Value += eventData.delta;
                model.localEndPos.Value += eventData.delta;
            }
        }
    }
}