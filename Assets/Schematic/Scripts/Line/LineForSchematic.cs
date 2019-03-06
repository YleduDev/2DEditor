using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 表示委托绑定的关系a=a
/// </summary>

public class LineForSchematic : BaseGraphicForSchematic
{
    public LinkType linkType=LinkType.Broken;
    //父物体RectTF
    private RectTransform  CanvasRt;

    //都是屏幕坐标
    [HideInInspector]
    public Vector2 screenA;
    [HideInInspector]
    public Vector2 screenB;
    [HideInInspector]
    public Vector2 pointA;
    [HideInInspector]
    public Vector3 breakWorldPos;
    [HideInInspector]
    public Vector3 breakLocalPos;
    [HideInInspector]
    public Vector2 pointB;
    //[HideInInspector]
    public BindData bindData; 
    //自身宽带
    public float minPlx = 5;
    [HideInInspector]
    public Vector2 formTo;
    Canvas canvas;

    Vector2 pos;
    Vector3 pos1;
    private Color lastColor;
    public Image[] imageArr;

    private RectTransform childRect;

    private SchematicUIEvent bindUIDragA, bindUIDragB;

    private Camera eventCamera;

    //初始化
    protected override void Start()
    {
        base.Start();
        Init();
    }
    // 改变自身长度和方向
    private void Init()
    {
        schematicType = SchematicType.Line;
        //获取渲染UI的摄像机
        Transform tf = transform;
        while (tf.parent != null)
        {
            canvas = tf.parent.GetComponent<Canvas>();
            if (canvas)
            {
                CanvasRt = canvas.transform as RectTransform;
                break;
            }
            tf = tf.parent;
        }
        //bindData.BingInit();
        rect = transform as RectTransform;
        imageArr = GetComponentsInChildren<Image>();
        childRect = (transform.GetChild(0)) as RectTransform;
    }
    
    //屏幕像素位置->LocalPos
    public void InitData(Vector3 A, Vector3 B,Camera camera)
    {
        screenA = A;
        screenB = B;
        eventCamera = camera;
        Link(A, B);
    }
    //屏幕像素位置
    public void ChangePointA(Vector3 pos,Camera camera)
    {
        screenA = pos ; 
        Link(screenA, screenB);
    }
    public void ChangePointB(Vector3 pos, Camera camera)
    {
        screenB = pos;
        Link(screenA, screenB);
    }
    
