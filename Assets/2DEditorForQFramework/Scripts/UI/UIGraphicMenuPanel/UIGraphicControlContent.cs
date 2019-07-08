/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System.Linq;
using UniRx;
#if UNITY_WEBGL
#else
using System.IO;
#endif
using TDE;

namespace QFramework.TDE
{

	public partial class UIGraphicControlContent : UIElement
	{
        public  RectTransform rect;
        //TSceneData model;
        ResLoader loader = ResLoader.Allocate();
        UIGraphicItem UIGraphicItem;
        UIimg UIimg;
        UIWidgetImg UIWidgetImg;
        UIDirButton UIDirBtn;
        RectTransform Viewport;

        public static Dictionary<string, string> WidgetPrefabDict = new Dictionary<string, string>();

        List<UIGraphicItem> stremGrephicItemDict = new List<UIGraphicItem>();

        //图元组管理 （总图元组字典管理 及 展示（选择）图元组字典管理）
        internal class UIGraphicItemDictMrg
        {
            //总的图元组字典
            private Dictionary<string, UIGraphicItem> UIGraphicItemDict = new Dictionary<string, UIGraphicItem>();
            //显示、操作的图元组字典
            private Dictionary<string, UIGraphicItem> ActiveGraphicItemDict = new Dictionary<string, UIGraphicItem>();

            Dictionary<string, bool> sets;

            public void Add(string key, UIGraphicItem item)
            {
                if (UIGraphicItemDict.ContainsKey(key))
                {
                    Log.LogError("不能添加相同的key"+key);
                }
                else
                {
                    UIGraphicItemDict.Add(key, item);
                }
                if (ActiveGraphicItemDict.ContainsKey(key))
                {
                    Log.LogError("不能添加相同的key" + key);
                }
                else
                {
                    ActiveGraphicItemDict.Add(key, item);
                }
                if (sets.IsNotNull() && !sets.ContainsKey(key)) sets.Add(key,item.isActiveAndEnabled);

            }
            public void Remove(string key)
            {
                if (UIGraphicItemDict.ContainsKey(key))
                {
                    UIGraphicItemDict.Remove(key);
                }
                if (ActiveGraphicItemDict.ContainsKey(key))
                {
                    ActiveGraphicItemDict.Remove(key);
                }
                if (sets.IsNotNull() && sets.ContainsKey(key)) sets.Remove(key);

            }

            public UIGraphicItem Get(String key )
            {
                if (UIGraphicItemDict.ContainsKey(key)) return UIGraphicItemDict[key]; return null;
            }
         

            //对外提供的设置图组 显示隐藏   Action表示设置显隐成功后的事件
            public void SetUIGraphicItemActive(string name, bool bo,Action action)
            {
                if (UIGraphicItemDict.ContainsKey(name))
                {
                    if (bo)
                    {
                        UIGraphicItemDict[name].Show();
                        if (!ActiveGraphicItemDict.ContainsKey(name)) ActiveGraphicItemDict.Add(name, UIGraphicItemDict[name]);
                       
                    }
                    else
                    {
                        UIGraphicItemDict[name].Hide();
                        if (ActiveGraphicItemDict.ContainsKey(name)) ActiveGraphicItemDict.Remove(name);
                    }
                    if (sets.IsNotNull() && sets.ContainsKey(name)) sets[name] = bo;
                    action?.Invoke();
                }
            }

            //显示 图元组集合刷新
            public void ActiveDict2UpdateLayout()
            {
                ActiveGraphicItemDict.ForEach(item => { if (item.Value.isActiveAndEnabled) item.Value.UpdateLayout(); });
            }
            //显示图元组显示全部
            public void ActiveDictShowAll()
            {
                ActiveGraphicItemDict.ForEach(item => item.Value.ShowSelf2AllChild());
            }
            //显示图元组搜索检测
            public void AcriveDictCheck(string check,Action act)
            {
                ActiveGraphicItemDict.ForEach(item => { if (item.Value.CheckUIimgName(check)) act?.Invoke(); });
            }
            //获取当前总字典的 设置
            internal PrimSet GetPrimSet(UIGraphicControlContent content)
            {
                if (UIGraphicItemDict.IsNull() || UIGraphicItemDict.Count <= 0) return null;
                Dictionary<string, bool> data = new Dictionary<string, bool>();
                foreach (KeyValuePair<string, UIGraphicItem> kv in UIGraphicItemDict)
                {
                    data.Add(kv.Key, kv.Value.isActiveAndEnabled);
                }
                return new PrimSet() { content = content, data = data };
            }

