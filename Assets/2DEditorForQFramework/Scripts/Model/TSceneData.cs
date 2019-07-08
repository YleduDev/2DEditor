using Newtonsoft.Json;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using System.Text;
namespace TDE
{
    /// <summary>
    /// 2d编辑器场景对象基类
    /// </summary>
    public class TSceneData
    {
        #region 数据

        //textrue缓存
        public Dictionary<string, string> textrueDict = new Dictionary<string, string>();
        public string Name;
        //textrue data缓存参照
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
        //控件
        public ReactiveCollection<T_Widget> WidgetDataList = new ReactiveCollection<T_Widget>();
         
        #endregion

        #region 对外提供图元组集合方法
        public bool Add(T_Graphic model)
        {
            if (model.graphicType == GraphicType.Image){
                ImageDataList.Add(model as T_Image); return true;
            }
            else if (model.graphicType == GraphicType.Line){
                LineDataList.Add(model as T_Line); return true;
            }
            else if (model.graphicType == GraphicType.Text){
                TextDataList.Add(model as T_Text); return true;
            }
            else if(model.graphicType == GraphicType.Widget) {
                    WidgetDataList.Add(model as T_Widget); return true;
            }
                return false;
        }

        public void Remove(T_Graphic model)
        {
            switch (model.graphicType)
            {
                case GraphicType.Image:
                    if (ImageDataList.Remove(model as T_Image) && textrueReferenceDict.IsNotNull() && textrueReferenceDict.ContainsKey(model.spritrsStr.Value))
                        textrueReferenceDict[model.spritrsStr.Value] -= 1;break;
                case GraphicType.Text:
                    if (TextDataList.Remove(model as T_Text) && textrueReferenceDict.IsNotNull() && textrueReferenceDict.ContainsKey(model.spritrsStr.Value))
                        textrueReferenceDict[model.spritrsStr.Value] -= 1;break;
                case GraphicType.Line:
                    LineDataList.Remove(model as T_Line);break;
                case GraphicType.Widget:
                    if (model.IsNotNull())WidgetDataList.Remove(model as T_Widget);break;
            }
        }
        #endregion
         

        public static TSceneData Load(string json)
        {
            if (json.IsNullOrEmpty()) return new TSceneData();
            else
            {
                try
                {
                    Global.OnClick(null);
                     TSceneData data = JsonConvert.DeserializeObject<TSceneData>(json, seting);
                    //缓存参照字典 初始化
                    data.TextrueReferenceDictInit();
                    //绑点数据初始化
                    data.InitGlobalBindDataDict();                   
                    return data;
                }
                catch (System.Exception e)
                {
                    //Log.I("反序列化有误 错误信息：" + e.Message);
                    return new TSceneData();
                }             
            }
        }     
        public string Save()
        {        
            //去除 场景中图元数据没有引用的数据
            if(textrueReferenceDict!=null&& textrueReferenceDict.Count > 0)
            {
                textrueReferenceDict.ForEach(item => { if (item.Value<=0) textrueDict.Remove(item.Key); });
            }
            string data = JsonConvert.SerializeObject(this, Formatting.Indented, seting);
            //Log.I(data);
            return data;
        } 
        //添加 图元数据
        public void AddTextrueData(string key,string value)
        {
            if (textrueDict.IsNotNull() && !textrueDict.ContainsKey(key))
            {
                textrueDict.Add(key, value);
                if(textrueReferenceDict==null) textrueReferenceDict= new Dictionary<string, int>();
                textrueReferenceDict.Add(key, 0);
            }
        }

        //初始化绑点数据
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
            foreach (var item in WidgetDataList)
            {
                if (item.AssetNodeData != null && item.AssetNodeData.Value != null)
                    Global.AddBindData(item.AssetNodeData);
                item.BindDataEvent(da => Global.AddBindData(da));
            }
        }
        //TextrueReference字典初始化
        private void TextrueReferenceDictInit()
        {
            if(textrueDict!=null&& textrueDict.Count > 0)
            {
                textrueReferenceDict = new Dictionary<string, int>();
                textrueDict.ForEach(item => textrueReferenceDict.Add(item.Key, 0));
            }
        }
        #region slef
        //触发器
        public void OnTrriger()
        {
            //触发
            LineDataList.Where(item => item.IsNotNull()).ForEach(item => item.trigger.Value = true);
            ImageDataList.Where(item => item.IsNotNull()).ForEach(item => item.trigger.Value = true);
            TextDataList.Where(item => item.IsNotNull()).ForEach(item => item.trigger.Value = true);

            //返回未触发状态
            LineDataList.Where(item => item.IsNotNull()).ForEach(item => item.trigger.Value = false);
            ImageDataList.Where(item => item.IsNotNull()).ForEach(item => item.trigger.Value = false);
            TextDataList.Where(item => item.IsNotNull()).ForEach(item => item.trigger.Value = false);
        }

        #endregion
    }

}