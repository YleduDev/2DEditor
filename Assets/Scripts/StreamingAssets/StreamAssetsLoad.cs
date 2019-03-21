using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class StreamAssetsLoad : MonoBehaviour
{
    /// <summary>
    /// 字典控制Image的图片显示
    /// </summary>
    private Dictionary<string, Image> dic = new Dictionary<string, Image>();

    /// <summary>
    /// 字典控制Content的Active显示
    /// </summary>
    private Dictionary<string, GameObject> objDic = new Dictionary<string, GameObject>();

    /// <summary>
    /// 控制按钮的旋转
    /// </summary>
    Vector3 hide = new Vector3(0, 0, 90);
    Vector3 show = Vector3.zero;

    IEnumerator Start()
    {     
      yield return StartCoroutine(LoadEditor());     
    }


    /// <summary>
    /// 生成编辑
    /// </summary>
    void LoadPictures()
    {
       
    }

    /// <summary>
    /// 加载编辑
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadEditor()
    {
        string streamingPath = Application.streamingAssetsPath;

        DirectoryInfo dir = new DirectoryInfo(streamingPath + "/2DEditorGraphics");

        GetAllFiles(dir, this.transform);

        foreach (KeyValuePair<string,Image> dict in dic)
        {

            WWW www = new WWW("file://" + streamingPath + dict.Key);

            yield return www;

            if (www != null && string.IsNullOrEmpty(www.error))
            {
                Texture2D tex = www.texture;
                dict.Value.sprite =Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f)) ;
               
            }
            if (www.isDone)
            {
                www.Dispose();
            }
        }

    }

    /// <summary>
    /// 得到所有文件夹及文件
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="parent"></param>
    void GetAllFiles(DirectoryInfo dir, Transform parent)
    {
        FileSystemInfo[] file = dir.GetFileSystemInfos();

        foreach (FileSystemInfo i in file)
        {
            if (i is DirectoryInfo)
            {
                CreateEditor((DirectoryInfo)i,parent);
            }
            else
            {
                CreateEditorContent(i, parent);
            }
        }
    }

    /// <summary>
    /// 按钮点击事件
    /// </summary>
    /// <param name="go"></param>
    private void OnBtnClick(GameObject go)
    {
        bool isShow = !objDic[go.transform.parent.parent.name.Trim()].activeSelf;
        objDic[go.transform.parent.parent.name.Trim()].SetActive(isShow);

        if (isShow)
        {
            go.transform.rotation = Quaternion.Euler(show);
        }
        else
        {
            go.transform.rotation = Quaternion.Euler(hide);
        }
    }

    /// <summary>
    /// 生成编辑框
    /// </summary>
    /// <param name="i"></param>
    /// <param name="parent"></param>
    private void CreateEditor(DirectoryInfo i,Transform parent)
    {
        GameObject editor = Resources.Load<GameObject>("Prefabs/GraphicItem");
        GameObject temp = Instantiate(editor, parent);
        temp.name = i.Name;
        GameObject tempContent = temp.transform.Find("EditorContent").gameObject;
        
        objDic.Add(temp.name, tempContent);

        Button but = temp.transform.Find("Editor/Button").GetComponent<Button>();
        EventTriggerListener.Get(but.gameObject).onClick += OnBtnClick;

        Text tex = temp.transform.Find("Editor/Text").GetComponent<Text>();
        tex.text = temp.name;

        GetAllFiles(i, tempContent.transform);

        tempContent.SetActive(false);
    }

    /// <summary>
    /// 生成编辑框内容，字典存储image
    /// </summary>
    /// <param name="i"></param>
    /// <param name="parent"></param>
    private void CreateEditorContent(FileSystemInfo i,Transform parent)
    {
        string str = i.FullName;

        string path = Application.streamingAssetsPath;

        string strType = str.Substring(path.Length);

        if (strType.Substring(strType.Length - 3).ToLower() == "png")
        {

            if (dic.ContainsKey(strType))
            {
                //dic[strType] = spriteImg;
            }
            else
            {
                
                GameObject img = Resources.Load<GameObject>("Prefabs/Img");
                GameObject tempImg = Instantiate(img, parent);

                Image spriteImg = tempImg.GetComponent<Image>();

                strType = strType.Replace("\\", "/");
                //Debug.Log(strType);
                dic.Add(strType, spriteImg);
            }
        }
    }
}
