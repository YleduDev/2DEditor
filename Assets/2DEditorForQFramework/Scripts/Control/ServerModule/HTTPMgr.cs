using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_WEBGL
#else
using System.IO;
using System.Net;
#endif
using System.Text;
using QFramework;
using TDE;
using UnityEngine;
using UnityEngine.Networking;
namespace XZL 
{
    public class HTTPMgr : MonoSingleton<HTTPMgr>
    {
       
        public static string contentType = "application/x-www-form-urlencoded";
        public static string method = "POST";
        public static int timeout = 300;
        public static string userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
        public static string accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";    
#if UNITY_WEBGL
#else
        #region http

        public CookieContainer cookieContainer = new CookieContainer();
        /// <summary>
        /// /// 创建一个HTTP请求    异步
        /// Action<HTTPResponse> callback  
        /// 可以拓展成Lua事件 这样非常灵活 这里只是没有相应需求 就先定义委托
        /// /// </summary> 
        /// /// <param name="url">URL.</param>
        /// /// <param name="callback">Callback</param> 
        //public HTTPRequest AsyCreateHTTPRequest(string url, string method, Action<HTTPResponse> callback)
        //{
        //    HTTPRequest client = new HTTPRequest(url, method, 5000, (HTTPResponse response) =>
        //    {
        //        if (null != callback)
        //        {
        //            callback(response);
        //        }
        //    });
        //    return client;
        //}
        /// <summary>
        /// 创建Http请求 非异步 Get请求
        /// </summary>
        /// <returns></returns>
        public string CreateHTTPRequest(string url)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = WebRequest.Create(url) as HttpWebRequest;
                //域
                request.CookieContainer = cookieContainer;
                //方法
                request.Method = method;
                //内容类型
                request.ContentType = contentType;
                //超时
                request.Timeout = timeout;
                //来源
                request.Referer = url;
                //承诺？
                request.Accept = accept;
                //代理
                request.UserAgent = userAgent;

                //获取返回结果
                response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                //读取返回数据流
                StreamReader streamReader = new StreamReader(responseStream,
                    Encoding.GetEncoding("UTF-8"));
                string res = streamReader.ReadToEnd();

                //清理
                streamReader.Close();
                responseStream.Close();
                request.Abort();
                response.Close();

                return res;
            }
            catch (Exception e)
            {
                Log.I("CreateHTTPRequest Error url is:" + url + "   errorMessage:" + e.ToString());
                if (response != null) response.Close();
                if (request != null) request.Abort();
                return null;
            }
        }
        /// <summary>
        /// 创建Http请求Post
        /// </summary>
        /// <param name="pathData">请求data</param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string CreateHTTPRequest(string pathData, string url)
        {
            //Debug.Log(pathData+"  "+ url);
            

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(pathData);
                //向服务端发送请求
                request = WebRequest.Create(url) as HttpWebRequest;
                //设置属性
                request.Method = method;
                request.ContentType = contentType;
                request.ContentLength = data.Length;
                request.CookieContainer = cookieContainer;
                //发送请求
                Stream newStream = request.GetRequestStream();
                newStream.Write(data, 0, data.Length);

                //请求结果发送给客户端
                response = request.GetResponse() as HttpWebResponse;
                //读取流数据
                StreamReader reader = new StreamReader(response.GetResponseStream(),
                 Encoding.UTF8);
                string res = reader.ReadToEnd();

                //清理
                reader.Close();
                newStream.Close();
                response.Close();
                request.Abort();

                return res;
            }
            catch (Exception e)
            {
                Log.I(e);
                Log.I("CreateHTTPRequest Error url is:" + url + "  parameter:" + pathData + "    errorMessage:" + e.Message);
                if (response != null) response.Close();
                if (request != null) request.Abort();
                return null; 
            }
        }
        /// <summary>
        ///  登陆并保存Cookie
        /// </summary>
        /// <param name="pathData"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string PostLogin(string pathData, string url)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                CookieContainer tempCookie = new CookieContainer();

                byte[] data = Encoding.UTF8.GetBytes(pathData);
                //向服务端发送请求
                request = WebRequest.Create(url) as HttpWebRequest;
                //设置属性
                request.Method = method;
                request.CookieContainer = cookieContainer;
                request.ContentType = contentType;
                request.KeepAlive = true;
                request.UserAgent = userAgent;
                request.Accept = accept;
                request.ContentLength = data.Length;
                //发送请求
                Stream newStream = request.GetRequestStream();
                newStream.Write(data, 0, data.Length);

                //请求结果发送给客户端
                response = request.GetResponse() as HttpWebResponse;
                tempCookie.Add(response.Cookies);
                //读取流数据
                StreamReader reader = new StreamReader(response.GetResponseStream(),
                 Encoding.UTF8);
                string res = reader.ReadToEnd();

                //清理
                reader.Close();
                newStream.Close();
                response.Close();
                request.Abort();

                cookieContainer = tempCookie;

                return res;
            }
            catch (Exception e)
            {
                Log.I("PostLogin Error url is:" + url + "  parameter:" + pathData + "    errorMessage:" + e.ToString());
                if (response != null) response.Close();
                if (request != null) request.Abort(); 
                return null;
            }
        }

        #endregion
