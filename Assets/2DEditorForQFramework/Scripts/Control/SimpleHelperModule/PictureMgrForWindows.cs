using System.Collections;
using UnityEngine;
using QFramework;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System;
using TDE;
#region OpenFileName���ݽ�����
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

#region ϵͳ����������
public class LocalDialog
{
    //����ָ��ϵͳ����       ���ļ��Ի���
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([InAttribute, OutAttribute] OpenFileName ofn);
    public static bool GetOFN([InAttribute, OutAttribute] OpenFileName ofn)
    {
        return GetOpenFileName(ofn);
    }

    //����ָ��ϵͳ����        ���Ϊ�Ի���
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

        openFileName.filter = "ͼƬ�ļ�(*.jpg,*.png,*.bmp)\0*.jpg;*.png;*.bmp";
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//Ĭ��·��
        openFileName.title = "ѡ��ͼƬ";

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
            // ���Ŀ¼·��������          
            lpszTitle = "Open Project"// ����          
        };
        // ofn2.ulFlags = BIF_NEWDIALOGSTYLE | BIF_EDITBOX; // �µ���ʽ,���༭��          
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
    /// �ļ�������ͼƬ
    /// </summary>
    private void FileStreamLoadTexture(string path, string texPath)
    {
        //C:\Users\Lenovo\Pictures\����Ա������\code-wallpaper-15.jpg
        string[] names = texPath.Split('\\');
        string name = names[names.Length - 1];
        name = name.Split('.')[0];

        //����Ѿ����� ��ֱ���˳�
        if (File.Exists(path + "\\" + name + ".png")|| !File.Exists(texPath)) return;

        //ͨ��·�����ر���ͼƬ
        FileStream fs = new FileStream(texPath, FileMode.Open);
        byte[] buffer = new byte[fs.Length];
        fs.Read(buffer, 0, buffer.Length);
        fs.Close();
        originalTex = new Texture2D(2, 2);
        var iSLoad = originalTex.LoadImage(buffer);
        originalTex.Apply();
        if (!iSLoad)
        {
            Debug.Log("Texture���ڵ�����Textureʧ��");
        }
        
        originalTex.name = name;
        //Log.I(name);
        //GetTexture();
        File.WriteAllBytes(path +"\\" +name + ".png", originalTex.EncodeToPNG());
    }
    //��ȡ�ļ��м����ļ���
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

    //ɾ��·���������ļ�
    public bool DeleteAllFile(string fullPath)
    {
        //��ȡָ��·�������������Դ�ļ�  Ȼ�����ɾ��
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

    //ɾ���ļ�
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
