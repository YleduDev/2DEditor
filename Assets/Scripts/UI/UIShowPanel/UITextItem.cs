/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TDE;
using UnityEngine.EventSystems;

namespace QFramework.TDE
{
	public partial class UITextItem : UIElement, IDragHandler, IBeginDragHandler, IPointerClickHandler
    {
        Vector2 offset;
        Vector2 localPoint;
        RectTransform canvasRT;

        public T_Text model;

        private void Awake()
		{
		}

		protected override void OnBeforeDestroy()
		{
		}

        internal void Init(T_Graphic graphicItem, Transform parent)
        {
            model = graphicItem as T_Text;
            EditorBoxInit(model);
            canvasRT = UIManager.Instance.RootCanvas.transform as RectTransform;

            this.transform.Parent(parent)
                .Show()
                .LocalPosition(graphicItem.localPos.Value)
                .LocalScale(graphicItem.localScale.Value)
                .LocalRotation(graphicItem.locaRotation.Value);

        }

        //待优化 设计方式不太理想
        private void EditorBoxInit(T_Graphic model)
        {
            UIRotate uRot = UIRotate.GetComponent<UIRotate>();
            uRot.Init(model, transform);

            UIDrag LeftDownUIDrag = UILeftDown.GetComponent<UIDrag>();
            LeftDownUIDrag.Init(model, new Center(UILeftUP.transform, UIRigghtUP.transform
                , UILeftDown.transform, UIRightDown.transform
                ));
            UIDrag LeftUpUIDrag = UILeftUP.GetComponent<UIDrag>();
            LeftUpUIDrag.Init(model, new Center(UILeftUP.transform, UIRigghtUP.transform
                , UILeftDown.transform, UIRightDown.transform
                ));
            UIDrag RigghtUpUIDrag = UIRigghtUP.GetComponent<UIDrag>();
            RigghtUpUIDrag.Init(model, new Center(UILeftUP.transform, UIRigghtUP.transform
                , UILeftDown.transform, UIRightDown.transform
                ));
            UIDrag RightDownUIDrag = UIRightDown.GetComponent<UIDrag>();
            RightDownUIDrag.Init(model, new Center(UILeftUP.transform, UIRigghtUP.transform
                , UILeftDown.transform, UIRightDown.transform
                ));

        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            //(1)将光标的屏幕坐标转换为世界坐标
            //RectTransformUtility.ScreenPointToWorldPointInRectangle(transform as RectTransform, eventData.position, eventData.pressEventCamera, out worldPoint);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT, eventData.position, eventData.pressEventCamera, out localPoint);
            //(2)记录偏移量
            offset = (Vector2)transform.localPosition - localPoint;
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT, eventData.position, eventData.pressEventCamera, out localPoint);
            model.localPos.Value = localPoint + offset;
        }

        //点击选中
        public void OnPointerClick(PointerEventData eventData)
        {
            Global.OnClick(model);
        }
    }
}