            //加载  Action 表示加载成功后的事件
            public void LoadSets(Action action)
            {
              string data=  PlayerPrefs.GetString("UIGraphicItemDictTest", string.Empty);
                if (!data.IsNullOrEmpty())
                {
                    sets = SerializeHelper.FromJson<Dictionary<string, bool>>(data);
                    if (sets.IsNotNull() && sets.Count > 0)
                    {
                       
                        string[] keys = sets.Keys.ToArray();
                        for (int i = 0; i < keys.Length; i++)
                        {
                            SetUIGraphicItemActive(keys[i], sets[keys[i]], action);
                        }
                    }
                    
                }
                else
                {
                    sets = new Dictionary<string, bool>();
                    foreach (KeyValuePair<string, UIGraphicItem> kv in UIGraphicItemDict)
                    {
                        sets.Add(kv.Key, true);
                    }
                }
            }
            //保存
            public void SaveSets()
            {
                if(sets.IsNotNull() && sets.Count > 0)
                PlayerPrefs.SetString("UIGraphicItemDictTest", sets.ToJson());
            }

        }

        UIGraphicItemDictMrg dictMrg = new UIGraphicItemDictMrg();

        private void Awake(){}

		protected override void OnBeforeDestroy(){
            loader.Recycle2Cache();
            loader = null;
            dictMrg.SaveSets();
        }
        
        //对总字典进行显隐=>会对activeDict 产生影响
        public void SetUIGraphicItemActive(string key,bool bo)
        {
            dictMrg.SetUIGraphicItemActive(key, bo, () => StartCoroutine(Global.UpdateLayout(rect)));
        }

        //初始化图元组
        internal void GenerateUIGrrphicsItemOnInit(UIGraphicItem UIGraphicItem, UIimg UIimg, UIWidgetImg UIWidgetImg,UIDirButton UIDirBtn, RectTransform Viewport)
        {
            rect = transform as RectTransform;
            //默认配置
            this.UIGraphicItem = UIGraphicItem;
            this.UIimg = UIimg;
            this.UIWidgetImg = UIWidgetImg;
            this.UIDirBtn = UIDirBtn;
            this.Viewport = Viewport;

            //初始化WidgetConfig
            var WidgetText = loader.LoadSync<TextAsset>(Global.WidgetConfigPathName);
            var WidgetDict = SerializeHelper.FromJson< List<string>>(WidgetText.text);

            foreach (string kv in WidgetDict)
            {
                UIGraphicItem GraphicItem = CreateGraphicItem(Global.allWidgetsFillName, transform, UIGraphicItem);
                GraphicItem.Init(Global.allWidgetsFillName, WidgetDict, UIWidgetImg, Viewport);
            }
            //初始化 WidgetPrefabDict
            var WidgetPrefabText = loader.LoadSync<TextAsset>(Global.WidgetPrefabConfigPathName);
            WidgetPrefabDict = SerializeHelper.FromJson<Dictionary<string,string>>(WidgetPrefabText.text);

            //初始化GraphisMenuConfig
            var text = loader.LoadSync<TextAsset>(Global.GraphisMenuConfigPathName);
            var dict = SerializeHelper.FromJson<Dictionary<string, List<string>>>(text.text);
            foreach (KeyValuePair<string, List<string>> kv in dict)
            {
                UIGraphicItem GraphicItem = CreateGraphicItem(kv.Key, transform, UIGraphicItem);
                GraphicItem.Init(kv, UIimg, Viewport); 
            }

#if UNITY_WEBGL
#else
            string path = FilePath.StreamingAssetsPath + Global.CustomCinfigGraphicsPathName;

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
             // 本地配置（目前无法应用到webgl）
            DirectoryInfo dir = new DirectoryInfo(path);

            DirectoryInfo[] childDirs = dir.GetDirectories();

            foreach (DirectoryInfo i in childDirs)
            {
                if (i.Parent.Name.Equals(Global.CustomCinfigGraphicsPathName))
                {
                    UIGraphicItem GraphicItem = CreateGraphicItem(i, transform, UIGraphicItem);
                    //Debug.Log(i.FullName);
                    GraphicItem.Init(i, UIimg, UIDirBtn, Viewport, i.FullName);
                    stremGrephicItemDict.Add(GraphicItem);
                                     
                }
            }
#endif
            dictMrg.LoadSets(() => StartCoroutine(Global.UpdateLayout(rect)));
            //刷新
            dictMrg.ActiveDict2UpdateLayout();
        }

#if UNITY_WEBGL
#else

      
        //本地目录GraphicItem
        private UIGraphicItem CreateGraphicItem(DirectoryInfo i, Transform parent, UIGraphicItem UIGraphicItem)
        {
            //生成itemd
            return CreateGraphicItem(i.Name, parent, UIGraphicItem);
        }
       
