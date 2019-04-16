using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XZL;
namespace TDE
{
    public class ServerData
    {

        string login = "login_id=admin&password=123456";
        string loginURL = "http://localhost:8080/vibe-web/user/login";
        string floorsUrl = "http://localhost:8080/vibe-web/asset/initAssetAllTree?flag=space&locationRoot=1";
        string categoryUrl = "http://localhost:8080/vibe-web/getItemsTree?catalogId=2002";
        string idUrl = "http://localhost:8080/vibe-web/asset/toDeviceEdit?id=";

        Dictionary<string, AssetNode> assetPool = new Dictionary<string, AssetNode>();

         
        public ServerData()
        {
            HTTPMgr.Instance.PostLogin(login, loginURL);
        }
        public SpaceTree GetFloors()
        {
            //Â¥²ã
            var floorsJson = HTTPMgr.Instance.CreateHTTPRequest(floorsUrl);
            List<SpaceTree> root = SerializeHelper.FromJson<List<SpaceTree>>(floorsJson);
            return root[0];
        }

        public List<SpaceTree> GetCategory()
        {
            //ÀàÐÍ
            var categoryJson = HTTPMgr.Instance.CreateHTTPRequest(categoryUrl);
            return SerializeHelper.FromJson<List<SpaceTree>>(categoryJson);
        }

        public AssetNode GetAssetForId(string id)
        {
            if (assetPool.ContainsKey(id)) return assetPool[id];
            var idJson = HTTPMgr.Instance.CreateHTTPRequest(idUrl + id);
            //Log.I(id+"   "+ idJson);
            var asset = idJson.IsNullOrEmpty() ? null : SerializeHelper.FromJson<AssetNode>(idJson);
            if (asset.IsNotNull()) assetPool.Add(id, asset);
            return asset;
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
}
