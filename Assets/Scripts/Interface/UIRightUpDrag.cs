using UnityEngine;
using QFramework.TDE;
using UnityEngine.EventSystems;

namespace TDE
{
    public class UIRightUpDrag : IUIScaleDrag
    {
        public void Drag(T_Graphic model, PointerEventData eventData, Corner center)
        {
            Vector2 LocalPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(center.parent
                , eventData.position, eventData.pressEventCamera, out LocalPoint);
            //3¸ö¹Ì¶¨µÄ
            Vector2 leftUpScreenPosition = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, center.leftUpPos);
            Vector2 leftDownScreenPosition = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, center.leftDownPos);
            Vector2 rightDownScreenPosition = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, center.rightDownPos);

            Vector2 leftDownLocalPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(center.parent
                , leftDownScreenPosition, eventData.pressEventCamera, out leftDownLocalPoint);

            Vector2 a = rightDownScreenPosition - leftDownScreenPosition;
            Vector2 b = eventData.position  - leftDownScreenPosition;
            Vector2 c = leftUpScreenPosition - leftDownScreenPosition;

            model.localPos.Value = center.beginLocalPosVale + (LocalPoint - center.beginDargLocalPoint) * .5f;

            float length = (LocalPoint - leftDownLocalPoint).magnitude;

            float dotForHeight = Vector2.Dot(c.normalized, b.normalized);
            float dotForWidht = Vector2.Dot(a.normalized, b.normalized);

            model.widht.Value = center.wbo ? length * dotForWidht : -length * dotForWidht;
            model.height.Value = center.Hbo ? length * dotForHeight : -length * dotForHeight;
        }
    }
}
