using UnityEngine;
using UnityEngine.EventSystems;

public class BeginDragForLine : MonoBehaviour, IDragHandler,IBeginDragHandler,IEndDragHandler
{
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
        //if (SchematicControl.Instance.schematicType == SchematicTransformType.Seting) return;       
        lineSchematic.ChangePointA(eventData.position, eventData.pressEventCamera);
    }
    private Vector3 beginPointA;
    private float screenRatio;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!lineSchematic) Init();
        beginPointA = lineSchematic.screenA;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 pointA = beginPointA;
        Vector3 point = eventData.position;
        Camera camera = eventData.pressEventCamera;

        CommandManager.CommandMan.AddCammand(
            () => 
            {
                lineSchematic.ChangePointA(point, camera);
            },
            () => 
            {
                lineSchematic.ChangePointA(pointA, camera);
            }
            );
    }
}
