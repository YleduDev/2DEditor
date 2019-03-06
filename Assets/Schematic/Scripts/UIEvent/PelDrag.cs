using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// UI精准拖拽
/// </summary>
public class PelDrag : SchematicUIEvent
{
    //2.拖拽时修改UI位置 
    public override void OnDrag(PointerEventData eventData)
    {
        Transform target = SchematicControl.Instance.ImageGizmo.GetTargetTF();
        //判断 当前是拖拽模式并且自身是目标物体 就退出
        if (SchematicControl.Instance.ImageGizmo.type != RuntimeGizmos.TransformType.Move
            && target != null && target.Equals(transform)) return;

        base.OnDrag(eventData);

        //检测委托
        Vector2 v2 = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, transform.position);
        dargICEvent?.Invoke(v2, eventData.pressEventCamera);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if (SchematicControl.Instance.schematicType == SchematicTransformType.Seting && SchematicControl.Instance.ImageGizmo.selectedAxis != Axis.None) return;
        Transform target = SchematicControl.Instance.ImageGizmo.GetTargetTF();
        //判断 当前是拖拽模式并且自身是目标物体 就退出
        if (SchematicControl.Instance.ImageGizmo.type != RuntimeGizmos.TransformType.Move
            && target != null && target.Equals(transform)) return;

        //添加 撤回
        Vector3 begin = orgenPos;
        Vector2 eventDataPos = eventData.position;
        Vector3 offsetNew = offset;
        Camera camera = eventData.pressEventCamera;

        CommandManager.CommandMan.AddCammand(
             () => {
                 Vector3 worldPoint;
                 RectTransformUtility.ScreenPointToWorldPointInRectangle(rtf, eventDataPos, camera, out worldPoint);
                 //print(rtf + "   eventDataPos:" + eventDataPos+ "    eventData.pressEventCamera"+ eventData.pressEventCamera);
                 tf.position = worldPoint + offsetNew;
             },
             () => {

                 tf.position = begin;
             });
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if (isDrag) return;
        if (SchematicControl.Instance.schematicType==SchematicTransformType.Seting)
        SchematicControl.Instance.SetTargetOb(gameObject);
    }
}