using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using QFramework.TDE;
using UniRx;
using System.Runtime.InteropServices;
using System;
using Newtonsoft.Json;
using XZL;

namespace TDE
{
    /// <summary>
    /// 2d编辑入口
    /// </summary>
    public class APP : MonoBehaviour
    {

        private void Awake()
        {
            //控制帧数
            Application.targetFrameRate = 30;
        }

#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern string MyURL();

        [DllImport("__Internal")]
        private static extern string MyLog();

        [DllImport("__Internal")]
        private static extern string GetText();
#else
#endif
        ServerData serverData;
        UIShowPanel showPanel;
#if UNITY_WEBGL
        IEnumerator Start() 
#else
        IEnumerator Start()
#endif
        {
#if UNITY_WEBGL
            //web端的url是浏览器托管 所以获取url
            string url = MyURL();
            //获取浏览器点击默认场景名称
            string fireText = GetText();
            //连接
            serverData = new ServerData(url);
            yield return StartCoroutine(serverData.ie);
            //serverData = new ServerData();
            //yield return StartCoroutine(serverData.ie);
            //获取web的资源加载
            yield return StartCoroutine(WebGLAssetBundleMrg.Instance.Load());
#else
            //资源加载
            ResMgr.Init();
            serverData = new ServerData();
            yield return StartCoroutine(serverData.ie);
#endif
            UIMgr.SetResolution(1920, 1080, 0);      

            UIMgr.OpenPanel<UIGraphicMenuPanel>();
#if UNITY_WEBGL
#else
            UIMgr.OpenPanel<UIOperatePanel>();
            UIMgr.OpenPanel<UIFunctionPanel>();
            UIMgr.OpenPanel<UIAttributePanel>();
            UIMgr.OpenPanel<UICanvasPanel>();

#endif
            showPanel = UIMgr.OpenPanel<UIShowPanel>();
            //加载历史场景
            Global.currentSceneData.Value = TSceneData.Load(PlayerPrefs.GetString("OnDrawing", ""));
            //协程 订阅 场景切换功能
            IEnumerator ie = ShowPanle();
            yield return  StartCoroutine(ie);


#if UNITY_WEBGL
#else
               UIMgr.OpenPanel<UIUPloadPanel>(new UIUPloadPanelData() { model = serverData }).Hide();
               UIMgr.OpenPanel<UIScenesScrollViewPanel>(new UIScenesScrollViewPanelData() { model = serverData }).Hide();
               UIMgr.OpenPanel<UIServerDatasMenuPanel>(new UIServerDatasMenuPanelData() { ServerData = serverData }).Hide();
               UIMgr.OpenPanel<UILinkedServerPanel>(new UILinkedServerPanelData() { ServerData = serverData }).Hide();
           
#endif
#if UNITY_WEBGL
             if (!fireText.IsNullOrEmpty())ChangeScene(fireText);
#endif
        }
        //订阅
        IEnumerator ShowPanle()
        {
            Global.currentSceneData.ObserveOnMainThread().Subscribe(data => StartCoroutine(TSceneChange(data)));
            yield return null;
        }
        //切换场景
         IEnumerator TSceneChange( TSceneData data)
        {
            if (data == null) data = TSceneData.Load("");
            IEnumerator ie = showPanel.ModelChangeInit(data);
            yield return StartCoroutine(ie);
        }

        private void OnApplicationQuit()
        {
            if (Global.currentSceneData.Value.IsNotNull())
            {
                Global.OnClick(null);
               PlayerPrefs.SetString("OnDrawing", Global.currentSceneData.Value.Save());
            }
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Delete))
            {
                if (Global.OnSelectedGraphic.IsNotNull() && Global.OnSelectedGraphic.Value.IsNotNull())
                {
                    Global.currentSceneData.Value.Remove(Global.OnSelectedGraphic.Value);
                }
            }
            //if (Input.GetKeyUp(KeyCode.Space))
            //{
            //    Global.currentSceneData.Value.WidgetDataList.ForEach(item => item.localPos.Value += new Vector2(.5f * Global.currentCanvasWidth.Value, .5f * -Global.currentCanvasheight.Value));
            //    Global.currentSceneData.Value.ImageDataList.ForEach(item => item.localPos.Value += new Vector2(.5f * Global.currentCanvasWidth.Value, .5f * -Global.currentCanvasheight.Value));
            //}

        }
#if UNITY_WEBGL
        private void OnDestroy()
        {
            WebGLAssetBundleMrg.Instance.ForceUnLoadAll();
        }

        public void WebGLCallBack(string data)
        {      
            try
            {
                WebSocketMessage message = JsonConvert.DeserializeObject<WebSocketMessage>(data);
                if (message != null) Global.UpdataBindData(message);
            }
            catch (Exception e)
            {
                Log.I(e.Message);
            }
           
        }

        public void ChangeScene( string sceneName)
        {
            serverData.GetScene(sceneName, (scene) => {
                if (scene.IsNotNull())
                {
                    Global.currentSceneData.Value = scene;
                    Global.WebglShowScene.Value = true;
                } 
            });
        }
#endif

    }
}
