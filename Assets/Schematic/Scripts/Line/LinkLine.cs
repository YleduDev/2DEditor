using DevelopEngine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI连线功能
/// </summary>
public class LinkLine : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    protected RectTransform rtf;//自身的Rect
    //当前的line游戏对象
    protected GameObject curentLine;
    protected LineForSchematic currentSimpleLine;
    //渲染的摄像机
    protected Camera renderCamera;
    //初始的屏幕坐标
    protected Vector2 originalScrPos;
    //当前鼠标的屏幕坐标
    protected Vector2 currentScrPos;

    //线的最小长度 如果小于，则这条线自动删除
    public float MinLenght = 5f;
    
    protected Vector2 orePos;
    //线条的起始目标物体
    protected GameObject beginTarget;

    public static GameObject endTargetForMouseEnter;
    public static bool mouseInEnter = false;
    protected virtual void Start()
    {
        rtf = transform as RectTransform;
        SchematicControl.Instance.GetPrefab("Schematic/Line/5Line");
        SchematicControl.Instance.GetPrefab("Schematic/Line/10Line");
        SchematicControl.Instance.GetPrefab("Schematic/Line/BoubleArrowLine");
        SchematicControl.Instance.GetPrefab("Schematic/Line/LeftLine");
        SchematicControl.Instance.GetPrefab("Schematic/Line/RightLine");
    }
    //因为线ui可能遮挡的情况。我们使用鼠标事件去区分
    protected void OnMouseEnter()
    {
        mouseInEnter = true;
        LinkLine.endTargetForMouseEnter = this.gameObject;
    }
    protected void OnMouseExit()
    {
        mouseInEnter = false;
    }

    //简单生成线
    protected LineForSchematic SimpleLinePointDownHandle(Vector3 pos,Camera evennCamera)
    {
        //生成线 
        GameObject prefab= SchematicControl.Instance.GetPrefabForLine();
        if (!prefab) ConsoleM.LogError(SchematicControl.Instance.linePrefabPath+"  路径没有正确的线预制体");
        curentLine = SchematicControl.Instance.Create(prefab.name, prefab, pos, Quaternion.identity);
        curentLine.SetActive(false);
        //设置父子级
        Transform parent = SchematicControl.Instance.LineParent;
        curentLine.transform.SetParent(parent, false);
        // print(curentLine.transform.position);
        RectTransform rt = curentLine.GetComponent<Transform>() as RectTransform;

        //转换成UI坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, pos, evennCamera, out orePos);

        rt.anchoredPosition = orePos;
        //赋值当前
        currentSimpleLine = curentLine.GetComponentInChildren<LineForSchematic>();
        currentSimpleLine.mainColor = curentLine.GetComponent<Image>().color;
        currentSimpleLine.prefabPath = SchematicControl.Instance.linePrefabPath;
        currentSimpleLine.bindData.RelieveBind();
        return currentSimpleLine;
    }
    //点击的初始世界位置
    protected Vector3 orgenPos;
    public virtual void OnDrag(PointerEventData eventData)
    {
        if (SchematicControl.Instance.schematicType != SchematicTransformType.DrawLine)
        {
            //防止点击到坐标系辅助线上
            if (SchematicControl.Instance.ImageGizmo.selectedAxis != Axis.None)
                SchematicControl.Instance.CanDrawRect = false;
            return;
        }
        if (!curentLine.activeSelf) curentLine.SetActive(true);
       // Debug.Log(eventData.position+ "   Input.mousePosition:"+ Input.mousePosition);
        currentSimpleLine.InitData(originalScrPos, eventData.position, eventData.pressEventCamera);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        //拖拽模式
        if (SchematicControl.Instance.schematicType != SchematicTransformType.DrawLine) return;
        GameObject go = eventData.pointerCurrentRaycast.gameObject;
        if (LinkLine.mouseInEnter) go = LinkLine.endTargetForMouseEnter;
        // 距离过短或者起末都是同一个图元就删除
        if ((eventData.position - originalScrPos).magnitude <= MinLenght)
        {
            currentSimpleLine.MyDestroy(); 
            SchematicControl.Instance.SetSelectsForEmpty();
            return;          
        }
        if ( go != null && go == beginTarget)
        {
            currentSimpleLine.MyDestroy();
            SchematicControl.Instance.SetSelectsForEmpty();
            return; 
        }

        Vector3 endPos = Input.mousePosition; /*RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, eventData.pointerCurrentRaycast.gameObject.transform.position);*/

        //注册撤销重做
        List<BaseGraphicForSchematic> liseBaseSch=SchematicControl.Instance.GetSelects();
        LineForSchematic simpleLine = currentSimpleLine;
        Vector2 begin = originalScrPos;

        Camera camera = eventData.pressEventCamera;
        Vector3 goPos = go.transform.position;
        CommandManager.CommandMan.AddCammand(() => 
        {
            if (!simpleLine.gameObject.activeSelf)
            {
                simpleLine.gameObject.SetActive(true);
                SchematicControl.Instance.Add(simpleLine.gameObject);
            }//SimpleLinePointDownHandle(orgenPos, eventData.pressEventCamera);
            //如果鼠标位置是划线目标
            if (SchematicControl.Instance.bind && go && go.tag == "Icon")
            {
                endPos = RectTransformUtility.WorldToScreenPoint(camera, goPos);
                //划线
                simpleLine.InitData(begin, endPos, eventData.pressEventCamera);
                //注册
                SchematicUIEvent uiDrag = go.GetComponent<SchematicUIEvent>();
                if (uiDrag)
                {
                    simpleLine.Bind(uiDrag, BindTpye.B);
                }
            }
            else
            {
                simpleLine.InitData(begin, endPos, eventData.pressEventCamera);
            }
            SchematicControl.Instance.SetSelects(simpleLine);
        },
        () => 
        {
            simpleLine.MyDestroy(); 
            SchematicControl.Instance.SetSelects(liseBaseSch);
        });
    }


    public virtual void OnPointerDown(PointerEventData eventData)
    {
        //当前选中的线为空哦
        SchematicControl.Instance.RemovePanelChild();
        //设置框选条件
        if (SchematicControl.Instance.schematicType != SchematicTransformType.DrawLine)
        {
            //Debug.Log(SchematicControl.Instance.ImageGizmo.selectedAxis);
            if (SchematicControl.Instance.ImageGizmo.selectedAxis == Axis.None)
                SchematicControl.Instance.CanDrawRect = true;
                return;
        }
        originalScrPos = eventData.position;
        orgenPos = eventData.position;
        //Debug.Log(orgenPos);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rtf, eventData.position, eventData.pressEventCamera, out orgenPos);
        //如果鼠标点击UI是划线目标
        if (SchematicControl.Instance.bind&&eventData.pointerCurrentRaycast.gameObject.tag == "Icon")
        { 

            //获取并赋值线游戏对象的初始值
            beginTarget = eventData.pointerCurrentRaycast.gameObject;
            orgenPos = beginTarget.transform.position;
            originalScrPos = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, orgenPos);
            //动态生成线 并绑定起点委托
            LineForSchematic sL = SimpleLinePointDownHandle(orgenPos, eventData.pressEventCamera);
            SchematicUIEvent uiDrag = beginTarget.GetComponent<SchematicUIEvent>();
            if (uiDrag)
            {
                sL.Bind(uiDrag, BindTpye.A);
            }

        }
        else
        //一般 划线
        SimpleLinePointDownHandle(orgenPos,eventData.pressEventCamera);
    }

}
