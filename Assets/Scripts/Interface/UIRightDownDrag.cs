using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework.TDE;
using UnityEngine.EventSystems;
using QFramework;

namespace TDE
{
    public class UIRightDownDrag : IUIScaleDrag
    {
        public void Drag(T_Graphic model, PointerEventData eventData, Center center)
        {
            //3¸ö¹Ì¶¨µÄ
            model.localPos.Value += eventData.delta * .5f;

            Vector2 leftUpScreenPosition = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, center.leftUpPos);
            Vector2 leftDownScreenPosition = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, center.leftDownPos);
            Vector2 rightUpScreenPosition= RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, center.rightUpPos);

            Vector2 a = leftDownScreenPosition - leftUpScreenPosition;
            Vector2 b = eventData.position - leftUpScreenPosition;
            Vector2 c = rightUpScreenPosition - leftUpScreenPosition;

            float length = b.magnitude;
            float dotForHeight= Vector2.Dot(a.normalized, b.normalized);
            float dotForWidht= Vector2.Dot(c.normalized, b.normalized);

            model.widht.Value = center.wbo? length * dotForWidht: -length * dotForWidht;
            model.height.Value =center.Hbo? length * dotForHeight: -length * dotForHeight;
        }
    }
}
