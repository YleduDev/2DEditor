/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.EventSystems;
using TDE;

namespace QFramework.TDE
{
	public partial class UILineSwitch : UIElement, IPointerEnterHandler, IPointerExitHandler, IDragHandler,IBeginDragHandler
    {
        public Texture2D hand;
        public Texture2D darg;

        TSceneData model;
        Transform parent;
        Vector2 localPoint;
        private void Awake()
        {
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        }
        public void Init(TSceneData model,Transform parent)
        {
            this.model = model;
            this.parent = parent;
        }
        protected override void OnBeforeDestroy()
        {
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parent as RectTransform,
                eventData.position, eventData.pressEventCamera, out localPoint);
            
            //Éú³ÉÏß¶Î
            T_Line line = new T_Line();
            line.height.Value = 3f;
            line.widht.Value = 100f;
            line.localPos.Value = localPoint;
            Debug.Log(eventData.position + "     " + localPoint);
            model.Add(line);
        }
        public void OnDrag(PointerEventData eventData)
        {
           
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            Cursor.SetCursor(hand, new Vector2(hand.width * .5f, hand.height * .5f), CursorMode.Auto);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Cursor.SetCursor(darg, new Vector2(darg.width * .5f, darg.height * .5f), CursorMode.Auto);
        }   
    }
}