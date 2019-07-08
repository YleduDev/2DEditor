/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/
using QFramework;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
namespace TDE
{
    /// <summary> /// 划线连接点/// </summary>
    public partial class UILinePoint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IDragHandler,IBeginDragHandler,IEndDragHandler
    {
        public T_Image image;
        public Texture2D hand;

        TSceneData model;
        RectTransform parent;
        Vector2 localPoint;
        T_Line line;
        
        public void Init(TSceneData model, T_Image image)
        {
            this.model = model;
            this.image = image;
            this.parent = Global.LineParent;
        }

        #region MonoEven
        public void OnBeginDrag(PointerEventData eventData)
        {
            line = new T_Line();
            line.px.Value = Global.LinePx;
            line.lineShapeType.Value = Global.lineShapeType;
            line.lineBeginShapeType.Value = Global.beginShape;
            line.lineEndShapeType.Value = Global.endShape;
            //起点
            line.localOriginPos.Value = image.localPos.Value
            + (Vector2)(Global.GetQuaternionForQS(image.locaRotation.Value) * transform.localPosition);

            model.Add(line);
        }

        public void OnDrag(PointerEventData eventData)
        {
            GameObject go = eventData.pointerCurrentRaycast.gameObject;
            if (go && go.tag == "LinePoint" && go != gameObject)
            {
                T_Image endPointImage = go.GetComponent<UILinePoint>().image;
                line.localEndPos.Value = endPointImage.localPos.Value
                +(Vector2)(Global.GetQuaternionForQS(endPointImage.locaRotation.Value)*go.transform.localPosition);
            }
            else if (go && go.tag == "LinePoint" && go == gameObject) {
                line.localEndPos.Value = image.localPos.Value
                + (Vector2)(Global.GetQuaternionForQS(image.locaRotation.Value) * transform.localPosition);
            }
            else
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parent,
                eventData.position, eventData.pressEventCamera, out localPoint);
                line.localEndPos.Value = localPoint;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //是否符合线段要求
            if (line.direction<Global.minLineLength) { model.Remove(line); return; }
            BindData beiginBind = new BindData()
            {
                line = line,
                LinePointType = LinePointType.Origin,
                LocalPointForImage = new Vector2ReactiveProperty(transform.localPosition),
                width = image.widht.Value,
                height = image.height.Value
            };
            image.Add(beiginBind);

            line.bindBeginImage = image;
           // line.bindBeginData = beiginBind;

            GameObject go = eventData.pointerCurrentRaycast.gameObject;
            if (go && go.tag == "LinePoint" && go != gameObject)
            {
                T_Image endPointImage = go.GetComponent<UILinePoint>().image;
                BindData endBind = new BindData()
                {
                    line = line,
                    LinePointType = LinePointType.End,
                    LocalPointForImage = new Vector2ReactiveProperty(go.transform.localPosition),
                    width = endPointImage.widht.Value,
                    height = endPointImage.height.Value
                };
                endPointImage.Add(endBind);

                line.bindEndImage = endPointImage;
               // line.bindEndData = endBind;
            }
        }
        #region 鼠标图标切换事件

        public void OnPointerEnter(PointerEventData eventData)
        {
            Cursor.SetCursor(hand, new Vector2(hand.width * .5f, hand.height * .5f), CursorMode.Auto);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
        #endregion
        private void OnDestroy()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
        #endregion
    }
}