using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework.TDE;
using UnityEngine.EventSystems;

namespace TDE
{
    public interface IUIScaleDrag
    {

        void Drag(T_Graphic model, PointerEventData eventData, Corner center);


    }
}
