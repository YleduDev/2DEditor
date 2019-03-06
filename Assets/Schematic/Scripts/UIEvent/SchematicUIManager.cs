using DevelopEngine;
using SpringGUI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//对齐方式
public enum AlignTpye
{
    //上下左右
    Top,Down,Left,Right
}
/// <summary>
/// 原理模型UI控制(粗糙版)
/// </summary>
public class SchematicUIManager : MonoSingleton<SchematicUIManager> {
    #region EidtUi
    //颜色编辑按钮
    public Camera EventCamera;
    public GameObject colorButton, upOneLevelButton,topButton,
        downOneLevelButton,lowerButton,copyButton,cutButton;
    public GameObject colorPickerComponent;
    public GameObject moveButton, rotateButton, ScaleButton;
    public GameObject topAlignButton, downAlignButton, leftAlignButton, rightAlignButton;

    private ColorPicker picker; 
    //设置颜色 复制 按钮显隐
    public void SetGeneralButtonActive(bool bo,params BaseGraphicForSchematic[] schematic)
    {
        colorButton.gameObject.SetActive(bo);
        copyButton.gameObject.SetActive(bo);

        AddListenersForGraphic(schematic);
        if (!bo) colorPickerComponent.SetActive(bo); 
    }
    //设置层级
    public void SetSiblingButtonActive(bool bo)
    {
        upOneLevelButton.gameObject.SetActive(bo);
        topButton.gameObject.SetActive(bo);
        downOneLevelButton.gameObject.SetActive(bo);
        lowerButton.gameObject.SetActive(bo);
    }
    //设置排序
    public void SetAlignButtonActive(bool bo)
    {
        topAlignButton.gameObject.SetActive(bo);
        downAlignButton.gameObject.SetActive(bo);
        leftAlignButton.gameObject.SetActive(bo);
        rightAlignButton.gameObject.SetActive(bo);
    }
    
