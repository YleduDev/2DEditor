using UnityEngine;

using System.Collections;

using System.Collections.Generic;

using System.Text;

using System.IO;

using QFramework;

/**
 
 * ��Դ��������
 
 * 1.��鱾��Application.persistentDataPathĿ¼���Ƿ���bundle_list�ļ�
 
 * 2.���û�У����ResourcesĿ¼�ж�ȡbundle_list�ļ�
 
 * 3.�ӷ�����������bundle_list�ļ����жϰ汾�Ƿ�һ�£����һ�¾Ͳ��ø���
 
 * 4.�汾��һ�£���Ҫ���£�����
 
 * 5.�����µ�bundle_list����Application.persistentDataPathĿ¼��
 
 **/
public class BundleUpdate : MonoBehaviour
{
    private static readonly string VERSION_FILE = "bundle_list";
    private string SERVER_RES_URL = "";
    private string LOCAL_RES_URL = "";
    private string LOCAL_RES_PATH = "";
    /// <summary>
    /// ���ذ汾json����
    /// </summary>
    private JsonData jdLocalFile;
    /// <summary>
    /// ����˰汾json����
    /// </summary>
    private JsonData jdServerFile;
    /// <summary>
    /// ������Դ����·���ֵ�
    /// </summary>
    private Dictionary<string, string> LocalBundleVersion;
    /// <summary>
    /// ��������Դ����·���ֵ�
    /// </summary>
    private Dictionary<string, string> ServerBundleVersion;
    /// <summary>
    /// ��Ҫ���ص��ļ�List
    /// </summary>
    private List<string> NeedDownFiles;
    /// <summary>
    /// �Ƿ���Ҫ���±��ذ汾�ļ�
    /// </summary>
    private bool NeedUpdateLocalVersionFile = false;
    /// <summary>
    /// �������ί��
    /// </summary>
    /// <param name="www"></param>
    public delegate void HandleFinishDownload(WWW www);
    /// <summary>
    /// ����һ����Ҫ���µ���Դ��
    /// </summary>
    int totalUpdateFileCount = 0;
    void Start()
    {
#if UNITY_EDITOR && UNITY_ANDROID
 
        SERVER_RES_URL = "file:///" + Application.streamingAssetsPath + "/android/";
 
        LOCAL_RES_URL = "file:///" + Application.persistentDataPath + "/res/";
 
        LOCAL_RES_PATH = Application.persistentDataPath + "/res/";
 
#elif UNITY_EDITOR && UNITY_IOS
 
        SERVER_RES_URL = "file://" + Application.streamingAssetsPath + "/ios/";
 
        LOCAL_RES_URL =  "file:///" + Application.persistentDataPath + "/res/";
 
        LOCAL_RES_PATH =  Application.persistentDataPath + "/res/";
 
#elif UNITY_ANDROID
 
        //��׿����Ҫʹ��www����StreamingAssets����ļ�,Streaming AssetsĿ¼�ڰ�׿�µ�·��Ϊ "jar:file://" + Application.dataPath + "!/assets/"
 
        SERVER_RES_URL =  "jar:file://" + Application.dataPath + "!/assets/" + "android/";
 
        LOCAL_RES_URL =  "jar:file://" + Application.persistentDataPath + "!/assets/" + "/res/";
 
        //LOCAL_RES_URL =  "file://" + Application.persistentDataPath + "/res/";
 
        LOCAL_RES_PATH =  Application.persistentDataPath + "/res/";
 
#elif UNITY_IOS
 
        SERVER_RES_URL = "http://127.0.0.1/resource/ios/"
 
        LOCAL_RES_URL =  "file:///" + Application.persistentDataPath + "/res/";
 
        LOCAL_RES_PATH =  Application.persistentDataPath + "/res/";
#endif
       //��ʼ��    
        LocalBundleVersion = new Dictionary<string, string>();
        ServerBundleVersion = new Dictionary<string, string>();
        NeedDownFiles = new List<string>();
        //���ر���version����    
        string tmpLocalVersion = "";
        if (!File.Exists(LOCAL_RES_PATH + VERSION_FILE))
        {
            TextAsset text = Resources.Load(VERSION_FILE) as TextAsset;
            tmpLocalVersion = text.text;
        }
        else
        {
            tmpLocalVersion = File.ReadAllText(LOCAL_RES_PATH + VERSION_FILE);
        }
        //���汾�ص�version       
        ParseVersionFile(tmpLocalVersion, LocalBundleVersion, 0);
        //���ط����version����    
        StartCoroutine(this.DownLoad(SERVER_RES_URL + VERSION_FILE, delegate (WWW serverVersion)
        {
            //��������version    
            ParseVersionFile(serverVersion.text, ServerBundleVersion, 1);
            //�������Ҫ���¼��ص���Դ    
            CompareVersion();
            //������Ҫ���µ���Դ    
            DownLoadRes();
        }));
    }
    //���μ�����Ҫ���µ���Դ    
    private void DownLoadRes()
    {
        if (NeedDownFiles.Count == 0)
        {
            UpdateLocalVersionFile();
            return;
        }
        string file = NeedDownFiles[0];
        NeedDownFiles.RemoveAt(0);
        StartCoroutine(this.DownLoad(SERVER_RES_URL + file, delegate (WWW w)
        {
            //�����ص���Դ�滻���ؾ͵���Դ    
            ReplaceLocalRes(file, w.bytes);
            DownLoadRes();
        }));
    }
    private void ReplaceLocalRes(string fileName, byte[] data)
    {
        try
        {
            string filePath = LOCAL_RES_PATH + fileName;
            if (!File.Exists(filePath))
            {
                string p = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(p))
                    Directory.CreateDirectory(p);
            }
            File.WriteAllBytes(filePath, data);
        }
        catch (System.Exception e)
        {
            Debug.Log("e is " + e.Message);
        }
    }
    //���±��ص�version����    
    private void UpdateLocalVersionFile()
    {
        if (NeedUpdateLocalVersionFile)
        {
            if (!Directory.Exists(LOCAL_RES_PATH))
                Directory.CreateDirectory(LOCAL_RES_PATH);
            StringBuilder versions = new StringBuilder(jdServerFile.ToJson());
            FileStream stream = new FileStream(LOCAL_RES_PATH + VERSION_FILE, FileMode.Create);
            byte[] data = Encoding.UTF8.GetBytes(versions.ToString());
            stream.Write(data, 0, data.Length);
            stream.Flush();
            stream.Close();
        }
    }
    private void CompareVersion()
    {
        int localVersionId;
        int serverVersionId;
        if (jdLocalFile != null && !jdLocalFile.id.IsNullOrEmpty())
            localVersionId = int.Parse(jdLocalFile.id);
        if (jdServerFile != null && jdServerFile.id.IsNullOrEmpty())
            serverVersionId = int.Parse(jdServerFile.id);
#if UNITY_ANDROID || UNITY_EDITOR
        NeedDownFiles.Add("android");
#endif
#if UNITY_IOS
#endif
        foreach (var version in ServerBundleVersion)
        {
            string fileName = version.Key;
            string serverHash = version.Value;
            //��������Դ    
            if (!LocalBundleVersion.ContainsKey(fileName))
            {
                NeedDownFiles.Add(fileName);
            }
            else
            {
                //��Ҫ�滻����Դ    
                string localHash;
                LocalBundleVersion.TryGetValue(fileName, out localHash);
                if (!serverHash.Equals(localHash))
                {
                    NeedDownFiles.Add(fileName);
                }
            }
        }
        totalUpdateFileCount = NeedDownFiles.Count;
        //�����и��£�ͬʱ���±��ص�version.txt    
        NeedUpdateLocalVersionFile = NeedDownFiles.Count > 0;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="content"></param>
    /// <param name="dict"></param>
    /// <param name="flag">0��ʾ���ذ汾�ļ���1��ʾ�������汾�ļ�</param>
    private void ParseVersionFile(string content, Dictionary<string, string> dict, int flag)
    {
        if (content == null || content.Length == 0)
        {
            return;
        }
        JsonData jd = null;
        try
        {
            jd = SerializeHelper.FromJson<JsonData>(content);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            return;
        }
        if (flag == 0)//����
        {
            jdLocalFile = jd;
        }
        else if (flag == 1)//������
        {
            jdServerFile = jd;
        }
        else
            return;
        //��ȡ��Դ����
       List<AssetPathJsonData> resObjs = null;
        if (jd.resource.IsNotNull()&& jd.resource.Count>0)
            resObjs = jd.resource;
       if (resObjs.IsNotNull())
        {
            for (int i = 0; i < resObjs.Count; i++)
            {
                    dict.Add(resObjs[i].name, resObjs[i].path);
            }
        }
    }
    private IEnumerator DownLoad(string url, HandleFinishDownload finishFun)
    {
        WWW www = new WWW(url);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError("www.error is " + www.error);
            yield break;
        }
        if (finishFun != null)
        {
            finishFun(www);
        }
        www.Dispose();
    }
}
public  class JsonData
{
    public string id;
    public List<AssetPathJsonData> resource;
} 
public class AssetPathJsonData
{
   public string name;
   public string path;
}