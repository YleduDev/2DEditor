using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UIDrag : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    protected RectTransform rtf;//自身的Rect
    protected virtual void Start()
    {
        rtf = transform as RectTransform;
    }
    protected Vector3 offset;
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        //(1)将光标的屏幕坐标转换为世界坐标
        Vector3 worldPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rtf, eventData.position, eventData.pressEventCamera, out worldPoint);
        //(2)记录偏移量
        offset = transform.position - worldPoint;
    }
    //2.拖拽时修改UI位置 
    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rtf, eventData.position, eventData.pressEventCamera, out worldPoint);
        transform.position = worldPoint + offset;
    }
}
