using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using QFramework.TDE;
using UniRx;
using System;
using Newtonsoft.Json;

namespace TDE
{
    /// <summary>
    /// 2d编辑入口
    /// </summary>
    public class APP : MonoBehaviour
    {
        TSceneData model;
        ServerData serverData;
        public void Awake()
        {
            #region test 数据
            model = TSceneData.Load();
            model.InitGlobalBindDataDict();
            serverData = new ServerData();
            #endregion
            ResMgr.Init();
            UIMgr.SetResolution(1920, 1080, 0);
            UIMgr.OpenPanel<UIGraphicMenuPanel>(new UIGraphicMenuPanelData() { model = model });
            UIMgr.OpenPanel<UIShowPanel>(new UIShowPanelData() { model=model});
            UIMgr.OpenPanel<UITestPanel>(new UITestPanelData() {model= serverData });
        }

        private void OnApplicationQuit()
        {
            model.Save();
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