    //TODo
    private void AddListenersForGraphic(params BaseGraphicForSchematic[] schematic)
    {
        if (!picker) picker = colorPickerComponent.GetComponent<ColorPicker>();      
            picker.onPicker.RemoveAllListeners();
            if (schematic == null||schematic.Length<=0) return;

             List<Color> colors = new List<Color>(); ;
             picker.onPicker.AddListener(                   
                    color => {

                        if (Input.GetMouseButtonDown(0))
                        {
                            colors.Clear();
                            for (int i = 0; i < schematic.Length; i++)
                            {
                                colors.Add(schematic[i].mainColor);
                            }
                        }

                        foreach (var item in schematic)
                        {
                        item.SetColor(color);
                        item.mainColor = color;
                        }
                        //鼠标抬起
                        if (Input.GetMouseButtonUp(0))
                        {
                            List<Color> lastColors = new List<Color>();
                            colors.ForEach((co) =>
                            {
                                lastColors.Add(co);
                            });

                            CommandManager.CommandMan.AddCammand(()=> {
                                foreach (var item in schematic)
                                { 
                                    item.SetColor(color);
                                    item.mainColor = color;
                                }
                            },()=> {
                                for (int i = 0; i < schematic.Length; i++)
                                {
                                    schematic[i].SetColor(lastColors[i]);
                                    schematic[i].mainColor = lastColors[i];
                                }
                            });
                        }

                    });                 
    }  
    //注册 即 颜色按钮单击
    public void ColorButtonClickEvent()
    {        
        if (colorPickerComponent.activeInHierarchy) return;
        CommandManager.CommandMan.AddCammand(() => {
            colorPickerComponent.SetActive(true);
        }, () => {
            colorPickerComponent.SetActive(false);
        });
        
    }
    //ColorPickerClose
    public void ColorPickerComponentCloseClickEvent()
    {
        CommandManager.CommandMan.AddCammand(() => {
            colorPickerComponent.SetActive(false);
        }, () => {
            colorPickerComponent.SetActive(true);
        });
    }
    //移动
    public void SetMoveTransformTpye() 
    {
        RuntimeGizmos.TransformType type = SchematicControl.Instance.ImageGizmo.type;
        CommandManager.CommandMan.AddCammand(() => {
            SchematicControl.Instance.SetTransformType(RuntimeGizmos.TransformType.Move);
        }, () => {
            SchematicControl.Instance.SetTransformType(type);
        });
    }
    //旋转
    public void SetRotateTransformTpye()
    {
        RuntimeGizmos.TransformType type = SchematicControl.Instance.ImageGizmo.type;
        CommandManager.CommandMan.AddCammand(() => {
            SchematicControl.Instance.SetTransformType(RuntimeGizmos.TransformType.Rotate);
        }, () => {
            SchematicControl.Instance.SetTransformType(type);
        });
   }
    //大小
    public void SetScaleTransformTpye()
    {
        RuntimeGizmos.TransformType type = SchematicControl.Instance.ImageGizmo.type;
        CommandManager.CommandMan.AddCammand(()=> {
            SchematicControl.Instance.SetTransformType(RuntimeGizmos.TransformType.Scale);
        }, ()=> {
            SchematicControl.Instance.SetTransformType(type);
        });
    }
    //编辑按钮的显隐
    public void SetEidtButtonActiveEvent(bool bo)
    {
        moveButton.SetActive(bo);
        rotateButton.SetActive(bo);
        ScaleButton.SetActive(bo);
    }
    #region  层级
    //上一级
    public void UpOneButtonEvent()
    {
        if (!SchematicControl.Instance.SelectIsNullOrEmpty())
        {
            List<BaseGraphicForSchematic> listBs = SchematicControl.Instance.GetSelects();
            CommandManager.CommandMan.AddCammand(()=> {
                foreach (var item in listBs)
                {
                    int index = item.Rect.GetSiblingIndex();
                    item.Rect.SetSiblingIndex(index + 1);
                }
            },()=> {
                foreach (var item in listBs)
                {
                    int index = item.Rect.GetSiblingIndex();
                    item.Rect.SetSiblingIndex(index - 1);
                }
            });
            
        }     
    }
    //顶级
    public void TopButton()
    {
        if (!SchematicControl.Instance.SelectIsNullOrEmpty())
        {
            List<BaseGraphicForSchematic> listBs = SchematicControl.Instance.GetSelects();
            int index = listBs[0].Rect.GetSiblingIndex();

            CommandManager.CommandMan.AddCammand(()=> {
                listBs[0].Rect.SetAsLastSibling();
            }, () => {
                listBs[0].Rect.SetSiblingIndex(index);
            });
            
        }
    }
    //下一级
    public void DownOneButtonEvent()
    {
        if (!SchematicControl.Instance.SelectIsNullOrEmpty())
        {
            List<BaseGraphicForSchematic> listBs = SchematicControl.Instance.GetSelects();
            int index = listBs[0].Rect.GetSiblingIndex();

            if (index - 1 >= 0)
            {
                CommandManager.CommandMan.AddCammand(() => {
                    listBs[0].Rect.SetSiblingIndex(index - 1);
                }, () => {
                    listBs[0].Rect.SetSiblingIndex(index);
                });
                
            }
        }
    }
    //底级
    public void LowerButton()
    {
        if (!SchematicControl.Instance.SelectIsNullOrEmpty())
        {
            List<BaseGraphicForSchematic> listBs = SchematicControl.Instance.GetSelects();
            int index = listBs[0].Rect.GetSiblingIndex();

            CommandManager.CommandMan.AddCammand(() => {
                listBs[0].Rect.SetAsFirstSibling();
            }, () => {
                listBs[0].Rect.SetSiblingIndex(index);
            });
                  
        }   
    }
    

