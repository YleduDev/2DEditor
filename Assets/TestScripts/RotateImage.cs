
using UnityEngine;

using System.Collections;

using UnityEngine.EventSystems;

public class RotateImage : MonoBehaviour, IDragHandler

{

    public void OnDrag(PointerEventData eventData)

    {

        //ÍÏ×§Ðý×ªÍ¼Æ¬

        SetDraggedRotation(eventData);

    }



    private void SetDraggedRotation(PointerEventData eventData)

    {

        Vector2 curScreenPosition = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, transform.position);

        Vector2 directionTo = curScreenPosition - eventData.position;

        Vector2 directionFrom = directionTo - eventData.delta;

        this.transform.rotation *= Quaternion.FromToRotation(directionTo, directionFrom);

    }

}

