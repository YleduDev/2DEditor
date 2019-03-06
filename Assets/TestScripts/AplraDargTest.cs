using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AplraDargTest : MonoBehaviour,IDragHandler,IPointerEnterHandler,IPointerExitHandler {


    public Texture2D hand;
    public Texture2D darg;
    void Start () {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
	}
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("»®Ïß");
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
