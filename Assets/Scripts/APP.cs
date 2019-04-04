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
            UIMgr.OpenPanel<UIGraphicMenuPanel>(new UIGraphicMenuPanelData() { model = model });
            UIMgr.OpenPanel<UIShowPanel>(new UIShowPanelData() { model=model});
            UIMgr.OpenPanel<UIServerDatasMenuPanel>();
        }

        private void OnApplicationQuit()
        {
            model.Save();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Delete))
            {
               if (Global.OnSelectedGraphic.IsNotNull())
                {
                    model.Remove(Global.OnSelectedGraphic);
                }
            }
        }

    }
    //场景对象

    public class TSceneData
    {
        public  FloatReactiveProperty canvasWidth = new FloatReactiveProperty(1651);
        public  FloatReactiveProperty canvasHeight = new FloatReactiveProperty(1444);

        public ReactiveCollection<T_Line> LineDataList=new ReactiveCollection<T_Line>();
        
        public ReactiveCollection<T_Image> ImageDataList = new ReactiveCollection<T_Image>();
        
        public ReactiveCollection<T_Text> TextDataList = new ReactiveCollection<T_Text>();
        
        public void Add(T_Graphic model)
        {
            if(model.graphicType==  GraphicType.Image)
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
                    Remove (model as T_Text);
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
            string json = PlayerPrefs.GetString("Test9", string.Empty);
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
            PlayerPrefs.SetString("Test9", JsonConvert.SerializeObject(this, Formatting.Indented, seting));
        }
    }
    //所以场景对象
    public class TSceneDataList
    {
        public Dictionary<string, TSceneData> sceneDataDict;
    }
}