    //区别于InitData 符合控制器算法
    public void LinklineData(Vector3 A, Vector3 B, Vector3 breaklocalPoint,Vector3 breakWorldPoint,Vector2 a, Vector2 b)
    {
        pointA = A ;
        pointB = B;
        breakWorldPos = breakWorldPoint;
        breakLocalPos = breaklocalPoint;
        if (!CanvasRt) Init();
        if(linkType == LinkType.Broken)
        {
            Vector2 salientPoint;
            //判断折点
            if (b.x == a.x || b.y == a.y)
                salientPoint = (a + b) / 2;
            else
            {
                Vector2 distance = b - a;
                if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
                    salientPoint = new Vector2(a.x, b.y);
                else salientPoint = new Vector2(b.x, a.y);
            }

            formTo = b - salientPoint; 
        }
        else if(linkType == LinkType.Straight)
        {
            formTo = b - ((b + a) / 2);
        }
        

        //屏幕坐标转化成UGUI坐标  
        LineForControl(pointA, pointB, breaklocalPoint, breakWorldPos);
    }
    void Link(Vector3 a,Vector3 b)
    {
        if (!CanvasRt) Init();
        if (eventCamera == null) eventCamera = canvas.worldCamera;
        switch (SchematicControl.Instance.linkType)
        {
            case LinkType.Straight:
                StraightLink(a,b ); linkType = LinkType.Straight;
                break;
            case LinkType.Broken:
                BrokenLink(a, b); linkType = LinkType.Broken;
                break;
            default:
                StraightLink(a, b);
                break;
        }
    }
    //直线
    public virtual void StraightLink( Vector3 a,Vector3 b)
    {
        if (!rect) Init();

        Vector3 worldPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(CanvasRt, a, eventCamera, out worldPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRt, a, eventCamera, out pos);
        pointA = pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRt, b, eventCamera, out pos);
        pointB = pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRt, (a + b) / 2, eventCamera, out pos);
        breakLocalPos = pos;

        Vector3 salientPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(CanvasRt, (a + b) / 2, eventCamera, out salientPoint);
        formTo = b - ((a + b) / 2);

        LineForSalient(worldPos,pointA, pointB, breakLocalPos, salientPoint);
    }
    //折线
    public virtual void BrokenLink(Vector3 a,Vector3 b)
    {
        Vector2 salientPoint;
            //判断折点
            if (b.x == a.x || b.y == a.y)
            salientPoint = (a + b) / 2;
            else
            {
             Vector2 distance = b - a;
             if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
                salientPoint = new Vector2(a.x, b.y);
             else salientPoint = new Vector2(b.x, a.y);
            }
        Vector3 worldPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(CanvasRt, a, eventCamera, out worldPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRt, a, eventCamera, out pos);
        pointA = pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRt, b, eventCamera, out pos);
        pointB = pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRt, salientPoint, eventCamera, out pos);
        breakLocalPos = pos;

        Vector3 midPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(CanvasRt, salientPoint, eventCamera, out midPos);

        formTo = (Vector2)b - salientPoint;
        LineForSalient(worldPos,pointA, pointB, breakLocalPos, midPos);
    }
    //解除
    public void RelieveBind()
    {
        if(bindUIDragA) bindUIDragA.dargICEvent-= this.ChangePointA;
        if (bindUIDragB) bindUIDragB.dargICEvent -= this.ChangePointB;
        bindData.RelieveBind();
    }
    //绑定
    public void Bind(SchematicUIEvent uiDrag, BindTpye tpye)
    {

        if (tpye == BindTpye.B)
        {
            bindUIDragB = uiDrag;
            uiDrag.dargICEvent += this.ChangePointB;
            bindData.BingInitB(tpye, uiDrag.transform.name);
        }
      if(tpye == BindTpye.A)
        {
            bindUIDragA = uiDrag;
            uiDrag.dargICEvent += this.ChangePointA;
            bindData.BingInitA(tpye, uiDrag.transform.name);
        }
    }
    //删除
    public override void MyDestroy()
    {
        RelieveBind();
        SchematicControl.Instance.Delete(this.gameObject);     
    }
    /// 解除绑定并划线
    public override void DragTOInit(Vector3 pos,Camera camera)
    {
      screenA= RectTransformUtility.WorldToScreenPoint(camera, pos);
      Vector2 bPos = RectTransformUtility.WorldToScreenPoint(camera, childRect.position);

      screenB = formTo + bPos;
      Link(screenA, screenB);
      RelieveBind();
    } 
    //设置颜色
    public override void SetColor(Color color)
    {
        if (imageArr!=null && imageArr.Length > 0)
        {
            lastColor = imageArr[0].color;
            foreach (var item in imageArr)
            {
                item.color = color;
            }
        }
    }
    
    //返回原先颜色
    public void RestoreColor()
    {
        if (imageArr != null && imageArr.Length > 0)
        {
            foreach (var item in imageArr)
            {
                item.color = lastColor;
            }
        }

    } 

    private Vector2 ReviseLine(RectTransform rectTransform,Vector2 form,Vector2 to)
    {     
        Vector2 size = rectTransform.sizeDelta;
        //赋值长度
        size.x = Vector3.Distance(to, form);/*CanvasRt.localScale.x)-(1* CanvasRt.localScale.x)*/
        size.y = minPlx;
        rectTransform.sizeDelta = size;

        Vector2 relativePos = to - form;
        Vector3 euler= Quaternion.FromToRotation(Vector3.right, relativePos).eulerAngles;
        //防止UI  z 轴方向相反导致ui事件检测不出来
        euler = new Vector3(-euler.y, euler.y, euler.z);
        rectTransform.eulerAngles = euler;
        return relativePos;
    }

    private void LineForSalient(Vector3 pos,Vector2 a, Vector2 b,Vector2 mid, Vector3 salient)
    {
        rect.position = pos;
        //矫正线段 
        ReviseLine(rect, a, mid);
        //第二段线的位置
        childRect.position = salient;

        breakWorldPos = childRect.localPosition;
        //矫正线段
        ReviseLine(childRect, mid, b);

    }
    private void LineForControl(Vector2 a, Vector2 b, Vector2 mid, Vector3 salient)
    {
        rect.localPosition = a;
        //矫正线段 
        ReviseLine(rect, a, mid);
        //第二段线的位置
        childRect.localPosition = salient;
        breakWorldPos = childRect.localPosition;
        //矫正线段
         ReviseLine(childRect, mid, b);

    }


    public override void SetParent(bool bo)
    {
        transform.SetParent(SchematicControl.Instance.LineParent, bo);
    }
}
