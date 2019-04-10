using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UniRx;
using System.Text;
using XZL;
using QFramework.TDE;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using TDE;

public class TestAPP : MonoBehaviour
{

    public class WWWGetServerDatas : NodeAction
    {
        protected override void OnBegin()
        {

            //ObservableWWW.Get(url).Subscribe(
            //    text =>
            //    {
            //        text.LogInfo();
            //        Finish();
            //    }, e =>
            //    {
            //        e.LogInfo();
            //    }
            //    );

            //ObservableWWW.Get(url)
            //     .Subscribe(
            //    text =>
            //    {
            //        text.LogInfo();
            //        Finish();
            //    }, e =>
            //    {
            //        e.LogInfo();
            //    }
            //    );
        }
    }

    public void Start()
    {
        ServerData data = new ServerData();
        ResMgr.Init();
        UIMgr.OpenPanel<UIServerDatasMenuPanel>(new UIServerDatasMenuPanelData() { ServerData= data });
    }
}

