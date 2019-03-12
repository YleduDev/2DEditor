using UnityEngine;
using QFramework.TDE;
using UnityEngine.EventSystems;

namespace TDE
{
    public class UILeftDownDrag : IUIScaleDrag
    {
        public void Drag(T_Graphic model, PointerEventData eventData, Center center)
        {
            //3¸ö¹Ì¶¨µÄ
            model.localPos.Value += eventData.delta * .5f;

            Vector2 rightUpScreenPosition = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, center.rightUpPos);
            Vector2 leftUpScreenPosition = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, center.leftUpPos);
            Vector2 rightDownScreenPosition = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, center.rightDownPos);

            Vector2 a = leftUpScreenPosition - rightUpScreenPosition;
            Vector2 b = eventData.position - rightUpScreenPosition;
            Vector2 c = rightDownScreenPosition - rightUpScreenPosition;

            float length = b.magnitude;
            float dotForHeight = Vector2.Dot(c.normalized, b.normalized);
            float dotForWidht = Vector2.Dot(a.normalized, b.normalized);

            model.widht.Value = center.wbo ? length * dotForWidht : -length * dotForWidht;
            model.height.Value = center.Hbo ? length * dotForHeight : -length * dotForHeight;
        }
    }
}
