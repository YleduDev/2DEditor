using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Text;
using QFramework;
namespace XZL
{
    public class HTTPMgr : MonoSingleton<HTTPMgr>
    {
        #region 请求
        public static string contentType = "application/x-www-form-urlencoded";
        public static string method = "POST";
        public static int timeout = 300;

        public static string userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";

        public static string accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
   
        public CookieContainer cookie = new CookieContainer();

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
        /// 创建Http请求 非异步
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
                request.CookieContainer = cookie;
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
                request.CookieContainer = cookie;
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
                request.CookieContainer = cookie;
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

                cookie = tempCookie;

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


        public int PostRequest(string Url, string user, string pwd, string paramData, Encoding MsgEncode)
        {
            if (string.IsNullOrEmpty(Url))
            {
                throw new ArgumentNullException("Url");
            }
            if (MsgEncode == null)
            {
                throw new ArgumentNullException("MsgEncoding");
            }

            string username = user;
            string password = pwd;
            string usernamePassword = username + ":" + password;
            CredentialCache mycache = new CredentialCache();
            mycache.Add(new Uri(Url), "Digest", new NetworkCredential(username, password));

            Log.I(GetType().ToString(), "PostRequest", "POST HTTP请求,创建短信请求", string.Empty);
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(new Uri(Url));
            Request.Credentials = mycache;
            Request.Headers.Add("Authorization", "Digest" + Convert.ToBase64String(MsgEncode.GetBytes(usernamePassword)));

            Request.Method = "POST";
            //Request.Timeout = 1000;
            Request.ContentType = "application/x-www-form-urlencoded";

            
            byte[] byteArray = MsgEncode.GetBytes(paramData);
            Request.ContentLength = byteArray.Length;

            Stream newStream = Request.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();

            //string ret = string.Empty;
            Log.I(GetType().ToString(), "PostRequest", "POST HTTP请求,获取短信HTTP请求响应", string.Empty);
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)Request.GetResponse();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
            }

            int ret = 0;
            ret = (int)response.StatusCode;
            Log.I(GetType().ToString(), "PostRequest", "POST HTTP请求,发送短信请求返回状态码", ret.ToString());

            Stream stream = response.GetResponseStream();
            byte[] rsByte = new Byte[response.ContentLength];
            //StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
            //ret = sr.ReadToEnd();
            try
            {
                stream.Read(rsByte, 0, (int)response.ContentLength);
                Log.I(GetType().ToString(), "PostRequest", "POST HTTP请求,发送短信请求返回内容", System.Text.Encoding.UTF8.GetString(rsByte, 0, rsByte.Length).ToString());
            }
            catch (Exception ex)
            {
                Log.I(GetType().ToString(), "PostRequest", "POST HTTP请求,发送短信请求返回内容异常", ex.ToString());
            }
            stream.Close();
            response.Close();

            return ret;
        }

        #endregion
    }
}