#endif
        #region unityWebRequset
        string cookie;
        public override void OnSingletonInit()
        {
            cookie = "";
        }
        public void Login(string url, Action act=null, Action loseAct=null)
        {
            StartCoroutine(IELogin(url, act, loseAct));
        }
        public void FindAllScene(string url, Action<List<string>> re)
        {
            StartCoroutine(IEGet<string>(url, (str)=> {
                List<string> item = null;
                try
                {
                    item = str.IsNullOrEmpty() ? null : SerializeHelper.FromJson<List<string>>(str);
                }
                catch (Exception){}
                re?.Invoke(item);
            }));
        }
        public void FindOneScene(string url, Action<string> re)
        {
            StartCoroutine(IEGet(url, re));
        }

        public void GetAssetForId(string url, Action<string> re=null)
        {
            StartCoroutine(IEGet(url, re));
        }

        public void GetAssetIdForHttp(string url, Action<List<AssetNode>> re)
        {
            StartCoroutine(IEGet<string>(url, str=> {
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

        //二维编辑器上传接口 服务端返回的数值
        [System.Serializable]
        public class TwdMessage
        {
            public string success;
            public string message;
        }
        public IEnumerator UpLoadSecne(string url, WWWForm data, Action<string> re)
        {

          yield return  StartCoroutine(IEPost<string>(url, data,(str)=> {
                TwdMessage meg = null;
                try
                {
                    meg = SerializeHelper.FromJson<TwdMessage>(str);
                    re?.Invoke(str);
                }
                catch (Exception){}
            }));
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="url">登陆url</param>
        /// <param name="act">登陆成功后回调</param>
        /// <param name="loseAct">登陆失败后回调</param>
        /// <returns></returns>
      public IEnumerator IELogin(string url, Action act, Action loseAct)
        {

            using (UnityWebRequest www = new UnityWebRequest())
            {
                www.url = url;
                www.method = UnityWebRequest.kHttpVerbPOST;
#if UNITY_EDITOR
                // www.SetRequestHeader("cookie",  cookie);
#elif UNITY_WEBGL
#else
                www.SetRequestHeader("cookie", string.Format("{0}", cookie));
#endif
                byte[] bodyRaw = Encoding.UTF8.GetBytes("login_id=admin&password=123456");
                www.uploadHandler = new UploadHandlerRaw(bodyRaw);
                www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                www.downloadHandler = new DownloadHandlerBuffer();
                www.timeout = 5;
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Log.I(www.error);
                    loseAct?.Invoke();
                }
                else
                {
                    cookie = www.GetResponseHeader("Set-Cookie");

                    Log.I(cookie);
                    act?.Invoke();
                    // Or retrieve results as binary data
                    //byte[] results = www.downloadHandler.data;
                }
            }
        }
        /// <summary>
        /// GET访问
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="re">获取成功后委托</param>
        /// <returns></returns>
        IEnumerator IEGet<T>(string url, Action<T> re)
        {
            using (UnityWebRequest www = new UnityWebRequest())
            {
                www.url = url;
                www.method = UnityWebRequest.kHttpVerbGET;
#if UNITY_EDITOR
               // Log.I(cookie);
                www.SetRequestHeader("cookie",  cookie);

#elif UNITY_WEBGL
#else
                www.SetRequestHeader("cookie", string.Format("{0}", cookie));
#endif
                www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                www.downloadHandler = new DownloadHandlerBuffer();
               // www.timeout = 300;
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Log.I(url);
                    Log.I(www.error);
                }
                else
                {
                    //Log.I(url);
                    //Log.I(www.downloadHandler.text);
                    //File.WriteAllText(FilePath.StreamingAssetsPath+"qqt.txt", www.downloadHandler.text                   
                    //  Log.I(www.downloadHandler.text.Substring(0,1));
                    //string tex =Global.GetUTF8( www.downloadHandler.text);
                    re?.Invoke((T)(object)www.downloadHandler.text);
                }
            }
        }
        /// <summary>
        /// post访问
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="data">发送data</param>
        /// <param name="re"> 获取成功后委托</param>
        /// <returns></returns>
        IEnumerator IEPost<T>(string url,WWWForm data, Action<T> re)
        {
            using (UnityWebRequest www =UnityWebRequest.Post(url, data))
            {
               // www.method = UnityWebRequest.kHttpVerbPOST;
                
#if UNITY_EDITOR
                www.SetRequestHeader("cookie", cookie);
#elif UNITY_WEBGL
#else
                www.SetRequestHeader("cookie", string.Format("{0}", cookie));
#endif
                //byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
                //www.uploadHandler = new UploadHandlerRaw(bodyRaw);              
                
                www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
               // www.timeout = 300;
                
                www.downloadHandler = new DownloadHandlerBuffer();
                
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Log.I(www.url);
                    Log.I(data);
                    Log.I(www.error);
                    Log.I(www.responseCode);
                }
                else
                {
                    Log.I(www.downloadHandler.text);
                    re?.Invoke((T)(object)www.downloadHandler.text);
                    
                }
            }
        }

#endregion
    }

   
}