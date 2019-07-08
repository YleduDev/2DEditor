using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.Networking;
using System.Linq;

public class WebGLAssetBundleMrg : Singleton<WebGLAssetBundleMrg> {

    private WebGLAssetBundleMrg() { }
    private Dictionary<string, AssetBundle> bundlesLoadedDict = new Dictionary<string,AssetBundle>(); //已下载的资源列表
    string streamingAssetPath;

    private Dictionary<string, Object> allObjs = new Dictionary<string, Object>();  //所有预设体字典


    public override void OnSingletonInit()
    {
        streamingAssetPath = Application.streamingAssetsPath + "/AssetBundles/WebGL/";     
    }
    

     
   public IEnumerator Load()
    {
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(streamingAssetPath + "WebGL");
        yield return request.SendWebRequest();
        AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);
        //  Debug.Log("bundle name:" + ab.name);
        //bundlesLoadedDict.Add(ab.name,ab);
      
        AssetBundleManifest manifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        string[] bundleNames = manifest.GetAllAssetBundles();
        foreach (var item in bundleNames)
        {
            //加载依赖
            string[] dependence = manifest.GetAllDependencies(item);
            foreach (var dep in dependence)
            {
                if (bundlesLoadedDict.ContainsKey(dep)&& bundlesLoadedDict[dep]!=null) continue;
                UnityWebRequest re = UnityWebRequestAssetBundle.GetAssetBundle(streamingAssetPath + dep);
                yield return re.SendWebRequest();
                AssetBundle able = DownloadHandlerAssetBundle.GetContent(re);
                bundlesLoadedDict.Add(able.name, able);
            }

            AssetBundle assetBundle;
            if (bundlesLoadedDict.ContainsKey(item) && bundlesLoadedDict[item] != null)
                assetBundle = bundlesLoadedDict[item];
            else
            {
                UnityWebRequest itemRequest = UnityWebRequestAssetBundle.GetAssetBundle(streamingAssetPath + item);
                yield return itemRequest.SendWebRequest();
                assetBundle = DownloadHandlerAssetBundle.GetContent(itemRequest);
                bundlesLoadedDict.Add(assetBundle.name, assetBundle);
            }

            Object[] objs = assetBundle.LoadAllAssets();  //加载过程

            foreach (Object obj in objs)
            {
                if (!allObjs.ContainsKey(obj.name))
                {
                    allObjs.Add(obj.name, obj);
                   // Debug.Log(obj.name);
                }
            }      
        }
        //释放
        ab.Unload(true);
        ab = null;
    }
    //强制卸载所有资源
    public void ForceUnLoadAll()
    {
        if (bundlesLoadedDict.Count == 0)
            return;
        string[] keys = bundlesLoadedDict.Keys.ToArray();
        for (int i = 0; i < keys.Length; i++)
        {
            bundlesLoadedDict[keys[i]].Unload (true);
            bundlesLoadedDict[keys[i]] = null;
        }

        string[] objKeys = allObjs.Keys.ToArray();
        for (int i = 0; i < objKeys.Length; i++)
        {
            allObjs[objKeys[i]] = null;
        }
        allObjs.Clear();
    }
    //弱卸载，释放AssetBundle本身的内存
    public void UnLoadAll()
    {
        if (bundlesLoadedDict.Count == 0)
            return;
        string[] keys = bundlesLoadedDict.Keys.ToArray();
        for (int i = 0; i < keys.Length; i++)
        {
            bundlesLoadedDict[keys[i]].Unload(false);
            bundlesLoadedDict[keys[i]] = null;
        }
        foreach (KeyValuePair<string, Object> obj in allObjs)
        {
            allObjs[obj.Key] = null;
        }
        allObjs.Clear();
    }

    // 全局弱卸载，回收无引用Asset
    private void UnloadUnusedAssets()
    {
        Resources.UnloadUnusedAssets();
    }


    public Object GetObject(string key)
    {
        if (allObjs.ContainsKey(key))
        {
            return  allObjs[key] ;
        }
        return null;
    }
}
