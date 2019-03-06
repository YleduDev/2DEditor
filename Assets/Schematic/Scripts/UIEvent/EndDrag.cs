using UnityEngine;
using UnityEngine.EventSystems;

public class EndDrag : MonoBehaviour,IDragHandler,IEndDragHandler,IBeginDragHandler{

    private LineForSchematic lineSchematic;
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        Transform tf = transform;
        while (!lineSchematic)
        {
            if (!tf.parent) return;
            lineSchematic = tf.GetComponentInParent<LineForSchematic>();
            tf = tf.parent;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        lineSchematic.ChangePointB(eventData.position, eventData.pressEventCamera);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 pointB = beginPointB;
        Vector3 point = eventData.position;
        Camera camera = eventData.pressEventCamera;
        CommandManager.CommandMan.AddCammand(
           () =>
           {
               lineSchematic.ChangePointB(point, camera);
           },
           () =>
           {
               lineSchematic.ChangePointB(pointB,camera);
           }
           );

    }
    private Vector3 beginPointB;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!lineSchematic) Init();
        beginPointB = lineSchematic.screenB;
    }
}
