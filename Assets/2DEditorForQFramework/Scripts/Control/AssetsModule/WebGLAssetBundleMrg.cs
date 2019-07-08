using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.Networking;
using System.Linq;

public class WebGLAssetBundleMrg : Singleton<WebGLAssetBundleMrg> {

    private WebGLAssetBundleMrg() { }
    private Dictionary<string, AssetBundle> bundlesLoadedDict = new Dictionary<string,AssetBundle>(); //�����ص���Դ�б�
    string streamingAssetPath;

    private Dictionary<string, Object> allObjs = new Dictionary<string, Object>();  //����Ԥ�����ֵ�


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
            //��������
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

            Object[] objs = assetBundle.LoadAllAssets();  //���ع���

            foreach (Object obj in objs)
            {
                if (!allObjs.ContainsKey(obj.name))
                {
                    allObjs.Add(obj.name, obj);
                   // Debug.Log(obj.name);
                }
            }      
        }
        //�ͷ�
        ab.Unload(true);
        ab = null;
    }
    //ǿ��ж��������Դ
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
    //��ж�أ��ͷ�AssetBundle������ڴ�
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

    // ȫ����ж�أ�����������Asset
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
