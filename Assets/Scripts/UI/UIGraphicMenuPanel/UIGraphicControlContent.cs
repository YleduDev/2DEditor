/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System.IO;
using TDE;

namespace QFramework.TDE
{
	public partial class UIGraphicControlContent : UIElement
	{
        RectTransform rect;
        TSceneData model;
        ResLoader loader = ResLoader.Allocate();
        private Dictionary<string, UIGraphicItem> UIGraphicItemDict = new Dictionary<string, UIGraphicItem>();

        private void Awake(){
            rect = transform as RectTransform;
        }

		protected override void OnBeforeDestroy(){
            loader.Recycle2Cache();
            loader = null;
        }
        public void SetUIGraphicItemActive(string name,bool bo)
        {
            if (UIGraphicItemDict.ContainsKey(name)) if (bo) UIGraphicItemDict[name].Show();else UIGraphicItemDict[name].Hide();
        }

        internal void GenerateUIGrrphicsItem(UIGraphicItem UIGraphicItem,UIimg UIimg,RectTransform Viewport, TSceneData model)
        {        
            this.model = model;
            var text= loader.LoadSync<TextAsset>(Global.GraphisMenuConfigPathName);
            var dict = SerializeHelper.FromJson<Dictionary<string, List<string>>>(text.text);
            foreach (KeyValuePair<string, List<string>> kv in dict)
            {
                UIGraphicItem GraphicItem = CreateGraphicItem(kv.Key, transform, UIGraphicItem);
                GraphicItem.Init(kv, UIimg, Viewport, model);
            }
        }
        //生成GraphicItem
        private UIGraphicItem CreateGraphicItem(DirectoryInfo i, Transform parent, UIGraphicItem UIGraphicItem)
        {
            //生成item
            return CreateGraphicItem(i.Name, parent, UIGraphicItem);
        }

        //生成GraphicItem
        private UIGraphicItem CreateGraphicItem(String itemName, Transform parent, UIGraphicItem UIGraphicItem)
        {
            UIGraphicItem item= UIGraphicItem.Instantiate()
            .ApplySelfTo(self => self.transform.SetParent(parent, false))
            .ApplySelfTo(self => self.name = itemName)
            .ApplySelfTo(self => self.Show());
            UIGraphicItemDict.Add(itemName, item);
            return item;
        }     

        public void CheckShow(string checkName)
        {         // Epmey null
            if (checkName.IsNullOrEmpty())
            {
                ShowAllGraphicItem();
            }
            // Not null epmey
            else
            {
                CheckGranhicItem(checkName);
            }
        }

        
        void ShowAllGraphicItem()
        {
            UIGraphicItemDict.ForEach(item =>item.Value.ShowSelf2AllChild());
        }
        void CheckGranhicItem(string check)
        {
            UIGraphicItemDict.ForEach(item => { if (item.Value.CheckUIimgName(check)) StartCoroutine(Global.UpdateLayout(rect)); });
        }
    }
}