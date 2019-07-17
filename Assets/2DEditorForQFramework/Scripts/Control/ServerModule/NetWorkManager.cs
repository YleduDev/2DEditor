using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
#if UNITY_WEBGL
#else
using System.Net;
using System.IO;
#endif
using System.Text;
using System.Reflection;

namespace XZL.Network
{
    /// <summary>
    /// 网络控制
    /// </summary>
    public class NetWorkManager
    {
        #region Single
        //一些初始化可以放着 也可以方到构造函数
        public void OnSingletonInit()
        {
            cookie = "";
        }
        //构造函数
        private NetWorkManager()
        {
        }

        private static NetWorkManager mInstance;
        private static readonly object mLock = new object();
        public static NetWorkManager Instance
        {
            get
            {
                lock (mLock)
                {
                    if (mInstance == null)
                    {
                        mInstance = CreateSingleton();
                    }
                }
                return mInstance;
            }
        }
        //释放Network
        public static void Dispose()
        {
            mInstance = null;
        }
        private static NetWorkManager CreateSingleton()
        {
            // 获取私有构造函数T
            var ctors = typeof(NetWorkManager).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);

            // 获取无参构造函数
            var ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);

            if (ctor == null)
            {
                throw new Exception("Non-Public Constructor() not found! in " + typeof(NetWorkManager));
            }

            // 通过构造函数，常见实例
            var retInstance = ctor.Invoke(null) as NetWorkManager;
            retInstance.OnSingletonInit();

            return retInstance;
        }
        #endregion

        private string contentType = "application/x-www-form-urlencoded";
        private int timeout = 5;

        public void SetTimeout(int value)
        {
            timeout = value;
        }

        public void SetContentType(string value)
        {
            contentType = value;
        }
#if UNITY_WEBGL
#else
        #region http
        public string userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
        public string accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";

        private CookieContainer cookieContainer = new CookieContainer();

        
        public void SetUserAgentInHttp(string userAgent)
        {
            this.userAgent = userAgent;
        }
        public void SetAcceptcInHttp(string accept)
        {
            this.accept = accept;
        }

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
        public void CreateHTTPRequest(string url, Action<string> sucAct = null, Action<string> loseAct = null)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = WebRequest.Create(url) as HttpWebRequest;
                //域
                request.CookieContainer = cookieContainer;
                //方法
                request.Method = "GET";
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

                sucAct?.Invoke(res);
            }
            catch (Exception e)
            {
               // DevelopEngine.Console.Log("CreateHTTPRequest Error url is:" + url + "   errorMessage:" + e.ToString());
                if (response != null) response.Close();
                if (request != null) request.Abort();
                loseAct?.Invoke(e.Message);
            }
        }
        /// <summary>
        /// 创建Http请求Post
        /// </summary>
        /// <param name="pathData">请求data</param>
        /// <param name="url"></param>
        /// <returns></returns>
        public void CreateHTTPRequest(string pathData, string url, Action<string> sucAct = null, Action<string> loseAct = null)
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
                request.Method = "POST";
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

                sucAct?.Invoke(res); 
            }
            catch (Exception e)
            {
                //DevelopEngine.Console.Log(e.ToString());
                //DevelopEngine.Console.Log("CreateHTTPRequest Error url is:" + url + "  parameter:" + pathData + "    errorMessage:" + e.Message);
                if (response != null) response.Close();
                if (request != null) request.Abort();
                loseAct?.Invoke(e.Message);
            }
        }
        /// <summary>
        ///  登陆并保存Cookie
        /// </summary>
        /// <param name="pathData"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public void PostLogin(string Id, string passWord, string url, Action<string> sucAct=null, Action<string> loseAct=null)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                string pathData = "login_id="+Id+"&password="+passWord;
                CookieContainer tempCookie = new CookieContainer();

                byte[] data = Encoding.UTF8.GetBytes(pathData);
                //向服务端发送请求
                request = WebRequest.Create(url) as HttpWebRequest;
                //设置属性
                request.Method = "POST";
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

                sucAct?.Invoke(res);
            }
            catch (Exception e)
            {
              //  DevelopEngine.Console.Log("PostLogin Error url is:" + url + "  parameter:" + Id+"Password"+passWord + "    errorMessage:" + e.ToString());
                if (response != null) response.Close();
                if (request != null) request.Abort();
                loseAct?.Invoke(e.Message);
            }
        }

        #endregion
