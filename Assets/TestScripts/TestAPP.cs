using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UniRx;
using System.Text;
using XZL;
using QFramework.TDE;
public class TestAPP : MonoBehaviour {

    static string login = "login_id=admin&password=123456";
    static string loginURL = "http://192.168.1.201:8080/vibe-web/user/login";
   static  string floorsUrl= "http://192.168.1.201:8080/vibe-web/asset/initAssetAllTree?flag=space&locationRoot=1";
    static string categoryUrl = "http://192.168.1.201:8080/vibe-web/getItemsTree?catalogId=2001";
    public class WWWGetServerDatas:NodeAction
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
        // ActionQueue.Append(new WWWGetServerDatas());
        //HttpHelper.HttpPost(loginURL, dictPost).LogInfo();
        //HttpHelper.HttpPost(url, dictPost).LogInfo();

        NetWorkManager.Instance.PostLogin(login, loginURL);
        var floorsJson=  NetWorkManager.Instance.CreateHTTPRequest(floorsUrl);
        var categoryJson= NetWorkManager.Instance.CreateHTTPRequest(categoryUrl);

        ResMgr.Init();
        UIMgr.OpenPanel<UIServerDatasMenuPanel>(new UIServerDatasMenuPanelData() { });
    }


}
