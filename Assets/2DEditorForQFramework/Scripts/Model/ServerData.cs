using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using XZL;
using System;

namespace TDE
{
    public  class ServerData 
    {
       public static string HeadUrl= "http://localhost:8080";

        string port = ":8080";
        string login = "login_id=admin&password=123456";
        string loginURL = "/vibe-web/user/login";
        
        //Asset
        string floorsUrl = "/vibe-web/asset/initAssetAllTree?flag=space&locationRoot=1";
        string categoryUrl = "/vibe-web/getItemsTree?catalogId=2002";
        static string idUrl = "/vibe-web/asset/toDeviceEdit?id=";
        static string queryAssetListUrl = "/vibe-web/asset/queryAssetAddEnergyDeviceType/";

        string nameOrCaption = "/vibe-web/asset/toAssetLikeNameAndCaption?";
        string nameContent = "name=";
        string captionContent = "&&caption=";
        string kingType = "&&king=";
        //2dEditeor
        string addOrUpdataSceneDataURL= "/vibe-web/twoDimension/upload";
        string findAllSceneDataURL = "/vibe-web/twoDimension/findAllName";
        string findOneSceneDataURL = "/vibe-web/twoDimension/findOneFilestr?name=";
        string deleteSceneDataURL = "/vibe-web/twoDimension/deleteTwoDimensionEditor?name=";

       public System.Collections.IEnumerator ie;
         
        public ServerData(string url,Action act=null,Action loseAct=null)
        {
            try
            {              
                if (!url.Contains(port)) url = url + port;
                HeadUrl = "http://" + url;
#if UNITY_WEBGL
#else
             act += () =>
                { //订阅
                    WebSocketInstance.Instance.InitWebSocket(url + "/vibe-web");
                };
#endif
                ie = XZL.Network.NetWorkManager.Instance.IELogin(HeadUrl + loginURL, "admin", "123456", act, loseAct);
                
                PlayerPrefs.SetString("LastServerURL", url);
            }
            catch (Exception e)
            {
                Log.I(e.Message);
            }
           
        }
        public ServerData()
        {
            try
            {
                string newURL = PlayerPrefs.GetString("LastServerURL", HeadUrl);
                if (!newURL.Contains(port)) newURL = newURL + port;
                HeadUrl = "http://" + newURL;
                Log.I(HeadUrl + loginURL);
                ie = XZL.Network.NetWorkManager.Instance.IELogin(HeadUrl + loginURL, "admin", "123456",
#if UNITY_WEBGL
                    null
#else
                      () => WebSocketInstance.Instance.InitWebSocket(HeadUrl.Replace("http://", "") + "/vibe-web")
#endif
                    , null);
            }
            catch (Exception e)
            {
                Log.I(e.Message);
            }
        }


        public static void GetAssetNodeForID(string id,Action<string> re)
        {
            try
            {
                XZL.HTTPMgr.Instance.GetAssetForId(HeadUrl + idUrl + id, re);
            }
            catch (Exception e)
            {

                Log.I(e.Message);
            }
           
        }
        public static void GetAssetNodesForDeviceId(string id, Action<string> re)
        {
            try
            {
                XZL.HTTPMgr.Instance.GetAssetForId(HeadUrl + queryAssetListUrl + id, re);
                Log.I(HeadUrl + queryAssetListUrl + id);
            }
            catch (Exception e)
            {

                Log.I(e.Message);
            }

        }


        public void GetAssetForSizer(AssetKindType kind, AssetCheckType check, string content,Action<List<AssetNode>> act)
        {
            string tUrl;
            if (check == AssetCheckType.caption)
            {
                 tUrl = HeadUrl + nameOrCaption + nameContent + captionContent+ content + kingType+ kind.ToString();              
            }
            else
            {
                tUrl = HeadUrl + nameOrCaption + nameContent +content + captionContent + kingType + kind.ToString();
            }
            XZL.HTTPMgr.Instance.GetAssetIdForHttp(tUrl, act);                        
        }

        public void GetAllScenes(Action<List<string>> action)
        {

            XZL.HTTPMgr.Instance.FindAllScene(HeadUrl + findAllSceneDataURL, action);
            
         
        }
        public void GetScene(string name, Action<TSceneData> re)
        {
            TSceneData sceneData;

            XZL.HTTPMgr.Instance.FindOneScene(HeadUrl + findOneSceneDataURL + name, (json)=> {
                 if (!json.IsNullOrEmpty())
                 {
                     sceneData = TSceneData.Load(json);

                     re?.Invoke(sceneData);
                 }

             });
        }

        public IEnumerator SceneAddOrUpdata(WWWForm jsonSer, Action<string> re)
        {                  
           yield return XZL.HTTPMgr.Instance.UpLoadSecne(HeadUrl + addOrUpdataSceneDataURL, jsonSer, re);
        }


    }

    [System.Serializable]
    public class SpaceTree
    {
        public string modelName;
        public string modelCoordinate;
        public string modelAngle;
        public string modelSize;
        public int id;
        public string text;
        public string kind;
        public int status;
        public string grade;
        public string time;
        public string catalog;
        public string catalogName;
        public string value;
        public string unit;
        public string valueStr;
        public string errorMsg;
        public string redio;
        public string kindStr;
        public string statusStr;
        public string monitorType;
        public string name;
        public string fullName;
        public string username;
        public string password;
        public string host;
        public int port;
        public string rtspUrlPattern;
        public string parent;
        public string itemizeType;
        public SpaceTree[] nodes;
    }
    [System.Serializable]
    public class AssetNode
    {
        public string fullName;
        public string valueStr;
        public string kindStr;
        public string siteId;
        public string id;
        public string typeName;
        public string parentId;
        public string parentCaption;
        public string name;
        public string caption;
        public string memo;
        public string state;
        public string error;
        public string enabled;
        public string typeCaption;
        public string parent;
        public string kind;
        public string removed;
        public List<ValueNode> valueList;
        public string value;
        public string serviceCaption;
        public string time_interval;
        public string unit;
        public string time_unit;
        public string refresh_delay;
        public string warn_cond;
        public string transform;
        public string source;
        public string catalogId;
        public string vendor;
        public string purchase_date;
        public string warranty_date;
        public string specification;
        public string models;
        public string increse_way;
        public string international_code;
        public string detail_config;
        public string production_date;
        public string using_department;
        public string departId;
        public string using_state;
        public string device_type;
        public string location;
        public string spaceCaption;
        public string keepers;
        public string userName;
        public string userId;
        public string quantity;
        public string deviceUnit;
        public string price;
        public string amount;
        public string enabing_date;
        public string maintenance_interval;
        public string original_value;
        public string use_year;
        public string salvage;
        public string salvage_value;
        public string depreciation_method;
        public string maintenance_time;
        public string maintenance_people;
        public string scrap;
        public string is_using;
        public string qrcode;
        public string username;
        public string password;
        public string host;
        public string port;
        public string rtspUrlPattern;
        public string spaceId;
        public string monitorType;
        public string seqence;
        public string detact_interval;
    }
    [System.Serializable]
    public class ValueNode
    {
        public int asset;
        public string name;
        public string value;
        public string caption;
    }
    [System.Serializable]
    public class QueryAsset
    {
        public int total;
        public List<AssetNode> rows;
    }
}
