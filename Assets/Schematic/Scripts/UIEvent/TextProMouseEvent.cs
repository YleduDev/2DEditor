using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextProMouseEvent : IconMouseEvent {

    private Image image;
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if(!image) image = GetComponent<Image>();
        Color color = image.color;
        color = new Color(color.r, color.g, color.b, 0.5f);
        image.color = color;
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        Color color = image.color;
        color = new Color(color.r, color.g, color.b, 0f);
        image.color = color;
    }
}
