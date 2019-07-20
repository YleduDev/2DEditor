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

        if (widget.sceneLoaded.IsNotNull()) widget.sceneLoaded = null;
        if (widget.sceneSaveBefore.IsNotNull()) widget.sceneSaveBefore = null;
        this.widget.sceneLoaded += OnSceneLoadEnd;
        this.widget.sceneSaveBefore += OnSceneBefore;

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
               v2 => {self.widget.TransformChange();self?.LocalPosition(v2);}))
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
              .ApplySelfTo(self => self.widget.widht.Subscribe(f => { self.widget.TransformChange(); rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, f); }))
              //渲染层级 
              .ApplySelfTo(self => self.widget.siblingType.Subscribe(
                     dataType =>
                     {
                         int index;
                         switch (dataType)
                         {
                             case SiblingEditorType.None: break;
                             case SiblingEditorType.UPOne:
                                 index = self.rect.GetSiblingIndex() + 1; self.rect.SetSiblingIndex(index); self.widget.siblingType.Value = SiblingEditorType.None;
                                 break;
                             case SiblingEditorType.DonwOne:
                                 index = self.rect.GetSiblingIndex() - 1; self.rect.SetSiblingIndex(index); self.widget.siblingType.Value = SiblingEditorType.None;
                                 break;
                             case SiblingEditorType.UpEnd:
                                 self.rect.SetAsLastSibling(); self.widget.siblingType.Value = SiblingEditorType.None;
                                 break;
                             case SiblingEditorType.DonwEnd:
                                 self.rect.SetAsFirstSibling(); self.widget.siblingType.Value = SiblingEditorType.None;
                                 break;
                         }
                     }));
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

    public void OnSceneLoadEnd()
    {
        this.rect.SetSiblingIndex(this.widget.localSiblingIndex);
       
    }

    public void OnSceneBefore()
    {
        this.widget.localSiblingIndex = this.rect.GetSiblingIndex();   //this.rect.SetSiblingIndex();

    }
}
