/************************************************************
  FileName: GetPicture.cs
  Author:杜乐      Version :1.0          Date: 2018-12-25
  Description:获取本地图片
************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;

using UnityEngine;
using UnityEngine.UI;
using System.Linq;

#region OpenFileName数据接收类
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenFileName
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public String file = null;
    public int maxFile = 0;
    public String fileTitle = null;
    public int maxFileTitle = 0;
    public String initialDir = null;
    public String title = null;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    public String defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public String templateName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;
}
#endregion

#region 系统函数调用类
public class LocalDialog
{
    //链接指定系统函数       打开文件对话框
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    public static bool GetOFN([In, Out] OpenFileName ofn)
    {
        return GetOpenFileName(ofn);
    }

    //链接指定系统函数        另存为对话框
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetSaveFileName([In, Out] OpenFileName ofn);
    public static bool GetSFN([In, Out] OpenFileName ofn)
    {
        return GetSaveFileName(ofn);
    }
}
#endregion

#region 入口类
public class GetPicture : MonoBehaviour {

    public GameObject iconItemPrefab;
    public Transform iconParent;

    private string texPath;
    private Texture2D originalTex;
    //private int count;
    private Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> bgSprites = new Dictionary<string, Sprite>();
    string dirPath,bgPath;
    private string picturesPath,bgImagePath;
    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        //count = PlayerPrefs.GetInt("Count",0);

        dirPath = Application.streamingAssetsPath+ "/Schematic/PinTu/Pictures";
        bgPath= Application.streamingAssetsPath + "/Schematic/PinTu/BG";
        picturesPath = Application.streamingAssetsPath + "\\Schematic\\PinTu\\Pictures//";
        bgImagePath= Application.streamingAssetsPath + "\\Schematic\\PinTu\\BG//";
        DirectoryInfo mydir = new DirectoryInfo(dirPath);
        if (!mydir.Exists)
            Directory.CreateDirectory(dirPath);
        DirectoryInfo bgDir = new DirectoryInfo(bgPath);
        if (!bgDir.Exists)
            Directory.CreateDirectory(bgPath);
        ReadPicture();
        //初始化bg字典
        GetBGTexture();
       // GetTexture();
    }

    /// <summary>
    /// 加载图片按钮点击事件
    /// </summary>
    public void OnLoadButtonClick()
    {
        GetPather(picturesPath);
        ReadPicture();
    }

    public void DeletePictures(string deletePath)
    {
        string path = picturesPath + deletePath + ".png";
        //Debug.Log(path);
        Delete(path);

        ReadPicture();
    }

    public string LoadBG()
    {
        //删除bg下所有文件
        DirectoryInfo bgDir = new DirectoryInfo(bgPath);
        if (bgDir.Exists) DeleteAllFile(bgPath);
        //创建新textrue
        GetPather(bgImagePath);
        //将新textrue
       string key=GetBGTexture();
        return key;
    }
    /// <summary>
    /// 获得路径
    /// </summary>
    private void GetPather(string path)
    {
        OpenFileName openFileName = new OpenFileName();
        openFileName.structSize = Marshal.SizeOf(openFileName);

        openFileName.filter = "图片文件(*.jpg,*.png,*.bmp)\0*.jpg;*.png;*.bmp";
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默认路径
        openFileName.title = "选择图片";

        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

        if (LocalDialog.GetSaveFileName(openFileName))
        {
            texPath = openFileName.file;

            FileStreamLoadTexture(path);
        }
    }

    /// <summary>
    /// 文件流加载图片
    /// </summary>
    private void FileStreamLoadTexture(string path)
    {
        //通过路径加载本地图片
        FileStream fs = new FileStream(texPath, FileMode.Open);
        byte[] buffer = new byte[fs.Length];
        fs.Read(buffer, 0, buffer.Length);
        fs.Close();
        originalTex = new Texture2D(2, 2);
        var iSLoad = originalTex.LoadImage(buffer);  
        originalTex.Apply();
        if (!iSLoad)
        {
            Debug.Log("Texture存在但生成Texture失败");
        }
        //C:\Users\Lenovo\Pictures\程序员的桌面\code-wallpaper-15.jpg
        string name = SchematicControl.GetEndStr(texPath, '\\'); 
        name = SchematicControl.GetBeginStr(name, '.');
        originalTex.name = name;
        //GetTexture();
        File.WriteAllBytes(path + name + ".png", originalTex.EncodeToPNG());
    }

    public bool DeleteAllFile(string fullPath)
    {
        //获取指定路径下面的所有资源文件  然后进行删除
        if (Directory.Exists(fullPath))
        {
            DirectoryInfo direction = new DirectoryInfo(fullPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

            //Debug.Log(files.Length);

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }
                string FilePath = fullPath + "/" + files[i].Name;
                //print(FilePath);
                File.Delete(FilePath);
            }
            return true;
        }
        return false;
    }

    private void Delete(string path)
    {
        if (!File.Exists(path)) return;

        File.Delete(path);
    }

    /// <summary>
    /// 转换为Sprite
    /// </summary>
    private Sprite ChangeToSprite(Texture2D tex)
    {
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        sprite.name = tex.name;
        return sprite;
    }

 //读取
    public  void ReadPicture()
    {
        //清空
        if (iconParent && iconParent.childCount > 0)
        {
            foreach (Transform item in iconParent)
            {
                Destroy(item.gameObject);
            }
        }

        GetTexture();

        foreach (var item in sprites.Values)
        {
            GameObject go = Instantiate(iconItemPrefab, iconParent);
            go.GetComponent<Image>().sprite = item;
            go.name = item.name;
        }
    }


    private void GetTexture()
    {
        sprites.Clear();

        DirectoryInfo dir = new DirectoryInfo(dirPath);
        FileInfo[] files = dir.GetFiles("*.png"); //获取所有文件信息
        foreach (FileInfo file in files)
        {
            FileStream fs = new FileStream(dirPath +"/"+ file.Name, FileMode.Open);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(buffer);
            tex.Apply();
            tex.name = file.Name.Replace(".png", ""); 
            sprites.Add(tex.name, ChangeToSprite(tex));
        }
    }
    private string GetBGTexture()
    {
        bgSprites.Clear();

        DirectoryInfo bg = new DirectoryInfo(bgPath);
        FileInfo[] files = bg.GetFiles("*.png"); //获取所有文件信息
        foreach (FileInfo file in files)
        {
            FileStream fs = new FileStream(bgPath + "/" + file.Name, FileMode.Open);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(buffer);
            tex.Apply();
            tex.name = file.Name.Replace(".png", "");
            bgSprites.Add(tex.name, ChangeToSprite(tex));
            return tex.name;
        }
        return null;
    }


    public Sprite GetSprite( string key)
    {
       key= key.Trim();
        //int spriteKey;
        if (sprites != null && sprites.ContainsKey(key)) return sprites[key];
        //if (sprites != null && int.TryParse(key, out spriteKey) && sprites.ContainsKey(spriteKey))
        //    return sprites[spriteKey];
            return null;
    }

    public Sprite GetSpriteByBg(string key)
    {
        key = key.Trim();
        if (bgSprites != null && bgSprites.ContainsKey(key)) return bgSprites[key];
        return null;
    }
}
#endregion