#endif
        #region unityWebRequset
        string cookie;
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="url">登陆url</param>
        /// <param name="act">登陆成功后回调</param>
        /// <param name="loseAct">登陆失败后回调</param>
        /// <returns></returns>
        public IEnumerator IELogin(string url, string Id, string passWord, Action act = null, Action loseAct = null)
        {

            using (UnityWebRequest www = new UnityWebRequest())
            {
                www.url = url;
                www.method = UnityWebRequest.kHttpVerbPOST;

                string pathData = "login_id=" + Id + "&password=" + passWord;
                byte[] bodyRaw = Encoding.UTF8.GetBytes(pathData);
                www.uploadHandler = new UploadHandlerRaw(bodyRaw);
                www.SetRequestHeader("Content-Type", contentType);
                www.downloadHandler = new DownloadHandlerBuffer();
                www.timeout = timeout;
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    //DevelopEngine.Console.Log(www.error);
                    loseAct?.Invoke();
                }
                else
                {
                    cookie = www.GetResponseHeader("Set-Cookie");

                    //DevelopEngine.Console.Log(www.downloadHandler.text);
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
        /// <param name="loseRe">失败后委托</param>
        /// <returns></returns>
        public IEnumerator IEGet<T>(string url, Action<T> re = null, Action<T> loseRe = null)
        {
            using (UnityWebRequest www = new UnityWebRequest())
            {
                www.url = url;
                www.method = UnityWebRequest.kHttpVerbGET;
#if UNITY_EDITOR
                // Log.I(cookie);
                www.SetRequestHeader("cookie", cookie);

#elif UNITY_WEBGL
#else
                www.SetRequestHeader("cookie", string.Format("{0}", cookie));
#endif
                www.SetRequestHeader("Content-Type", contentType);
                www.downloadHandler = new DownloadHandlerBuffer();
                www.timeout = timeout;
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    //DevelopEngine.Console.Log(url);
                    //DevelopEngine.Console.Log(www.error);
                    loseRe?.Invoke((T)(object)www.responseCode);
                }
                else
                {
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
        /// <param name="loseRe">失败后委托</param>
        /// <returns></returns>
        public IEnumerator IEPost<T>(string url, WWWForm data, Action<T> re = null, Action<T> loseRe = null)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(url, data))
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

                www.SetRequestHeader("Content-Type", contentType);
                www.timeout = timeout;

                www.downloadHandler = new DownloadHandlerBuffer();

                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    //DevelopEngine.Console.Log(www.url);
                    //DevelopEngine.Console.Log(www.error);
                    loseRe?.Invoke((T)(object)www.responseCode);
                }
                else
                {
                    //DevelopEngine.Console.Log(www.downloadHandler.text);
                    re?.Invoke((T)(object)www.downloadHandler.text);
                }
            }
        }

        public IEnumerator IEPost<T>(string url, string data, Action<T> re = null, Action<T> loseRe = null)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(url, data))
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

                www.SetRequestHeader("Content-Type", contentType);
                www.timeout = timeout;

                www.downloadHandler = new DownloadHandlerBuffer();

                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    //DevelopEngine.Console.Log(www.url);
                    //DevelopEngine.Console.Log(www.error);
                    loseRe?.Invoke((T)(object)www.responseCode);
                }
                else
                {
                    //DevelopEngine.Console.Log(www.downloadHandler.text);
                    re?.Invoke((T)(object)www.downloadHandler.text);
                }
            }
        }

        #endregion
    }
}
