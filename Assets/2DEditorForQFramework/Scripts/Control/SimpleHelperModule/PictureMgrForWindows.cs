using System.Collections;
using UnityEngine;
using QFramework;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System;
using TDE;
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

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenDialogDir
{
    public IntPtr hwndOwner = IntPtr.Zero;
    public IntPtr pidlRoot = IntPtr.Zero;
    public String pszDisplayName = null;
    public String lpszTitle = null;
    public UInt32 ulFlags = 0;
    public IntPtr lpfn = IntPtr.Zero;
    public IntPtr lParam = IntPtr.Zero;
    public int iImage = 0;
}
#endregion

#region 系统函数调用类
public class LocalDialog
{
    //链接指定系统函数       打开文件对话框
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([InAttribute, OutAttribute] OpenFileName ofn);
    public static bool GetOFN([InAttribute, OutAttribute] OpenFileName ofn)
    {
        return GetOpenFileName(ofn);
    }

    //链接指定系统函数        另存为对话框
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetSaveFileName([InAttribute, OutAttribute] OpenFileName ofn);
    public static bool GetSFN([InAttribute, OutAttribute] OpenFileName ofn)
    {
        return GetSaveFileName(ofn);
    }


    [DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern IntPtr SHBrowseForFolder([InAttribute, OutAttribute] OpenDialogDir ofn);

    [DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool SHGetPathFromIDList([InAttribute] IntPtr pidl, [InAttribute, OutAttribute] char[] fileName);

}
#endregion
public class PictureMgrForWindows : Singleton<PictureMgrForWindows> {

    private PictureMgrForWindows() { }
    
    private string imgtype = "*.JPG|*.PNG";

    private Texture2D originalTex;

    public void GetPather(string path)
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
            string texPath = openFileName.file;

            FileStreamLoadTexture(path, texPath);
        }
    }

    public void GetPatherForDir(string path,Action act)
    {
        OpenDialogDir ofn2 = new OpenDialogDir
        {
            pszDisplayName = new string(new char[2000]),
            // 存放目录路径缓冲区          
            lpszTitle = "Open Project"// 标题          
        };
        // ofn2.ulFlags = BIF_NEWDIALOGSTYLE | BIF_EDITBOX; // 新的样式,带编辑框          
        IntPtr pidlPtr = LocalDialog.SHBrowseForFolder(ofn2);
        char[] charArray = new char[2000];
        for (int i = 0; i < 2000;i++ )
        charArray[i] = '\0';
        if (LocalDialog.SHGetPathFromIDList(pidlPtr, charArray))
        {
            string fullDirPath = new String(charArray);

            fullDirPath = fullDirPath.Substring(0, fullDirPath.IndexOf('\0'));

           string texPath = fullDirPath;

            FileSereamLoadDir(path, texPath, act);
        }
        
        //if (LocalDialog.GetSaveFileName(openFileName))
        //{
        //    

        //    FileSereamLoadDir(path);
        //}
    }


    /// <summary>
    /// 文件流加载图片
    /// </summary>
    private void FileStreamLoadTexture(string path, string texPath)
    {
        //C:\Users\Lenovo\Pictures\程序员的桌面\code-wallpaper-15.jpg
        string[] names = texPath.Split('\\');
        string name = names[names.Length - 1];
        name = name.Split('.')[0];

        //如果已经有了 就直接退出
        if (File.Exists(path + "\\" + name + ".png")|| !File.Exists(texPath)) return;

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
        
        originalTex.name = name;
        //Log.I(name);
        //GetTexture();
        File.WriteAllBytes(path +"\\" +name + ".png", originalTex.EncodeToPNG());
    }
    //读取文件夹加载文件流
    private  void FileSereamLoadDir(string path,string texPath, Action act)
    {
        Log.I(texPath);
        DirectoryInfo dir = new DirectoryInfo(texPath);
        
        List<FileInfo> newFiles = new List<FileInfo>();

        string[] ImageType = imgtype.Split('|');

        for (int i = 0; i < ImageType.Length; i++)
        {
            FileInfo[] files = dir.GetFiles(ImageType[i],SearchOption.AllDirectories);
            newFiles.AddRange(files);
        }

        for (int i = 0; i < newFiles.Count; i++)
        {
            string dirFull = FilePath.StreamingAssetsPath + Global.CustomCinfigGraphicsPathName + "/" + newFiles[i].Directory.Name;
            if (!Directory.Exists(dirFull)) Directory.CreateDirectory(dirFull);
            FileStreamLoadTexture(dirFull, newFiles[i].FullName);
        }
        act?.Invoke();
       // DirectoryInfo[] childDirs = dir.GetDirectories();
    }

    //删除路径下所有文件
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
                //if (files[i].Name.EndsWith(".meta"))
                //{
                //    continue;
                //}
                string FilePath = fullPath + "/" + files[i].Name;
                //print(FilePath);
                File.Delete(FilePath);
            }
            return true;
        }
        return false;
    }

    //删除文件
    public void DeleteFile(string path)
    {
        if (!File.Exists(path)) return;

        File.Delete(path);
    }
    public void DeleteDir(string path)
    {
        if (!Directory.Exists(path)) return;
        DeleteAllFile(path);
        Directory.Delete(path);
    }
}
