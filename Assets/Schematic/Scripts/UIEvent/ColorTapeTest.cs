using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorTapeTest : MonoBehaviour, IPointerDownHandler,IEndDragHandler
{
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("结束");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("开始");
    }
}
