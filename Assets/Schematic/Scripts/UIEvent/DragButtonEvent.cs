using DevelopEngine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 左侧图标按钮管理类
/// </summary>
public class DragButtonEvent : MonoBehaviour,IPointerExitHandler,IPointerDownHandler,IDragHandler,IEndDragHandler
{
    public string pelPrefabPathPrefix = "Schematic/IC/Icon"; 
    protected string key; 
    //能否生产的bo
    protected bool canCretat = false; 
    //自身rect
    protected RectTransform rtf;

    protected GameObject go;
    protected GameObject prefab;
    public static string fileNane;
    //标准位 图元物体的拖拽
    protected bool noDrag = false;
    protected void Start()
    {
       prefab = Resources.Load<GameObject>(pelPrefabPathPrefix);
       rtf = transform as RectTransform;
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        key =eventData.pointerCurrentRaycast.gameObject.name;
        canCretat = true;     
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (!Input.GetMouseButton(0)||!canCretat) return;
        //生成 物体 
        if (!prefab) ConsoleM.LogError(pelPrefabPathPrefix + "目录下没有预制体");
        go = SchematicControl.Instance.Create(key, prefab,prefab.transform.position,Quaternion.identity);
        Vector3 worldPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rtf, eventData.position, eventData.pressEventCamera, out worldPoint);
        go.transform.position = worldPoint;      
        canCretat = false;
    }

    protected virtual void OnMouseEnter()
    {
        noDrag = true;
    }
    protected virtual void OnMouseExit()
    {
        noDrag = false;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (go&&!noDrag) {
            Vector3 worldPoint;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rtf, eventData.position, eventData.pressEventCamera, out worldPoint);
            go.transform.position = worldPoint;
                }
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        Vector2 newV2 = eventData.position;
        GameObject target = go;
        BaseGraphicForSchematic baseSch = target.GetComponentInChildren<BaseGraphicForSchematic>();
        List<BaseGraphicForSchematic>lastSelects = SchematicControl.Instance.GetSelects();
        Camera camera = eventData.pressEventCamera;
        CommandManager.CommandMan.AddCammand(()=> {
            if (!target.activeSelf)
            {
                target.SetActive(true);
                SchematicControl.Instance.Add(target);
            }
            if (target && !noDrag)
            {
                 //位置
                 Vector3 worldPoint;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(rtf, newV2, camera, out worldPoint);
                target.transform.position = worldPoint;
                //拖拽结束 聚焦物体
               // target.GetComponent<SchematicUIEvent>()?.OnPointerClick(null);
                //当前目标物体制空
                go = null;
            }
        },
        ()=> {
            baseSch.MyDestroy();
            SchematicControl.Instance.SetSelects(lastSelects);
        });

    }


 
}
