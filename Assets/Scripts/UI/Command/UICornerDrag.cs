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

        //记录model开始拖拽状态的value
        public Vector2 beginLocalPosVale;
        //开始拖拽鼠标的localPos
        public Vector2 beginDargLocalPoint;
        
        public Vector3 leftUpPos;
        public Vector3 rightUpPos;
        public Vector3 leftDownPos;
        public Vector3 rightDownPos;
        public RectTransform parent;
        public Corner(Transform leftUp, Transform rightUp, Transform leftDown, Transform rightDown,RectTransform Parent)
        {
            this.leftUp = leftUp;
            this.rightUp = rightUp;
            this.leftDown = leftDown;
            this.rightDown = rightDown;
            parent = Parent;
        }
        public void InitValue(T_Graphic model,Vector2 pos)
        {
            leftUpPos = leftUp.position;
            rightUpPos = rightUp.position;
            leftDownPos = leftDown.position;
            rightDownPos = rightDown.position;
            Hbo = (model.height.Value > 0f);
            wbo = (model.widht.Value > 0f);
            beginDargLocalPoint = pos;
            beginLocalPosVale = model.localPos.Value;
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
        Vector2 pos;
        public void OnBeginDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(center.parent
              , eventData.position, eventData.pressEventCamera, out pos);
            center.InitValue(model, pos);
        }

        public void OnDrag(PointerEventData eventData)
        {
            SelectorFactory.CreateUIScaleSelector(ScaleCenter).Drag(model, eventData, center);
        }
       
    }
}