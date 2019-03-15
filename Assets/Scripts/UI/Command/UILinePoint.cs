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
        
        public void Init(TSceneData model,Transform parent, T_Image image)
        {
            this.model = model;
            this.image = image;
            this.parent = parent as RectTransform;
        }

        #region MonoEven
        public void OnBeginDrag(PointerEventData eventData)
        {
            line = new T_Line();
            line.height.Value = Global.LineHeight;
            line.lineShapeType.Value = Global.lineShapeType;
            //起点
            line.localOriginPos.Value = image.localPos.Value
            + (Vector2)(image.locaRotation.Value * transform.localPosition);

            model.Add(line);
        }

        public void OnDrag(PointerEventData eventData)
        {
            GameObject go = eventData.pointerCurrentRaycast.gameObject;
            if (go && go.tag == "LinePoint" && go != gameObject)
            {
                T_Image endPointImage = go.GetComponent<UILinePoint>().image;
                line.localEndPos.Value = endPointImage.localPos.Value
                +(Vector2)(endPointImage.locaRotation.Value*go.transform.localPosition);
            }
            else if (go && go.tag == "LinePoint" && go == gameObject) {
                line.localEndPos.Value = image.localPos.Value
                + (Vector2)(image.locaRotation.Value * transform.localPosition);
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
            if (line.widht.Value<Global.minLineLength) { model.Remove(line); return; }

            image.Add(new BindData()
            {
                line = line, LinePointType = LinePointType.Origin,
                LocalPointForImage = new Vector2ReactiveProperty(transform.localPosition),
                width = image.widht.Value,
                height = image.height.Value
            });

            GameObject go = eventData.pointerCurrentRaycast.gameObject;
            if (go && go.tag == "LinePoint" && go != gameObject)
            {
                T_Image endPointImage = go.GetComponent<UILinePoint>().image;
                endPointImage.Add(new BindData() {
                    line = line,
                    LinePointType = LinePointType.End,
                    LocalPointForImage = new Vector2ReactiveProperty(go.transform.localPosition),
                    width = endPointImage.widht.Value,
                    height = endPointImage.height.Value
                });
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
        #endregion
    }
}