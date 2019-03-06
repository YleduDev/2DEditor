using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DargUITest : MonoBehaviour,IBeginDragHandler,IDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    private Vector3 offset;
    public Texture2D hand;
    public void OnBeginDrag(PointerEventData eventData)
    {
        //(1)将光标的屏幕坐标转换为世界坐标
        Vector3 worldPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(transform as RectTransform, eventData.position, eventData.pressEventCamera, out worldPoint);
        //(2)记录偏移量
        offset = transform.position - worldPoint;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(transform as RectTransform, eventData.position, eventData.pressEventCamera, out worldPoint);
        //print(rtf + "   eventDataPos:" + eventDataPos+ "    eventData.pressEventCamera"+ eventData.pressEventCamera);
        transform.position = worldPoint + offset;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(hand, new Vector2(hand.width * .5f, hand.height * .5f), CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor( null, Vector2.zero, CursorMode.Auto);
    }
}
