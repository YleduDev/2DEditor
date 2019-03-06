using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 线段鼠标事件
/// </summary>
public class LineMouseEvent : IconMouseEvent,IPointerClickHandler{

    public Color clickColor;
    private LineForSchematic lineForSchematic;
    private LineForSchematic LineForSchematic
    {
        get
        {
            if (lineForSchematic) return lineForSchematic;
            lineForSchematic = GetComponent<LineForSchematic>();
            if (!lineForSchematic)
                lineForSchematic = GetComponentInParent<LineForSchematic>();
            return lineForSchematic;
        }
    }
    private bool onClick = false;


    //标识符 表示单击状态
    private void Update()
    { 
        if (onClick && Input.GetMouseButtonDown(0))
        {
            LineForSchematic.RestoreColor();
            onClick = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        LineForSchematic.SetColor(clickColor);
        onClick = true;
    }
}
