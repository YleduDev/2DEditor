using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class DrawRectangle : MonoBehaviour
{
    public Color rectColor = Color.green;
    private Vector3 start = Vector3.zero;//记下鼠标按下位置
    public Material rectMat = null;//画线的材质 不设定系统会用当前材质画线 结果不可控
    private bool drawRectangle = false;//是否开始画线标志
    private List<GameObject> characters;//框选选择物体的集合
    private RectTransform rt;
    public Camera cam;
    void Start()
    {
        SchematicControl.Instance.Panel.SetActive(false);
        rectMat.hideFlags = HideFlags.HideAndDontSave;
        rectMat.shader.hideFlags = HideFlags.HideAndDontSave;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && SchematicControl.Instance.CanDrawRect)
        {
            drawRectangle = true;
            start = Input.mousePosition;//记录按下位置
        }
        else if (Input.GetMouseButtonUp(0) && SchematicControl.Instance.CanDrawRect)
        {
            if (drawRectangle)
            {
                CheckSelection(start, Input.mousePosition);//框选物体
                SchematicControl.Instance.CanDrawRect = false;
            }
            drawRectangle = false;//如果鼠标左键放开 结束画线
        }
    }
    void OnPostRender()
    {
        //画线这种操作推荐在OnPostRender()里进行 而不是直接放在Update，所以需要标志来开启
        if (drawRectangle && SchematicControl.Instance.CanDrawRect)
        {
            Vector3 end = Input.mousePosition;//鼠标当前位置
            GL.PushMatrix();//保存摄像机变换矩阵
            if (!rectMat)
                return;
            rectMat.SetPass(0);
            GL.LoadPixelMatrix();//设置用屏幕坐标绘图
            GL.Begin(GL.QUADS);
            GL.Color(new Color(rectColor.r, rectColor.g, rectColor.b, 0.1f));//设置颜色和透明度，方框内部透明
            GL.Vertex3(start.x, start.y, 0);
            GL.Vertex3(end.x, start.y, 0);
            GL.Vertex3(end.x, end.y, 0);
            GL.Vertex3(start.x, end.y, 0);
            GL.End();
            GL.Begin(GL.LINES);
            GL.Color(rectColor);//设置方框的边框颜色 边框不透明
            GL.Vertex3(start.x, start.y, 0);
            GL.Vertex3(end.x, start.y, 0);
            GL.Vertex3(end.x, start.y, 0);
            GL.Vertex3(end.x, end.y, 0);
            GL.Vertex3(end.x, end.y, 0);
            GL.Vertex3(start.x, end.y, 0);
            GL.Vertex3(start.x, end.y, 0);
            GL.Vertex3(start.x, start.y, 0);
            GL.End();
            GL.PopMatrix();//恢复摄像机投影矩阵
        }

    }

    void CheckSelection(Vector3 start, Vector3 end)
    {
        Vector3 p1 = Vector3.zero;
        Vector3 p2 = Vector3.zero;
        if (start.x > end.x)
        {
            //这些判断是用来确保p1的xy坐标小于p2的xy坐标，因为画的框不见得就是左下到右上这个方向的
            p1.x = end.x;
            p2.x = start.x;
        }
        else
        {
            p1.x = start.x;
            p2.x = end.x;
        }

        if (start.y > end.y)
        {
            p1.y = end.y;
            p2.y = start.y;
        }
        else
        {
            p1.y = start.y;
            p2.y = end.y;
        }
        //判断
        characters = SchematicControl.Instance.GetCharacters();
        if (characters == null && characters.Count <= 0) return;

        List<BaseGraphicForSchematic> listBasesch = new List<BaseGraphicForSchematic>();
        foreach (GameObject obj in characters)
        {
            //把可选择的对象保存在characters数组里
            Vector3 location = cam.WorldToScreenPoint(obj.transform.position);//把对象的position转换成屏幕坐标
            if (location.x < p1.x || location.x > p2.x || location.y < p1.y || location.y > p2.y
                || location.z < cam.nearClipPlane || location.z > cam.farClipPlane)//z方向就用摄像机的设定值，看不见的也不需要选择了
            {
                //Disselecting(obj);//上面的条件是筛选 不在选择范围内的对象，然后进行取消选择操作，比如把物体放到default层，就不显示轮廓线了
            }
            else
            {
                BaseGraphicForSchematic sch = obj.GetComponent<BaseGraphicForSchematic>();
                listBasesch.Add(sch);

                //Selecting(obj);//否则就进行选中操作，比如把物体放到画轮廓线的层去
            }
        }

        //获得上次选择集合
        List<BaseGraphicForSchematic> lastSchs = SchematicControl.Instance.GetSelects();
        //辅助框
        //因为目标为主动框选，所有主动设置panel大小
        if (listBasesch.Count > 0)
        {
            Vector3 newP1 = p1; Vector3 newP2 = p2;
            List<BaseGraphicForSchematic> itemSchs = new List<BaseGraphicForSchematic>() ;
            //判断并获取上次框选数据
            CommandManager.CommandMan.AddCammand(
                () =>
                {
                    //因为这是直接将itemSchs引用复制给selects，如果操作删除（会clear集合导致集合为空）
                    itemSchs.Clear();
                    listBasesch.ForEach(i => { itemSchs.Add(i); });
                    //清空panel 在赋值
                    SchematicControl.Instance.JsutSetSelects(itemSchs);
                    SchematicControl.Instance.SetPanelActive(true);
                    SchematicControl.Instance.SetPanel(newP1, newP2, itemSchs);
                    //设置为子物体
                    for (int i = 0; i < itemSchs.Count; i++)
                    {
                        itemSchs[i].transform.SetParent(SchematicControl.Instance.Panel.transform);
                        itemSchs[i].SetMoonFlashRun(); 
                    }
                },
                () =>
                {
                    SchematicControl.Instance.SetSelects(lastSchs);
                }
                );
        }
        else
        {
            if (lastSchs == null || lastSchs.Count == 0) return;
            List<BaseGraphicForSchematic> itemSchs = listBasesch;
            CommandManager.CommandMan.AddCammand(()=> {
                SchematicControl.Instance.SetSelects(itemSchs);
            },()=> {
                SchematicControl.Instance.SetSelects(lastSchs);
            });
            
        }
    }
    void Disselecting(GameObject obj)
    {
        //obj.GetComponent<ChangeShader>().DisSelect();
        //Debug.Log(obj.name);
    }
    void Selecting(GameObject obj)
    {
        //obj.GetComponent<BaseGraphicForSchematic>().SetColor(Color.red);
        //obj.GetComponent<ChangeShader>().Select();
    }

}
