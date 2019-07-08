using QFramework;
using TDE;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.EventSystems;

public abstract class UIWidget : UIElement, IDragHandler, IBeginDragHandler, IPointerDownHandler
{
    protected Vector2 offset;
    protected Vector2 localPoint;
    protected TSceneData model;
    protected T_Widget widget;
    protected RectTransform parentRT;
    protected RectTransform rect;
    protected Image UIimage;
    //初始化绑点数据(只做一次）
    protected bool AssetNodeDataOnInit = false;

    public virtual void Init(TSceneData model, T_Widget widget)
    {
        this.model = model;
        this.widget = widget;
        parentRT = Global.imageParent;
        rect = transform as RectTransform;
        this.transform.Parent(parentRT) 
            .Show()
            .LocalPosition(widget.localPos.Value)
            .LocalScale(widget.localScale.Value)
            .LocalRotation(Global.GetQuaternionForQS(widget.locaRotation.Value))
            .ApplySelfTo(self => { UIimage = self.GetComponent<Image>(); });

    }
    protected virtual void SubscribeInit()
    {
        this //移动
              .ApplySelfTo(self => self.widget.localPos.Subscribe(
               v2 => { self.widget.TransformChange(); self.LocalPosition(v2);}))
              //大小
              .ApplySelfTo(self => self.widget.localScale.Subscribe(
               v3 => rect.LocalScale(v3)))
              //旋转
              .ApplySelfTo(self => self.widget.locaRotation.Subscribe(
               qua => { self.widget.TransformChange(); rect.LocalRotation(Global.GetQuaternionForQS(qua)); }))
               //颜色
               .ApplySelfTo(self => self.widget.mainColor.Subscribe(color => UIimage.color = Global.GetColorCS(color)))
              //gao
              .ApplySelfTo(self => self.widget.height.Subscribe(f => { self.widget.TransformChange(); rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, f); }))
              //kuan
              .ApplySelfTo(self => self.widget.widht.Subscribe(f => { self.widget.TransformChange(); rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, f); }));
    }
    #region MonoEvent
    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, eventData.position, eventData.pressEventCamera, out localPoint);
        //(2)记录偏移量
        offset = (Vector2)transform.localPosition - localPoint;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, eventData.position, eventData.pressEventCamera, out localPoint);
        if (!Global.GetLocalPointOnCanvas(localPoint)) return;
        widget.localPos.Value = localPoint + offset;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        Global.OnClick(widget);
    }
    #endregion

}