    #endregion
    #region 对齐
    //上对齐
    public void TopAlignButton()
    {
        //线段要求重置
        //找出最高处顶点
        List<BaseGraphicForSchematic> listSch = SchematicControl.Instance.GetIconOrTextInSelects();
        float top= GetAllSecenPoint(listSch, AlignTpye.Top);
        List<Vector3> posList = new List<Vector3>();
        for (int i = 0; i < listSch.Count; i++)
        {
            posList.Add(listSch[i].transform.position);
        }

        CommandManager.CommandMan.AddCammand(()=> {
            SetAlign(listSch, AlignTpye.Top, top);
        }, () => {
            for (int i = 0; i < listSch.Count; i++)
            {
                listSch[i].transform.position = posList[i];
            }

        });
    }
    public void DownAlignButton()
    {
        //找出最高处顶点
        List<BaseGraphicForSchematic> listSch = SchematicControl.Instance.GetIconOrTextInSelects();
        float top = GetAllSecenPoint(listSch, AlignTpye.Down);

        List<Vector3> posList = new List<Vector3>();
        for (int i = 0; i < listSch.Count; i++)
        {
            posList.Add(listSch[i].transform.position);
        }

        CommandManager.CommandMan.AddCammand(() => {
            SetAlign(listSch, AlignTpye.Down, top);
        }, () => {
            for (int i = 0; i < listSch.Count; i++)
            {
                listSch[i].transform.position = posList[i];
            }

        });

       
    }
    public void LeftAlignButton()
    {
        //找出最高处顶点
        List<BaseGraphicForSchematic> listSch = SchematicControl.Instance.GetIconOrTextInSelects();
        float top = GetAllSecenPoint(listSch, AlignTpye.Left);

        List<Vector3> posList = new List<Vector3>();
        for (int i = 0; i < listSch.Count; i++)
        {
            posList.Add(listSch[i].transform.position);
        }

        CommandManager.CommandMan.AddCammand(() => {
            SetAlign(listSch, AlignTpye.Left, top);
        }, () => {
            for (int i = 0; i < listSch.Count; i++)
            {
                listSch[i].transform.position = posList[i];
            }

        });
    }
    public void RightAlignButton()
    {
       //找出最高处顶点
        List<BaseGraphicForSchematic> listSch = SchematicControl.Instance.GetIconOrTextInSelects();
        float top = GetAllSecenPoint(listSch, AlignTpye.Right);

        List<Vector3> posList = new List<Vector3>();
        for (int i = 0; i < listSch.Count; i++)
        {
            posList.Add(listSch[i].transform.position);
        }

        CommandManager.CommandMan.AddCammand(() => {
            SetAlign(listSch, AlignTpye.Right, top);
        }, () => {
            for (int i = 0; i < listSch.Count; i++)
            {
                listSch[i].transform.position = posList[i];
            }

        });
    }

