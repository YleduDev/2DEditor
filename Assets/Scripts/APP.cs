using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using QFramework.TDE;
using UniRx;
using System;
using Newtonsoft.Json;

namespace TDE
{
    /// <summary>
    /// 2d编辑入口
    /// </summary>
    public class APP : MonoBehaviour
    {
        TSceneData model;
        public void Awake()
        {
            #region test 数据
            model = TSceneData.Load();
            #endregion
            ResMgr.Init();
            UIMgr.SetResolution(1920, 1080, 0);
            UIMgr.OpenPanel<UIShowPanel>(new UIShowPanelData() { model=model});
        }

        private void OnApplicationQuit()
        {
            model.Save();
        }

    }
    //场景对象

    public class TSceneData
    {
        public ReactiveCollection<T_Line> LineDataList;
        
        public ReactiveCollection<T_Image> ImageDataList;
        
        public ReactiveCollection<T_Text> TextDataList;
        
        public void Add(T_Graphic model)
        {
            if(model.graphicType==  GraphicType.Image)
                ImageDataList.Add(model as T_Image);
            if (model.graphicType == GraphicType.Line)
                LineDataList.Add(model as T_Line);
            if (model.graphicType == GraphicType.Text)
                TextDataList.Add(model as T_Text);
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
            string json = PlayerPrefs.GetString("Test7", string.Empty);
            if (json.IsNullOrEmpty()) return new TSceneData();
            else
            {
                return JsonConvert.DeserializeObject<TSceneData>(json,seting);
            }
        }

        public  void Save()
        {
            JsonSerializerSettings seting = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                TypeNameHandling = TypeNameHandling.All
            };
            PlayerPrefs.SetString("Test7", JsonConvert.SerializeObject(this, Formatting.Indented, seting));
        }
    }
    //所以场景对象
    public class TSceneDataList
    {
        public Dictionary<string, TSceneData> sceneDataDict;
    }
}
