using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDE;
#if UNITY_EDITOR
using System.IO;
#endif
using QFramework;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CreateOrUpdataGraphicsMenmConfig{

    

#if UNITY_EDITOR
    [MenuItem("Tools/CreateOrUdateGraphicsMenuConfig")]
    private static void MenuClick()
    {
        string fileName =  Global.allGraphicsFillName;

        Dictionary<string, List<string>>GraphicsMenuConfig = new Dictionary<string, List<string>>();

        string path = Application.dataPath+"/Art/" + fileName;

        DirectoryInfo dir = new DirectoryInfo(path);

        DirectoryInfo[] childDirs = dir.GetDirectories();

        foreach (DirectoryInfo i in childDirs)
        {
            if (i.Parent.Name.Equals(fileName))
            {
                //获取文件夹下所有png文件信息
                FileInfo[] files = i.GetFiles("*.png");
                List<string> listStr = new List<string>();
                files.ForEach(file => listStr.Add(file.Name.Replace(".png","")));
                GraphicsMenuConfig.Add(i.Name, listStr);
            }
        }
       File.WriteAllText(Application.dataPath + "/Art/Config/" + Global.GraphisMenuConfigPathName+".txt", GraphicsMenuConfig.ToJson());
    }

    [MenuItem("Tools/CreateOrUdateUIWidgetConfig")]
    private static void UIWidgetMenuClick()
    {
        string fileName = Global.allWidgetsFillName;

        List<string> GraphicsMenuConfig = new List<string>();

        string path = Application.dataPath + "/Art/" + fileName;
       // Debug.Log(path);
        DirectoryInfo dir = new DirectoryInfo(path);
        // 获取文件夹下所有png文件信息
        FileInfo[] files = dir.GetFiles("*.png");
        foreach (FileInfo i in files)
        {     
                string value="";
                files.ForEach(file => value= file.Name.Replace(".png", ""));
                GraphicsMenuConfig.Add(value);
            
        }
        File.WriteAllText(Application.dataPath + "/Art/Config/" + Global.WidgetConfigPathName + ".txt", GraphicsMenuConfig.ToJson());
    }

    [MenuItem("Tools/CreateWidgetPrefabConfig")]
    private static void UIWidgetPrefabMenuClick()
    {
        string fileName = Global.allWidgetsFillName;

        Dictionary<string,string> GraphicsMenuConfig = new Dictionary<string, string>();

        string path = Application.dataPath + "/Art/" + fileName;
        // Debug.Log(path);
        DirectoryInfo dir = new DirectoryInfo(path);
        // 获取文件夹下所有png文件信息
        FileInfo[] files = dir.GetFiles("*.png");
        foreach (FileInfo i in files)
        {
            string value = "";
            files.ForEach(file => value = file.Name.Replace(".png", ""));
            GraphicsMenuConfig.Add(value," ");

        }
        File.WriteAllText(Application.dataPath + "/Art/Config/" + Global.WidgetPrefabConfigPathName + ".txt", GraphicsMenuConfig.ToJson());
    }

#endif
}
