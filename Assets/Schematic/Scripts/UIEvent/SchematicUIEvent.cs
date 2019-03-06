using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public delegate void DargICEvent( Vector3 v3,Camera camera);
/// <summary>
/// UI精准拖拽
/// </summary>
public class SchematicUIEvent : MonoBehaviour,IPointerDownHandler,IDragHandler,IPointerClickHandler,IEndDragHandler,IBeginDragHandler
{
    protected RectTransform rtf;//自身的Rect
    protected BaseGraphicForSchematic baseGraphic;
    protected Transform tf;
    protected bool isDrag = false;//因为拖拽结束事件和点击事件共存，且点击事件线发生，所有做个标志位
    protected virtual void Start()
    {
        Init();
    }
    public virtual void Init()
    {
        tf = transform;
        rtf = transform as RectTransform;
        baseGraphic = GetComponent<BaseGraphicForSchematic>();
    }
    //1.按下时记录偏移量 
    protected Vector3 offset;
    //图元拖拽挂载委托
    public DargICEvent dargICEvent;
    protected Vector3 orgenPos;
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        orgenPos = tf.position;
        //(1)将光标的屏幕坐标转换为世界坐标
        Vector3 worldPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rtf, eventData.position, eventData.pressEventCamera, out worldPoint);
        //(2)记录偏移量
        offset = tf.position - worldPoint;
    }

    //2.拖拽时修改UI位置 
    public virtual void OnDrag(PointerEventData eventData)
    {
        //如果是编辑模式且鼠标点击到方向轴  退出
        if (SchematicControl.Instance.schematicType == SchematicTransformType.Seting && SchematicControl.Instance.ImageGizmo.selectedAxis != Axis.None) return;
        Vector3 worldPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rtf, eventData.position, eventData.pressEventCamera, out worldPoint);
        tf.position = worldPoint + offset;         
    }


    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (isDrag) return;
        if (!baseGraphic) Init();
        BaseGraphicForSchematic baseSch = baseGraphic;
        List< BaseGraphicForSchematic> listBs=SchematicControl.Instance.GetSelects();
        //防止无意义的一直重复添加注册 导致撤销重做功能不友好
        if(listBs!=null&& listBs.Count == 1 && baseSch.Equals(listBs[0])) { }
        else
        {
            CommandManager.CommandMan.AddCammand(() => {
                SchematicControl.Instance.SetSelects(baseSch);
            }, () => {
                SchematicControl.Instance.SetSelects(listBs);
            });
        }      
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
    }
}
