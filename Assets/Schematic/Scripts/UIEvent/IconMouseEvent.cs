using UnityEngine;
using UnityEngine.EventSystems;

public class IconMouseEvent : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler {

    // 要改变的鼠标样式图片
    public Texture2D cursorTexture;
    protected CursorMode cursorMode = CursorMode.Auto;
    protected Vector2 hotSpot = Vector2.zero;


    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width * .4f, cursorTexture.height*0.14f), cursorMode);
        //SchematicControl.Instance.CanDrawRect = false;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, hotSpot, cursorMode);
    }

}
