using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_WEBGL
#else
#endif
using QFramework;
using TDE;
using UnityEngine;
using XZL.Network;

namespace XZL
{
    public class HTTPMgr : MonoSingleton<HTTPMgr>
    {

        public void Login(string url, Action act = null, Action loseAct = null)
        {
            StartCoroutine(XZL.Network.NetWorkManager.Instance.IELogin(url, "admin", "123456", act, loseAct));
        }
        public void FindAllScene(string url, Action<List<string>> re)
        {
            StartCoroutine(NetWorkManager.Instance.IEGet<string>(url,
                (str) =>
                {
                    List<string> item = null;
                    try
                    {
                        item = str.IsNullOrEmpty() ? null : SerializeHelper.FromJson<List<string>>(str);
                    }
                    catch (Exception) { }
                    re?.Invoke(item);
                }));
        }
        public void FindOneScene(string url, Action<string> re)
        {
            StartCoroutine(XZL.Network.NetWorkManager.Instance.IEGet(url, re));
        }

        public void GetAssetForId(string url, Action<string> re = null)
        {
            StartCoroutine(XZL.Network.NetWorkManager.Instance.IEGet(url, re));
        }
        public IEnumerator UpLoadSecne(string url, WWWForm data, Action<string> re)
        {

            yield return StartCoroutine(XZL.Network.NetWorkManager.Instance.IEPost<string>(url, data, 
                (str) =>
            {
                try
                {
                    re?.Invoke(str);
                }
                catch (Exception) { }
            }));
        }
        public void GetAssetIdForHttp(string url, Action<List<AssetNode>> re)
        {
            StartCoroutine(XZL.Network.NetWorkManager.Instance.IEGet<string>(url,
                str =>
                {
                    List<AssetNode> asset = null;
                    try
                    {
                        asset = str.IsNullOrEmpty() ? null : SerializeHelper.FromJson<List<AssetNode>>(str);
                    }
                    catch (Exception)
                    {

                    }
                    re?.Invoke(asset);
                }));
        }

    }
}