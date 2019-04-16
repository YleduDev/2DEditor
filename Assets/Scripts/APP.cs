using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using QFramework.TDE;
using UniRx;
using System;
using Newtonsoft.Json;
using System.IO;
using XZL;

namespace TDE
{
    /// <summary>
    /// 2d编辑入口
    /// </summary>
    public class APP : MonoBehaviour
    {
        TSceneData model;
        ServerData serverData;
        string path ;
        public void Awake()
        {
            //string json = PlayerPrefs.GetString("Test19");
            //path = FilePath.StreamingAssetsPath + "Graphics/Data/11.txt";
            // string json= File.ReadAllText(path);
            //FileMgr.Instance.GetZippathFile()
            //ZipUtil.ZipDirectory(FilePath.StreamingAssetsPath + "Graphics/Data", FilePath.StreamingAssetsPath +"Zip");
            //File.WriteAllText(path, json);
            #region test 数据
            //string json = PlayerPrefs.GetString("Test19");
            serverData = new ServerData();
            string json = HTTPMgr.Instance.CreateHTTPRequest("http://localhost:8080/vibe-web/twoDimension/findOneFilestr?name=Test19");
            //Log.I(json);
            model = TSceneData.Load(json);
            

            //string jsonSer = string.Format("name={0}&josnStr={1}", "Test19", json);

           // 
            #endregion
            ResMgr.Init();
            UIMgr.SetResolution(1920, 1080, 0);
            UIMgr.OpenPanel<UIGraphicMenuPanel>(new UIGraphicMenuPanelData() { model = model });
            UIMgr.OpenPanel<UIShowPanel>(new UIShowPanelData() { model=model});
            UIMgr.OpenPanel<UITestPanel>(new UITestPanelData() {model= serverData });
        }

        private void OnApplicationQuit()
        {
            string json = model.Save();
            // File.WriteAllText(path, json);
            // PlayerPrefs.SetString("Test19", model.Save());
            string jsonSer = string.Format("name={0}&josnStr={1}", "Test19", json);
            HTTPMgr.Instance.CreateHTTPRequest(jsonSer, "http://localhost:8080/vibe-web/twoDimension/upload");
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Delete))
            {
               if (Global.OnSelectedGraphic.IsNotNull())
                {
                    model.Remove(Global.OnSelectedGraphic.Value);
                }
            }
        }

    }
    //all场景对象
    public class TSceneDataList
    {
        public Dictionary<string, TSceneData> sceneDataDict;
    }
}
