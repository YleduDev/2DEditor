using DevelopEngine;
using Newtonsoft.Json;
using SpringGUI;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class SchematicControl:MonoSingleton<SchematicControl>
{
  
    //编辑模式
    public SchematicTransformType schematicType = SchematicTransformType.DrawLine;
    public LinkType linkType = LinkType.Broken;
    public bool bind = false;
    public bool CanDrawRect = false;
    //原理图目标Canvas
    private Canvas canvas;
    private Canvas Canvas
    {
        get
        {
            if (!canvas) canvas = GameObject.FindGameObjectWithTag("ImageCanvas")?.GetComponent<Canvas>();
          //  if(!canvas) ConsoleM.LogError("ImageCanvas 标签获取的Canvas有问题");
            return canvas;
        }
        set
        {
            canvas = value;
        }
    }

    //图元 tf 编辑脚本
    private ImageGizmo imageGizmo;
    public ImageGizmo ImageGizmo
    {
        get
        {
            if (!imageGizmo)
            {
                imageGizmo = Canvas.worldCamera.GetComponent<ImageGizmo>();
                if (!imageGizmo)
                {
                    imageGizmo = Canvas.worldCamera.gameObject.AddComponent<ImageGizmo>();
                    imageGizmo.Init();
                }
                if (!imageGizmo) ConsoleM.LogError("添加 ImageGizmo 脚本 不正确");
            }
            return imageGizmo;
        }
    }
    //颜色编辑器
    private ColorPicker colorPicker;
    public ColorPicker ColorPicker
    {
        get
        {
            if(!colorPicker)
            {
                colorPicker = FindObjectOfType<ColorPicker>();
            }
            return colorPicker;
        }
    }

    public string sceneName = "0";//当前划线场景名称
    //图元及线对应父物体的标签
    public readonly string icParentTag = "ICParent";
    public readonly string lineParentTag = "LineParent";
    public readonly string textParentTag = "TextParent";
    public readonly string bgTag = "LineBackground";
    public readonly string panelTag = "DrawPanel";
    private Transform icParent;
    public Transform IcParent
    {
        get
        {
            if (icParent == null)
                icParent = GameObject.FindGameObjectWithTag(icParentTag)?.transform;
            if (icParent == null) ConsoleM.LogError(icParentTag + "  标签的父物体不存在");
            return icParent;
        }
    }
    private Transform lineParent;
    public Transform LineParent
    {
        get
        {
            if (lineParent == null)
                lineParent = GameObject.FindGameObjectWithTag(lineParentTag)?.transform;
            if (lineParent == null) ConsoleM.LogError(lineParentTag + "  标签的父物体不存在");
            return lineParent;
        }
    }
    private Transform textParent;
    public Transform TextParent
    {
        get
        {
            if (textParent == null)
                textParent = GameObject.FindGameObjectWithTag(textParentTag)?.transform;
            if (textParent == null) ConsoleM.LogError(textParentTag + "  标签的父物体不存在");
            return textParent;
        }
    }
    private Image bg;
    public Image BG
    {
        get
        {
            if (bg == null)
                bg = GameObject.FindGameObjectWithTag(bgTag).GetComponent<Image>();
            if (bg == null) ConsoleM.LogError(bgTag + "  标签的父物体不存在");
            return bg;
        }
    }

    private Dictionary<string, SchematicJsonData> AllLineDict;   //总字典缓存
    string path = "/StreamingAssets/LineData.txt";//配置文件目录
    public string linePrefabPath= "Schematic/Line/5Line";//预制体目录
    private SchematicJsonData currentSave; //当前场景线 图 数据
    private Dictionary<string, GameObject> uiDargDict;//Pel Key->图元

    private Dictionary<string, GameObject> prefabDict;//预制体对象

    private List<GameObject> characters;//框选目标集合

    private List<BaseGraphicForSchematic> selects;//选择的目标集合
    private GameObject panel;//框选辅助框
    public GameObject Panel
    {
        get
        {
            if (!panel) panel = GameObject.FindGameObjectWithTag(panelTag);
            if (panel == null) ConsoleM.LogError(panelTag + "  标签的父物体不存在");
            return panel;
        }
    }
    #region 外部调用
    //保存
    public void Save()
    {
         RemovePanelChild();
        //获取活跃的图元
        List<PelData> pelList = new List<PelData>();
        for (int i = 0; i < IcParent.childCount; i++)
        {
            Transform childTF = IcParent.GetChild(i);
            if (childTF.gameObject.activeInHierarchy)
            {
                IconForSchematic iconSch = childTF.GetComponent<IconForSchematic>();
                if (!iconSch) ConsoleM.LogError("图元对象没有ICData对象");
                PelData pelData = new PelData(iconSch);
                pelList.Add(pelData);
            }
        }
        //获取活跃的线
        List<LineData> lineList = new List<LineData>();
        for (int i = 0; i < LineParent.childCount; i++)
        {
            Transform childTF = LineParent.GetChild(i);
            if (childTF.gameObject.activeInHierarchy)
            {
                LineForSchematic simleLine = childTF.GetComponent<LineForSchematic>();
                if (!simleLine) ConsoleM.LogError("获取的活跃的线物体 没找到simleLine 对象");
                //Debug.Log("line 路径" + simleLine.linePath);
                LineData lineData = new LineData(simleLine);
                lineList.Add(lineData);
            }
        }

        //获取活跃的文本
        List<TextData> textList = new List<TextData>();
        for (int i = 0; i < TextParent.childCount; i++)
        {
            Transform childTF = TextParent.GetChild(i);
            if (childTF.gameObject.activeInHierarchy)
            {
                TextForSchematic textSch = childTF.GetComponent<TextForSchematic>();
                //Debug.Log(textSch.mainColor);
                if (!textSch) ConsoleM.LogError("获取的活跃的文本物体 textSch 对象");
                //Debug.Log("line 路径" + simleLine.linePath);
                TextData textData = new TextData(textSch);
                textList.Add(textData);
            }
        }
        //获取背景信息
        BgForSchematic bgForSch = BG.GetComponent<BgForSchematic>();
        BGData bgData = new BGData(bgForSch);

        currentSave = new SchematicJsonData(sceneName, lineList, pelList,textList, bgData);
        //防止多次保存重复
        SchematicJsonData schJsonData;
        if (currentSave != null&& AllLineDict.TryGetValue(sceneName, out schJsonData) && currentSave.Equals(schJsonData)) return;

        AllLineDict[sceneName] = currentSave;
        //生成SceneLineData 对象
        SceneLineData sceneLineData = new SceneLineData(AllLineDict);
        string json = JsonConvert.SerializeObject(sceneLineData);

        StreamWriter sw = new StreamWriter(Application.dataPath + path);
        if (sw == null) { sw.Close(); };
        sw.Write(json);
        sw.Close();

    }

    //选择线 删除
    public void MySelectDelele()
    {
        if (selects != null && selects.Count > 0)
        { 
            PlaneRect plRect = GetPanelRect(selects);
            List<BaseGraphicForSchematic> listBS = new List<BaseGraphicForSchematic>();

            CommandManager.CommandMan.AddCammand(
                ()=> {
                    RemovePanelChild();
                    foreach (var item in selects) 
                    {
                        item.MyDestroy();
                    }
                    listBS = new List<BaseGraphicForSchematic>();
                    selects.ForEach(i => listBS.Add(i));
                    selects.Clear();
                },
                ()=> {
                    foreach (var item in listBS) 
                    {
                        if (!item.gameObject.activeSelf) item.gameObject.SetActive(true);
                        Add(item.gameObject);
                    }
                    JsutSetSelects(listBS);
                    if (plRect != null)
                    {
                        SetPanelActive(true);
                        SetPanel(plRect.start, plRect.end);
                        foreach (var item in selects)
                        {
                            item.transform.SetParent(Panel.transform);
                            item.SetMoonFlashRun();
                        }
                    }
                });     
        }
    }

    /// 读取
    public void LoadLineScene(string sceneName)
    {
        //获取总字典
        if (AllLineDict == null) GetAllDict();
        if (AllLineDict == null)
        {
            ConsoleM.Log("没有缓存，读取场景为空白");
            AllLineDict = new Dictionary<string, SchematicJsonData>();
            return;
        }
        //清空当前图元 和线 文本
        DeleteChild(LineParent);
        DeleteChild(IcParent);
        DeleteChild(TextParent);
        //改变当前场景名称
        this.sceneName = sceneName;
        SchematicJsonData lineJsonData;
        if (AllLineDict != null && AllLineDict.TryGetValue(sceneName, out lineJsonData))
        {
            //判断
            if (lineJsonData == null || lineJsonData.listLineData == null 
                 /*lineJsonData.listPelData == null || lineJsonData.listPelData.Count <= 0*/)
            {
                ConsoleM.LogError("读取的json串生成的对象有误"); return;
            }
            //生成 图元 
            List<PelData> listPelData = lineJsonData.listPelData;
            uiDargDict = new Dictionary<string, GameObject>();

            GameObject pelGO;
            for (int i = 0; i < listPelData.Count; i++)
            {
                GameObject prefab = GetPrefab(listPelData[i].prefabPath);
                if (prefab == null)
                {
                    ConsoleM.LogError("目录为" + listPelData[i].prefabPath + "  的图元加载失败");
                }
                pelGO = Create(listPelData[i].key, prefab, prefab.transform.position,Quaternion.identity);
                //调整位置
                SetPel(pelGO, listPelData[i]);
                //uiDargDict.Add(listPelData[i].id, pelGO);
            }
            //生成线
            List<LineData> listLineData = lineJsonData.listLineData;
            for (int i = 0; i < listLineData.Count; i++)
            {
                GameObject line=   GetPrefab(listLineData[i].linePrefabPath);
                if (!line) ConsoleM.LogError("线预制体的目录有问题：" + listLineData[i].linePrefabPath);
                GameObject lineGO = Create(line.name, line, line.transform.position, Quaternion.identity);              
                SetLine(lineGO, listLineData[i]);
            }
            //生成 文本
            List<TextData> listTextData = lineJsonData.textData;
            for (int i = 0; i < listTextData.Count; i++)
            {
                GameObject text = GetPrefab(listTextData[i].prefabPath);
                if (!text) ConsoleM.LogError("text预制体的目录有问题：" + listTextData[i].prefabPath);
                GameObject textGO = Create(text.name, text, text.transform.position, Quaternion.identity);
                SetText(textGO, listTextData[i]);
            }
            //背景
            BGData bgData = lineJsonData.bgData;
            BgForSchematic bgForSch = BG.GetComponent<BgForSchematic>();
            if (!bgForSch) ConsoleM.LogError("没找到BgForSchematic组件");
            if (!string.IsNullOrEmpty(bgData.key))
            {
                Sprite sprite= SchematicUIManager.Instance.getPicture.GetSpriteByBg(bgData.key);
                BG.sprite = sprite;
                bgForSch.key = bgData.key;
            }
            bgForSch.mainColor = new Color(bgData.color.r, bgData.color.g, bgData.color.b, bgData.color.a);
            BG.color = bgForSch.mainColor;
        }      
    }
    
    //设置聚焦物体
    public void SetTargetOb(GameObject GO)
    {
        ImageGizmo.SetTarget(GO);
    }
    //改变状态
    public void ChangeState()
    {
        schematicType = schematicType==SchematicTransformType.Seting?SchematicTransformType.DrawLine : SchematicTransformType.Seting;
                   
    } 
    //设置编辑状态
    public void SetTransformType(RuntimeGizmos.TransformType type)
    {
        ImageGizmo.type = type;   
    }

    //对外提供的预制体
    public GameObject GetPrefab(string path)
    {
        if (prefabDict==null) prefabDict = new Dictionary<string, GameObject>();
        GameObject prefab;
        if (!prefabDict.TryGetValue(path, out prefab)
            || prefab)
        {
            prefab = Resources.Load<GameObject>(path);
            prefabDict[path] = prefab;
            return prefab;
        }
        return prefab;
    }
    //对外提供的获取线预制体
    public GameObject GetPrefabForLine()
    {
        if (prefabDict == null) prefabDict = new Dictionary<string, GameObject>();
        GameObject prefab;
        if (!prefabDict.TryGetValue(linePrefabPath, out prefab)
            || prefab)
        {
            prefab = Resources.Load<GameObject>(linePrefabPath);
            prefabDict[linePrefabPath] = prefab;
            return prefab;
        }
        return prefab;
    }
    //生成
    public GameObject Create(string key,GameObject prefab,Vector3 pos,Quaternion qua)
    {
        GameObject go = GameObjectPool.Instance.CreateObject(key, prefab, pos, qua);
        Add(go);
        return go;
    }
    public GameObject Create(GameObject prefab,Vector3 pos,Quaternion qua)
    {
        GameObject go = Instantiate(prefab, pos,qua);
        Add(go);
        return go;
    }
    //删除
    public void Delete(GameObject go)
    {
        if (go)
        {
            Remove(go);
            GameObjectPool.Instance.MyDestory(go);
        }
    }
    //框选总目标集合
    public List<GameObject> GetCharacters()
    {
        return characters;
    }

    #region 框选集合
    //用于正常赋值
    public void SetSelects( BaseGraphicForSchematic sch)
    {
        //如果panel下有目标移除
        RemovePanelChild();
        if (sch == null)
        {
            selects = null;
            return;
        }
        if (selects == null) selects = new List<BaseGraphicForSchematic>();
        selects.Clear();
        selects.Add(sch);

        //设置UI
        SetUIComponent();
    }
    public void SetSelectsForEmpty()
    {
        //如果panel下有目标移除
        RemovePanelChild();
        if (selects == null) selects = new List<BaseGraphicForSchematic>();
        selects.Clear();
        //设置UI
        SetUIComponent();
    }
    public void JsutSetSelects(List<BaseGraphicForSchematic> schList)
    {
        //清空Panel下所有物体
        RemovePanelChild();
        //赋值
        if (schList == null)
        {
            selects = null;
            return;
        }
        selects = schList;
        //设置UI
        SetUIComponent();
    }

    public void SetSelects(List<BaseGraphicForSchematic> schList)
    {
        JsutSetSelects(schList);
        // panel
        if (selects!=null&&selects.Count >=1)
        {
            if(dict.ContainsKey(schList))
            {
                PlaneRect rect = dict[schList];
                SetPanelActive(true);
                SetPanel(rect.start, rect.end);
                foreach (var item in selects)
                {
                    item.transform.SetParent(Panel.transform);
                    item.SetMoonFlashRun();
                }
            } 
        }
    }
    //用于复制粘贴
    //bo 表示是否右下偏移
    //panelBo 表示是否检测panle辅助框

    public void SetCopySelects(List<BaseGraphicForSchematic> schList)
    {
        JsutSetSelects(schList);
        //多选 偏移
        if (selects.Count > 1)
        {
            foreach (var item in selects)
            {
                item.transform.SetParent(Panel.transform, false);
                //图元单位右下偏移
                Vector3 screenPos = Canvas.worldCamera.WorldToScreenPoint(item.transform.position);
                screenPos = new Vector3(screenPos.x + 10, screenPos.y - 10, screenPos.z);
                item.transform.position = Canvas.worldCamera.ScreenToWorldPoint(screenPos);
            }
        }
        //单选 偏移
        else if (selects.Count == 1)
        {
            selects[0]?.SetParent(false);
            // 将目标图元单位位置右下偏移
            Vector3 screenPos = Canvas.worldCamera.WorldToScreenPoint(selects[0].transform.position);
            screenPos = new Vector3(screenPos.x + 10, screenPos.y - 10, screenPos.z);
            selects[0].transform.position = Canvas.worldCamera.ScreenToWorldPoint(screenPos);
        }
    }

    public bool SelectIsNullOrEmpty()
    {
        return !(selects != null && selects.Count > 0);
    }
    public List<BaseGraphicForSchematic> GetSelects()
    {
        return  selects;
    }
    public PlaneRect GetPanelRectForCopy()
    {
        if(panel.activeSelf)return planeRect;
        return null;
    }
    public List<BaseGraphicForSchematic> GetIconOrTextInSelects(BaseGraphicForSchematic[] baseSchList)
    {
        if (baseSchList == null || baseSchList.Length <= 0) return null;
        List<BaseGraphicForSchematic> list = new List<BaseGraphicForSchematic>();
        for (int i = 0; i < baseSchList.Length; i++)
        {
            if (baseSchList[i].schematicType != SchematicType.Line) list.Add(baseSchList[i]);
        }
        return list;
    }
    public List<BaseGraphicForSchematic> GetIconOrTextInSelects()
    {
        if (selects == null || selects.Count <= 0) return null;
        List<BaseGraphicForSchematic> list = new List<BaseGraphicForSchematic>();
        for (int i = 0; i < selects.Count; i++)
        {
            if (selects[i].schematicType != SchematicType.Line) list.Add(selects[i]);
        }
        return list;
    }
    //移出辅助框中的元素 
    public void RemovePanelChild()
    {
        if (!SelectIsNullOrEmpty())
        {
            foreach (var item in selects)
            {
                item.SetParent(true);
                item.SetMoonFlashClose();
            }
            Panel.SetActive(false);
        }
    }

    //根据目标设置目标的ui按钮
    public void SetUIComponent()
    {
        if (schematicType == SchematicTransformType.Seting)SchematicUIManager.Instance.SetEidtButtonActiveEvent(true);
        else SchematicUIManager.Instance.SetEidtButtonActiveEvent(false);

        if (selects == null || selects.Count == 0)
        {
            //设置共性按钮
            SchematicUIManager.Instance.SetGeneralButtonActive(false, null);
            SchematicUIManager.Instance.SetSiblingButtonActive(false);
            SchematicUIManager.Instance.SetAlignButtonActive(false);
            return;
        }
        //设置共性按钮(颜色、赋值等)
        SchematicUIManager.Instance.SetGeneralButtonActive(true, selects.ToArray());
        
        if (selects.Count == 1)
        {
            //层级
            SchematicUIManager.Instance.SetSiblingButtonActive(true);//对齐
            SchematicUIManager.Instance.SetAlignButtonActive(false);
            
        }
        else
        {
            //层级
            SchematicUIManager.Instance.SetSiblingButtonActive(false);//对齐

            List<BaseGraphicForSchematic> iconOrTextList = GetIconOrTextInSelects();
            if (iconOrTextList != null && iconOrTextList.Count > 1)
            {
                SchematicUIManager.Instance.SetAlignButtonActive(true);
            }
            else SchematicUIManager.Instance.SetAlignButtonActive(false);
        }
    }


    public void Enlarge(Camera camera, float mouseX, float mouseY, float deltaSize)
    {
        // 获取鼠标放置位置
        var mousePos = camera.ScreenToWorldPoint(new Vector3(mouseX, mouseY, 0));

        float size = camera.orthographicSize;

        // 放大比例
        float ratio = deltaSize / size;

        camera.transform.position -= ratio * (camera.transform.position - mousePos);
        camera.orthographicSize = size - deltaSize;
    }


    #endregion
    #endregion

    #region 内部调用
    //设置线
    private void SetLine(GameObject line, LineData lineData)
    {
        //赋值
        LineForSchematic simpleLine = line.GetComponent<LineForSchematic>();
        if(!simpleLine) ConsoleM.LogError("线预制体木有挂载SimpleLine脚本：");
        simpleLine.minPlx = lineData.LineHigh;
        simpleLine.SetParent(false);
        Vector3 pointA = new Vector3(lineData.a.x, lineData.a.y, lineData.a.z);
        Vector3 pointB = new Vector3(lineData.b.x, lineData.b.y, lineData.b.z);
        Vector3 breakPoint = new Vector3(lineData.breakPoint.x, lineData.breakPoint.y, lineData.breakPoint.z);
        Vector3 breakLocalPoint= new Vector3(lineData.breakLocalPoint.x, lineData.breakLocalPoint.y, lineData.breakLocalPoint.z);
        Vector3 screenA= new Vector3(lineData.screenA.x, lineData.screenA.y, lineData.screenA.z);
        Vector3 screenB= new Vector3(lineData.screenB.x, lineData.screenB.y, lineData.screenB.z);
        //设置线的算法
        simpleLine.linkType = lineData.linkType;
        simpleLine.LinklineData(pointA, pointB, breakLocalPoint,breakPoint, screenA, screenB);
        simpleLine.screenA = screenA;
        simpleLine.screenB = screenB;
        simpleLine.bindData = lineData.bindData;
        simpleLine.prefabPath = lineData.linePrefabPath;
        Color color = new Color(lineData.color.r, lineData.color.g, lineData.color.b, lineData.color.a);
        simpleLine.mainColor = color;
        simpleLine.SetColor(color);
        simpleLine.schematicType = SchematicType.Line;
        //线的颜色
        //绑定
        GameObject a;
        if(simpleLine.bindData.aBandNane!=null&&uiDargDict.TryGetValue(simpleLine.bindData.aBandNane,out a))
        {
            SchematicUIEvent uiDrag = a.GetComponent<SchematicUIEvent>();
            simpleLine.Bind(uiDrag, BindTpye.A);
        }
        GameObject b;
        if (simpleLine.bindData.bBandNane != null && uiDargDict.TryGetValue(simpleLine.bindData.bBandNane, out b))
        {
            SchematicUIEvent uiDrag = b.GetComponent<SchematicUIEvent>();
            simpleLine.Bind(uiDrag, BindTpye.B);
        }

    }
    //设置图
    private void SetPel(GameObject go, PelData pelData)
    {
        // prefab.name = pelData.id;
        //if (input) input .text= pelData.textValue;
        IconForSchematic icoSch = go.GetComponent<IconForSchematic>();
        icoSch.prefabPath = pelData.prefabPath;
        icoSch.spriteKey = pelData.key;
        icoSch.mainColor = new Color(pelData.color.r, pelData.color.g, pelData.color.b, pelData.color.a);
        icoSch.schematicType = SchematicType.Icon;

        icoSch.SetParent(false);
        go.transform.localPosition = new Vector3(pelData.icLocaPos.x, pelData.icLocaPos.y,pelData.icLocaPos.z);
        go.transform.localEulerAngles = new Vector3(pelData.icLocalEuler.x, pelData.icLocalEuler.y, pelData.icLocalEuler.z);
        go.transform.localScale = new Vector3(pelData.icLocalScale.x, pelData.icLocalScale.y, pelData.icLocalScale.z);

        //图片 大小  sprite 颜色
        Image image = go.GetComponent<Image>();
        image.sprite = SchematicUIManager.Instance.getPicture.GetSprite(pelData.key);
        image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pelData.width);
        image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, pelData.height);
        image.color = icoSch.mainColor;
    }

    private void SetText(GameObject go, TextData textData)
    {
        TextForSchematic textSch = go.GetComponent<TextForSchematic>();
        textSch.prefabPath = textData.prefabPath;
        textSch.text = textData.text;
        Color color = new Color(textData.textColor.r, textData.textColor.g, textData.textColor.b, textData.textColor.a);
        textSch.mainColor = color;
        textSch.schematicType = SchematicType.Text;

        textSch.SetParent(false);
        go.transform.localPosition = new Vector3(textData.icLocaPos.x, textData.icLocaPos.y, textData.icLocaPos.z);
        go.transform.localEulerAngles = new Vector3(textData.icLocalEuler.x, textData.icLocalEuler.y, textData.icLocalEuler.z);
        go.transform.localScale = new Vector3(textData.icLocalScale.x, textData.icLocalScale.y, textData.icLocalScale.z);

        //大小 字  颜色
        RectTransform rt= go.transform as RectTransform;
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, textData.width);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, textData.height);
        if (!textSch.textMeshProUGUI) textSch.Init(); 
        textSch.textMeshProUGUI.color = color;
        textSch.inputField.text = textData.text;
    }

    //获取总字典
    private void GetAllDict()
    {
        if (File.Exists(Application.dataPath + path))
        {
            string json = File.ReadAllText(Application.dataPath + path);
            if (!String.IsNullOrEmpty(json))
            {
                SceneLineData sceneLineData = JsonConvert.DeserializeObject<SceneLineData>(json);
                if (sceneLineData != null && sceneLineData.schematData != null && sceneLineData.schematData.Count > 0)
                {
                    AllLineDict = new Dictionary<string, SchematicJsonData>();
                    //将对象转化成allLineDict
                    foreach (var item in sceneLineData.schematData)
                    {
                        if (AllLineDict.ContainsKey(item.sceneName)) ConsoleM.LogError("文本或者对象的key有重复");
                        AllLineDict.Add(item.sceneName, item);
                    }
                }
                else ConsoleM.LogError("序列化出错“ 序列化的文本是”：" + json);
            }
            else
            {
                ConsoleM.LogError("json字符串为空");
            }
        }
        //没有配置文件
        else
        {
            //AllLineDict = new Dictionary<string, LineJsonData>();
        }
    }

   //删除目标物体下所有子物体
    private void DeleteChild(Transform tf)
   {
        if (tf.childCount <= 0) return;
        for (int i = 0; i < tf.childCount; i++)
        {      
         tf.GetChild(i).GetComponent<BaseGraphicForSchematic>().MyDestroy();         
        }
   }
    //移出框选集合
    private void Remove(GameObject go)
    {
        if (characters == null) return;
        characters.Remove(go);
    }

    public void Add(GameObject go)
    {
        if (characters == null) characters = new List<GameObject>();
        characters.Add(go);
    }
   
    #endregion

    #region 静态工具
    //工具  获取字符串 用 ch 切割后 索引最后一个字符串
    public static string GetEndStr(string str,char ch)
    {
        if (string.IsNullOrEmpty(str)) return null;
        string[] arrStr = str.Split(ch);
        if (arrStr != null && arrStr.Length > 0) return arrStr[arrStr.Length - 1];
        return null;
    }

    public static string GetBeginStr(string str, char ch)
    {
        if (string.IsNullOrEmpty(str)) return null;
        string[] arrStr = str.Split(ch);
        if (arrStr != null && arrStr.Length > 0) return arrStr[0];
        return null;
    }
    #endregion

    #region 框选辅助框 
    public void SetPanelActive(bool bo)
    {
        Panel.SetActive(bo);
    }
    private Dictionary<List<BaseGraphicForSchematic>, PlaneRect> dict = new Dictionary<List<BaseGraphicForSchematic>, PlaneRect>();
    private RectTransform rt;
    private PlaneRect planeRect;
    public void SetPanel(Vector2 start, Vector2 end,List<BaseGraphicForSchematic> listBaseSch)
    {
        if (!rt) rt= Panel.transform as RectTransform;
        planeRect = new PlaneRect(start, end);
        dict[listBaseSch]=planeRect;
        Vector3 v3;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, start, Canvas.worldCamera, out v3);
        rt.position = v3;
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, end.x - start.x);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, end.y - start.y);
    }
    public void SetPanel(Vector2 start, Vector2 end)
    {
        if (!rt) rt = Panel.transform as RectTransform;
        Vector3 v3;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, start, Canvas.worldCamera, out v3);
        rt.position = v3;
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, end.x - start.x);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, end.y - start.y);
    }
    public PlaneRect GetPanelRect(List<BaseGraphicForSchematic> listBaseSch)
    {
        if (dict != null && !dict.ContainsKey(listBaseSch)) return null;
        return dict[listBaseSch];
    }
    #endregion
}
public class PlaneRect
{
    public Vector2 start;
    public Vector2 end;
    public PlaneRect(Vector2 start, Vector2 end)
    {
        this.start = start;
        this.end=end;
    }
}