    private float GetAllSecenPoint(List<BaseGraphicForSchematic> listSch, AlignTpye alignType)
    {
        List<float> listF = new List<float>();
        foreach (var item in listSch)
        {
            Vector3 secenPoint = EventCamera.WorldToScreenPoint(item.transform.position);
            switch (alignType)
            {
                case AlignTpye.Top:
                    float top = secenPoint.y + ( (1f-item.Rect.pivot.y) * item.Rect.rect.height);
                    listF.Add(top);break;
                case AlignTpye.Down:
                    float down = secenPoint.y - (item.Rect.pivot.y * item.Rect.rect.height);
                    listF.Add(down);break;
                case AlignTpye.Left:
                    float left = secenPoint.x - (item.Rect.pivot.x * item.Rect.rect.width);
                    listF.Add(left);break;
                case AlignTpye.Right:
                    float right = secenPoint.x + ((1f-item.Rect.pivot.x) * item.Rect.rect.width);
                    listF.Add(right);break;
            }
        }
        switch (alignType)
        {
            case AlignTpye.Top:
              return  CollectionHelper.Find(listF.ToArray(), (a)=>
                {
                    float max = a[0];
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (max < a[i]) max = a[i];
                    }
                    return max;
                });
            case AlignTpye.Down:
                return CollectionHelper.Find(listF.ToArray(), (a) =>
                {
                    float min = a[0];
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (min > a[i]) min = a[i];
                    }
                    return min;
                });
            case AlignTpye.Left:
                return CollectionHelper.Find(listF.ToArray(), (a) =>
                {
                    float min = a[0];
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (min > a[i]) min = a[i];
                    }
                    return min;
                });
            case AlignTpye.Right:
                return CollectionHelper.Find(listF.ToArray(), (a) =>
                {
                    float max = a[0];
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (max < a[i]) max = a[i];
                    }
                    return max;
                });
            default:
                return 0;
        }
    }
    private void SetAlign(List<BaseGraphicForSchematic> listSch, AlignTpye alignType,float Point)
    {
        foreach (var item in listSch)
        {
            Vector3 secenPoint = EventCamera.WorldToScreenPoint(item.transform.position);
            Vector3 newScenePoint;
            switch (alignType)
            {
                case AlignTpye.Top:
                    float top = Point + ((item.Rect.pivot.y-1) * item.Rect.rect.height);
                    newScenePoint = new Vector3(secenPoint.x, top, secenPoint.z);
                    item.transform.position = EventCamera.ScreenToWorldPoint(newScenePoint);
                    break;
                case AlignTpye.Down:
                    float down = Point + ((item.Rect.pivot.y) * item.Rect.rect.height);
                     newScenePoint = new Vector3(secenPoint.x, down, secenPoint.z);
                    item.transform.position = EventCamera.ScreenToWorldPoint(newScenePoint);
                    break;
                case AlignTpye.Left:
                    float left = Point + ((item.Rect.pivot.x) * item.Rect.rect.width);
                    newScenePoint = new Vector3(left, secenPoint.y, secenPoint.z);
                    item.transform.position = EventCamera.ScreenToWorldPoint(newScenePoint);
                    break;
                case AlignTpye.Right:
                    float right = Point + ((item.Rect.pivot.x-1) * item.Rect.rect.width);
                    newScenePoint = new Vector3(right, secenPoint.y, secenPoint.z);
                    item.transform.position = EventCamera.ScreenToWorldPoint(newScenePoint);
                    break;
            }
        }
        
        
    }


    #endregion
    #region 复制 剪切 黏贴
    public GameObject copyObject;
    private List<BaseGraphicForSchematic> copyGo;
    PlaneRect planeRect;
    //复制
    public void Copy()
    {
        //判断
        if (SchematicControl.Instance.SelectIsNullOrEmpty()) return;
        //清空复制缓存
        CopyObjectClear();

        CreateSchematic(SchematicControl.Instance.GetSelects());
        planeRect = SchematicControl.Instance.GetPanelRectForCopy();
    }
    //剪切 ?
    public void Cut()
    {

    }
    //粘贴
    public void Stickup()
    {
        if (copyGo == null || copyGo.Count <= 0) return;
        List<BaseGraphicForSchematic> baseSchList = new List<BaseGraphicForSchematic>();
        List<BaseGraphicForSchematic> lastSchList = SchematicControl.Instance.GetSelects();
        CommandManager.CommandMan.AddCammand(()=> {
            for (int i = 0; i < copyGo.Count; i++)
            {
             //生成
             GameObject go = SchematicControl.Instance.Create(copyGo[i].gameObject, copyGo[i].transform.position, copyGo[i].transform.rotation);
             baseSchList.Add(go.GetComponentInChildren<BaseGraphicForSchematic>());
            }
            //设置为selects 
            SchematicControl.Instance.SetCopySelects(baseSchList);
            //辅助框 panel 标记。
            if (planeRect != null)
            {
                SchematicControl.Instance.SetPanelActive(true);
                SchematicControl.Instance.SetPanel(planeRect.start, planeRect.end, baseSchList);
            }

        },()=> {
            foreach (var item in baseSchList)
            {
                item.MyDestroy();
            }
            baseSchList.Clear();
            SchematicControl.Instance.SetSelects(lastSchList);
        });

        
    }
    //辅助栏 清空
    private void CopyObjectClear()
    {
        if (copyGo == null) copyGo = new List<BaseGraphicForSchematic>();
        copyGo.Clear();
        if (copyObject.transform.childCount <= 0) return;
        while (copyObject.transform.childCount > 0)
        { 
            DestroyImmediate(copyObject.transform.GetChild(0).gameObject);
        }
    }
    //在复制栏中生成  复制的原理图
    private void CreateSchematic(List<BaseGraphicForSchematic> listBaseSchematic)
    {
        for (int i = 0; i < listBaseSchematic.Count; i++)
        {
           GameObject go = Instantiate(listBaseSchematic[i].gameObject, copyObject.transform);
            //go.transform.SetParent();
            copyGo.Add(go.GetComponent<BaseGraphicForSchematic>());
        }
    }

    #endregion
    #endregion

    #region BG
    public Image bg;
    private BgForSchematic bgGS;
    private BgForSchematic BgGS {get
        {
            if(!bgGS) bgGS = bg.GetComponent<BgForSchematic>();
            return bgGS;
        }
    }    public GameObject bgPanel;
    private string colorHz = "#E0DCC7";
    public GameObject backgrandEditButton;
    //背景编辑界面点击事件
    public void BackgrandEiditOnCilck()
    {
        CommandManager.CommandMan.AddCammand(()=>{

            bgPanel.SetActive(true);
            backgrandEditButton.SetActive(false);

        },()=>{
            bgPanel.SetActive(false);
            backgrandEditButton.SetActive(true);
        });
      
    }
    //编辑
    public void BackBGEvent()
    {

        bool bo = colorPickerComponent.activeInHierarchy;
        CommandManager.CommandMan.AddCammand(() => {

            if (bo) colorPickerComponent.SetActive(false);
            //颜色关闭
            bgPanel.SetActive(false);
            backgrandEditButton.SetActive(true);

        }, () => {
            bgPanel.SetActive(true);
            backgrandEditButton.SetActive(false);
            if (bo) colorPickerComponent.SetActive(true);
        });

       
    }

    //重置背景
    public void ResetBG()
    {
        Color lastColor = bg.color;
        Sprite lastSprite = bg.sprite;
        string lastKey = BgGS.key;
        CommandManager.CommandMan.AddCammand(() => {
            Color color;
            ColorUtility.TryParseHtmlString(colorHz, out color);
            bg.color = color;
            bg.sprite = null;
            //赋值
            BgGS.key = "";
            BgGS.mainColor = color;

        }, () => {
            bg.color = lastColor;
            bg.sprite = lastSprite;
            //赋值
            BgGS.key = lastKey;
            BgGS.mainColor = lastColor;
        });

        
    }

    public void OnLoadBGButtenCilck()
    {
        string key = "";
        CommandManager.CommandMan.AddCammand(() => {
            if (string.IsNullOrEmpty(key))
            {
                key = getPicture.LoadBG();
            }
            //if (!sprite) return;
            bg.sprite = getPicture.GetSpriteByBg(key);
            //赋值
            BgGS.key = key;
            BgGS.mainColor = bg.color;

        }, () => {
            //设为默认

            Color color;
            ColorUtility.TryParseHtmlString(colorHz, out color);
            bg.color = color;
            bg.sprite = null;
            //赋值
            BgGS.key = "";
            BgGS.mainColor = color;

        });
       
    }
    //设置颜色
    public void BGColor()
    {
        CommandManager.CommandMan.AddCammand(() => {
            if (!colorPickerComponent.activeInHierarchy) colorPickerComponent.SetActive(true);
            AddListenersForGraphic(BgGS);
        }, () => {
            colorPickerComponent.SetActive(false);
            AddListenersForGraphic(null);
        });
    }

    #endregion

    #region PelSeting
    public GetPicture getPicture;
    //更新左边图标
    private void UpdateIcon()
    {
        getPicture.ReadPicture(); 
    }

    //删除按钮
    public void DeleteTexture()
    {
        getPicture.DeletePictures(DragButtonEvent.fileNane);
    }
    //自定义获取图标
    public void PelLoadButtonClick()
    {
        getPicture.OnLoadButtonClick();
    }

    #endregion

    public GameObject lineButton, editButton;

    public void Line()
    {
        lineButton.SetActive(false);
        editButton.SetActive(true);
        SchematicControl.Instance.ChangeState();
        //跟新UI
        SchematicControl.Instance.SetUIComponent();
    }
    //改变状态
    public void Edit()
    {
        lineButton.SetActive(true);
        editButton.SetActive(false);
        SchematicControl.Instance.ChangeState();
        //跟新UI
        SchematicControl.Instance.SetUIComponent();
    }


    //保存按钮注册事件
    public void SaveClick()
    {
        SchematicControl.Instance.Save();
    }

}
