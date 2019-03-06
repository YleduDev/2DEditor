using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
/// <summary>
/// Panel拖拽面板
/// </summary>
public class PanelDrag : UIDrag,IEndDragHandler{
    private Vector3 orgenPos;

    public override void OnPointerDown(PointerEventData eventData)
    {
        orgenPos = rtf.transform.position;
        base.OnPointerDown(eventData);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 begin = orgenPos;
        Vector2 eventDataPos = eventData.position;
        Vector3 offsetNew = offset;
        Camera camera = eventData.pressEventCamera;

        List<BaseGraphicForSchematic> listBaseSch = SchematicControl.Instance.GetSelects();

        CommandManager.CommandMan.AddCammand(
             () => {
                 Vector3 worldPoint;
                 RectTransformUtility.ScreenPointToWorldPointInRectangle(rtf, eventDataPos, camera, out worldPoint);
                 rtf.position = worldPoint + offsetNew;

                 //线段的起末初始化
                 LineForSchematic[] lineSchs = transform.GetComponentsInChildren<LineForSchematic>();
                 if (lineSchs != null && lineSchs.Length > 0)
                 {
                     foreach (var item in lineSchs)
                     {
                         item.DragTOInit(item.transform.position, camera);
                     }
                 }
                 //重置panelRect
                 ResetPanelRect(listBaseSch, rtf.position, camera);
             },
             () => {
                 rtf.position = begin; 
                 //线段的起末初始化
                 LineForSchematic[] lineSchs = transform.GetComponentsInChildren<LineForSchematic>();
                 if (lineSchs != null && lineSchs.Length > 0)
                 {
                     foreach (var item in lineSchs)
                     {
                         item.DragTOInit(item.transform.position, camera);
                     }
                     ResetPanelRect(lineSchs.ToList<BaseGraphicForSchematic>(), rtf.position, camera);
                 }
                 //重置panelRect
                 ResetPanelRect(listBaseSch, rtf.position, camera);
             });      
    }

    private void ResetPanelRect(List<BaseGraphicForSchematic> listBaseSch,Vector3 worldPos, Camera camera)
    {
        //因为panel中心再左下角 
         Vector2  start=  camera.WorldToScreenPoint(worldPos);
         Vector2 end = start + new Vector2(rtf.rect.width, rtf.rect.height);
        SchematicControl.Instance.SetPanel(start, end, listBaseSch);
    }
}
