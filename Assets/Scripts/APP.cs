using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using QFramework.TDE;
namespace TDE
{
    /// <summary>
    /// 2d编辑入口
    /// </summary>
    public class APP : MonoBehaviour
    {
        public void Awake()
        {
            //test 数据
            TSceneData model = new TSceneData();
            model.graphicDataList = new List<T_Graphic>(2);
            model.graphicDataList.Add(new T_Graphic() { graphicType = GraphicType.Text, localScale=Vector3.one, localPos = new Vector3(-645f, 395f, 0) });
            model.graphicDataList.Add (new T_Graphic() { graphicType = GraphicType.Image, localScale = Vector3.one, localPos = new Vector3(776f, -374f, 0) });

            ResMgr.Init();
            UIMgr.SetResolution(1920, 1080, 0);
            UIMgr.OpenPanel<UIShowPanel>(new UIShowPanelData() { model=model});
        }

    }
    //场景对象
    public class TSceneData
    {
        public List<T_Graphic> graphicDataList;
    }
    //所以场景对象
    public class TSceneDataList
    {
        public Dictionary<string, TSceneData> sceneDataDict;
    }
}
