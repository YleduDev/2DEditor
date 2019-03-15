/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UniRx;
using UnityEngine.EventSystems;
using TDE;

namespace QFramework.TDE
{
    /// <summary>/// 辅助点中心对象/// </summary>
    public class Corner
    {
        public bool Hbo = false;
        public bool wbo = false;
        public Transform leftUp;
        public Transform rightUp;
        public Transform leftDown;
        public Transform rightDown;

        public Vector3 leftUpPos;
        public Vector3 rightUpPos;
        public Vector3 leftDownPos;
        public Vector3 rightDownPos;
        public Corner(Transform leftUp, Transform rightUp, Transform leftDown, Transform rightDown)
        {
            this.leftUp = leftUp;
            this.rightUp = rightUp;
            this.leftDown = leftDown;
            this.rightDown = rightDown;
        }
        public void InitValue(T_Graphic model)
        {
            leftUpPos = leftUp.position;
            rightUpPos = rightUp.position;
            leftDownPos = leftDown.position;
            rightDownPos = rightDown.position;
            Hbo = (model.height.Value > 0f);
            wbo = (model.widht.Value > 0f);
        }
    }
    public partial class UICornerDrag : MonoBehaviour, IDragHandler,IBeginDragHandler
    {
        T_Graphic model;
        Corner center;
        public ScaleCenter ScaleCenter= ScaleCenter.RightDown;

        public void Init(T_Graphic model, Corner center)
        {
            this.model = model;
            this.center = center;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            center.InitValue(model);       
        }

        public void OnDrag(PointerEventData eventData)
        {
            SelectorFactory.CreateUIScaleSelector(ScaleCenter).Drag(model, eventData, center);
        }
       
    }
}