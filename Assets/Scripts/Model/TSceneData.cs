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
        public FloatReactiveProperty canvasWidth = new FloatReactiveProperty(1651);
        public FloatReactiveProperty canvasHeight = new FloatReactiveProperty(1444);

        public ReactiveCollection<T_Line> LineDataList = new ReactiveCollection<T_Line>();

        public ReactiveCollection<T_Image> ImageDataList = new ReactiveCollection<T_Image>();

        public ReactiveCollection<T_Text> TextDataList = new ReactiveCollection<T_Text>();

        public void Add(T_Graphic model)
        {
            if (model.graphicType == GraphicType.Image)
                ImageDataList.Add(model as T_Image);
            if (model.graphicType == GraphicType.Line)
                LineDataList.Add(model as T_Line);
            if (model.graphicType == GraphicType.Text)
                TextDataList.Add(model as T_Text);
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
            ImageDataList.Remove(model);
        }
        public void Remove(T_Text model)
        {
            TextDataList.Remove(model);
        }
        public static TSceneData Load()
        {
            //
            JsonSerializerSettings seting = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                TypeNameHandling = TypeNameHandling.All
            };
            string json = PlayerPrefs.GetString("Test19", string.Empty);
            if (json.IsNullOrEmpty()) return new TSceneData();
            else
            {
                return JsonConvert.DeserializeObject<TSceneData>(json, seting);
            }
        }

        public void Save()
        {
            JsonSerializerSettings seting = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                TypeNameHandling = TypeNameHandling.All
            };
            PlayerPrefs.SetString("Test19", JsonConvert.SerializeObject(this, Formatting.Indented, seting));
        }

        public void InitGlobalBindDataDict()
        {
            foreach (var item in LineDataList)
            {
                if (item.AssetNodeData != null && item.AssetNodeData.Value != null)
                    //item.AssetNodeData.Value = null;
                    Global.AddBindData(item.AssetNodeData);
            }
            foreach (var item in ImageDataList)
            {
                if (item.AssetNodeData != null && item.AssetNodeData.Value != null)
                   // item.AssetNodeData.Value = null;
                 Global.AddBindData(item.AssetNodeData);
            }
            foreach (var item in TextDataList)
            {
                if (item.AssetNodeData != null && item.AssetNodeData.Value != null)
                   // item.AssetNodeData.Value = null;
                 Global.AddBindData(item.AssetNodeData);
            }
        }
    }
}