        //刷新 本地自定义 图元组
        public void UpdateCounstGraphicItem()
        {
            string path = FilePath.StreamingAssetsPath + Global.CustomCinfigGraphicsPathName;

            DirectoryInfo dir = new DirectoryInfo(path);

            DirectoryInfo[] childDirs = dir.GetDirectories();

            foreach (DirectoryInfo i in childDirs)
            {
                if (i.Parent.Name.Equals(Global.CustomCinfigGraphicsPathName))
                {
                    // 删除 所有 自定义配置图元组
                    DeleteItem(i.Name);
                }
            }
            //生成
            foreach (DirectoryInfo i in childDirs)
            {
                if (i.Parent.Name.Equals(Global.CustomCinfigGraphicsPathName))
                {
                    UIGraphicItem GraphicItem = CreateGraphicItem(i, transform, UIGraphicItem);
                    //Debug.Log(i.FullName);
                    GraphicItem.Init(i, UIimg, UIDirBtn, Viewport, i.FullName);
                    stremGrephicItemDict.Add(GraphicItem);
                }
            }
            //刷新
            dictMrg.ActiveDict2UpdateLayout();

        }

#endif
        private void DeleteItem(string key)
        {
            UIGraphicItem item = dictMrg.Get(key);
            dictMrg.Remove(key);
            if (item & stremGrephicItemDict.Remove(item))
                Destroy(item.gameObject);
        }
        //生成GraphicItem
        private UIGraphicItem CreateGraphicItem(String itemName, Transform parent, UIGraphicItem UIGraphicItem)
        {
            string targetPaht = FilePath.StreamingAssetsPath + Global.CustomCinfigGraphicsPathName + "/" + itemName;

            UIGraphicItem item= UIGraphicItem.Instantiate()
            .ApplySelfTo(self => self.transform.SetParent(parent, false))
            .ApplySelfTo(self => self.name = itemName)
            .ApplySelfTo(self => self.Show())
            //注册 删除按钮事件
            .ApplySelfTo(self => self.DeletBtn.onClick.AddListener(() => { DeleteItem(itemName); PictureMgrForWindows.Instance.DeleteDir(targetPaht); }));
            dictMrg.Add(itemName, item);
            return item;
        }     

        //搜索功能事件
        public void CheckShow(string checkName)
        {   
            // Epmey null
            if (checkName.IsNullOrEmpty())
            {
                ShowAllGraphicItem(); 
                //强制刷新
                StartCoroutine(Global.UpdateLayout(rect));
            }
            // Not null epmey
            else
            {
                CheckGranhicItem(checkName);
            }
        }

        //展示所有item
        void ShowAllGraphicItem()
        {

            dictMrg.ActiveDictShowAll();
        }
        //搜索关键字 的图元 并展示所在图组
        void CheckGranhicItem(string check)
        {
            dictMrg.AcriveDictCheck(check, () => StartCoroutine(Global.UpdateLayout(rect)));         
        }

        public PrimSet GetPrimSet()
        {
            return dictMrg.GetPrimSet(this);      
        }

        public class PrimSet
        {
            public UIGraphicControlContent content;
            public  Dictionary<string, bool> data;
        }

        //设置UIimg 删除按钮
        public void SetUIimgActiveForItem(bool bo)
        {
            if (stremGrephicItemDict.IsNotNull())stremGrephicItemDict.Where(item => item.IsNotNull()).ForEach(item => item.SetAllUIimgActive(bo));
        }
        //设置UIGraphicItem 删除按钮
        public void SetGruphicItemsDeleteBtnActive(bool bo)
        {
            if (stremGrephicItemDict.IsNotNull()) stremGrephicItemDict.Where(item => item.IsNotNull()).ForEach(item => item.SetDeleteBtnActive(bo));
        }
    }
}