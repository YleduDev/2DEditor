using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace TDE
{
    public class UIRotate : MonoBehaviour,IDragHandler
    {
        T_Graphic model;
        Transform itemTF;

        public void Init(T_Graphic model, Transform target)
        {
            this.model = model;
            itemTF = target;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 curScreenPosition = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, itemTF.position);
            Vector2 directionTo = eventData.position-curScreenPosition;
            model.locaRotation.Value = new QuaternionSerializer(Quaternion.LookRotation(transform.forward, directionTo));       
        }
    }
}
