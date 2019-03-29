using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDE;
using System.IO;
using QFramework;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CreateOrUpdataGraphicsMenmConfig{

    

#if UNITY_EDITOR
    [MenuItem("Tools/CreateOrUdateGraphicsMenuConfig")]
#endif
    private static void MenuClick()
    {
        string fileName =  Global.allGraphicsFillName;

        Dictionary<string, List<string>>GraphicsMenuConfig = new Dictionary<string, List<string>>();

        string path = Application.dataPath+"/Res/" + fileName;

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
       File.WriteAllText(Application.dataPath + "/Res/Config/" + Global.GraphisMenuConfigPathName+".txt", GraphicsMenuConfig.ToJson());
    }
}
