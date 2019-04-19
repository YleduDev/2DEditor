using Newtonsoft.Json;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
namespace TDE
{
    public class TSceneData
    {
        #region ����

        //�洢����ͼƬ�Ļ���
        public  Dictionary<string, string> textrueDict = new Dictionary<string, string>();

        //��Ҫ���textrue data�Ƿ���ͼƬ���ü����ø���
        [JsonIgnore]
        public Dictionary<string, int> textrueReferenceDict;

        public static JsonSerializerSettings seting = new JsonSerializerSettings
      {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            TypeNameHandling = TypeNameHandling.All
      };

        public FloatReactiveProperty canvasWidth = new FloatReactiveProperty(1651);
        public FloatReactiveProperty canvasHeight = new FloatReactiveProperty(1444);

        public ReactiveCollection<T_Line> LineDataList = new ReactiveCollection<T_Line>();

        public ReactiveCollection<T_Image> ImageDataList = new ReactiveCollection<T_Image>();

        public ReactiveCollection<T_Text> TextDataList = new ReactiveCollection<T_Text>();

        #endregion

        #region �����Լ��ϵĲ�������
        public bool Add(T_Graphic model)
        {
            if (model.graphicType == GraphicType.Image)
            {
                ImageDataList.Add(model as T_Image); return true;
            }
            if (model.graphicType == GraphicType.Line)
            {
                LineDataList.Add(model as T_Line); return true;
            }
            if (model.graphicType == GraphicType.Text)
            {
                TextDataList.Add(model as T_Text); return true;
            }
            return false;
        }

        public void Remove(T_Graphic model)
        {
            switch (model.graphicType)
            {
                case GraphicType.Image:
                    Remove(model as T_Image);
                    break;
                case GraphicType.Text:
                    Remove(model as T_Text);
                    break;
                case GraphicType.Line:
                    Remove(model as T_Line);
                    break;
            }
        }

        public void Remove(T_Line model)
        {
            LineDataList.Remove(model);
        }
        public void Remove(T_Image model)
        {
            if (ImageDataList.Remove(model)&&textrueReferenceDict.IsNotNull()&&textrueReferenceDict.ContainsKey(model.spritrsStr.Value))         
                textrueReferenceDict[model.spritrsStr.Value] -= 1;
        }
        public void Remove(T_Text model)
        {
            if (TextDataList.Remove(model) && textrueReferenceDict.IsNotNull()&&textrueReferenceDict.ContainsKey(model.spritrsStr.Value))
                textrueReferenceDict[model.spritrsStr.Value] -= 1;                    
        }

        #endregion
         

        public static TSceneData Load(string json)
        {
            if (json.IsNullOrEmpty()) return new TSceneData();
            else
            {
                TSceneData data= JsonConvert.DeserializeObject<TSceneData>(json, seting);
                //��ʼ���Ǵ���
                data.TextrueReferenceDictInit();
                //��ʼ��������
                data.InitGlobalBindDataDict();
                
                return data;
            }
        }     

        public string Save()
        {        
            //���û�����õ�ͼƬ data
            if(textrueReferenceDict!=null&& textrueReferenceDict.Count > 0)
            {
                textrueReferenceDict.ForEach(item => { if (item.Value<=0) textrueDict.Remove(item.Key); });
            }
           return  JsonConvert.SerializeObject(this, Formatting.Indented, seting);
        }

        //��ӷ���
        public void AddTextrueData(string key,string value)
        {
            if (textrueDict.IsNotNull() && !textrueDict.ContainsKey(key))
            {
                textrueDict.Add(key, value);
                if(textrueReferenceDict==null) textrueReferenceDict= new Dictionary<string, int>();
                textrueReferenceDict.Add(key, 0);
            }
        }

        //������ݳ�ʼ��
        private void InitGlobalBindDataDict()
        {
            foreach (var item in LineDataList)
            {
                if (item.AssetNodeData != null && item.AssetNodeData.Value != null)
                    Global.AddBindData(item.AssetNodeData);
            }
            foreach (var item in ImageDataList)
            {
                if (item.AssetNodeData != null && item.AssetNodeData.Value != null)
                 Global.AddBindData(item.AssetNodeData);
            }
            foreach (var item in TextDataList)
            {
                if (item.AssetNodeData != null && item.AssetNodeData.Value != null)
                 Global.AddBindData(item.AssetNodeData);
            }
        }

        //��ʼ���Ǵ���
        private void TextrueReferenceDictInit()
        {
            if(textrueDict!=null&& textrueDict.Count > 0)
            {
                textrueReferenceDict = new Dictionary<string, int>();
                textrueDict.ForEach(item => textrueReferenceDict.Add(item.Key, 0));
            }
        }
    }

}