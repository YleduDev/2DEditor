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
            //model.graphicDataList.Add(new T_Text() {
            //    graphicType = GraphicType.Text,
            //    localScale = new Vector3ReactiveProperty(Vector3.one),
            //    localPos = new Vector2ReactiveProperty(new Vector2(-645f, 395f)),
            //    locaRotation = new QuaternionReactiveProperty(Quaternion.identity),
            //    widht = new FloatReactiveProperty(100f),
            //    height = new FloatReactiveProperty(100f)
            //});

            //model.graphicDataList.Add (new T_Image() {
            //    graphicType = GraphicType.Image,
            //    localScale =new Vector3ReactiveProperty(Vector3.one),
            //    localPos = new Vector2ReactiveProperty(new Vector2(776f, -374f)),
            //    locaRotation = new QuaternionReactiveProperty(Quaternion.identity),
            //    widht = new FloatReactiveProperty(100f),
            //    height = new FloatReactiveProperty(100f)
            //});

            //model.graphicDataList.Add(new T_Image()
            //{
            //    graphicType = GraphicType.Image,
            //    localScale = new Vector3ReactiveProperty(Vector3.one),
            //    localPos = new Vector2ReactiveProperty(new Vector2(356f, 88f)),
            //    locaRotation = new QuaternionReactiveProperty(Quaternion.identity),
            //    widht = new FloatReactiveProperty(100f),
            //    height = new FloatReactiveProperty(100f)
            //});

            

           

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
        public ReactiveCollection<T_Graphic> graphicDataList;
        public void Add(T_Graphic model)
        {
            graphicDataList.Add(model);
        }
        public void Remove(T_Graphic model)
        {
            graphicDataList.Remove(model);
        }
        public static TSceneData Load()
        {
            //第一个参数如果没有 返回一个 值为null 的json
            string json = PlayerPrefs.GetString("Test6", string.Empty);
            if (json.IsNullOrEmpty()) return new TSceneData();
            else
            {
                return SerializeHelper.FromJson<TSceneData>(json);
            }
        }

        public  void Save()
        {
            PlayerPrefs.SetString("Test6", this.ToJson());
        }
    }
    //所以场景对象
    public class TSceneDataList
    {
        public Dictionary<string, TSceneData> sceneDataDict;
    }
}
