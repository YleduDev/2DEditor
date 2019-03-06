using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// UI精准拖拽
/// </summary>
public class LineDrag :SchematicUIEvent,IEndDragHandler
{
    private LineForSchematic lineForSchematic;

    public override void Init()
    {
        GetLineSchematic();
        baseGraphic = lineForSchematic;
        tf = lineForSchematic.transform;
        rtf = transform as RectTransform;
    }
 
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        //如果是编辑状态，单击线段元素，就想目标元素设为空
        if (SchematicControl.Instance.schematicType == SchematicTransformType.Seting)
            SchematicControl.Instance.SetTargetOb(null);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        //如果是编辑模式并且点中箭头、半弧 、缩放框 退出
        if (SchematicControl.Instance.schematicType == SchematicTransformType.Seting && SchematicControl.Instance.ImageGizmo.selectedAxis != Axis.None) return;
        //添加 撤回
        Vector3 begin = orgenPos;
        Vector2 eventDataPos = eventData.position;
        Vector3 offsetNew = offset;
        Camera camera = eventData.pressEventCamera;

        CommandManager.CommandMan.AddCammand(
             () => {
                 Vector3 worldPoint;
                 RectTransformUtility.ScreenPointToWorldPointInRectangle(rtf, eventDataPos, camera, out worldPoint);
                 tf.position = worldPoint + offsetNew;
                 
                 //重置线段起末
                 lineForSchematic.DragTOInit(tf.position, camera);
             },
             () => {
                 tf.position = begin;
                 //重置线段起末
                 lineForSchematic.DragTOInit(begin, camera);
             });
    }

    private void GetLineSchematic()
    {
        if (lineForSchematic) return;
        lineForSchematic = GetComponent<LineForSchematic>();
        if (!lineForSchematic)
            lineForSchematic = GetComponentInParent<LineForSchematic>();
    }
}
