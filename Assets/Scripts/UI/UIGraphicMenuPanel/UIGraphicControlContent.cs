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
        TSceneData model;
        ResLoader loader = ResLoader.Allocate();
        private void Awake(){}

		protected override void OnBeforeDestroy(){
            loader.Recycle2Cache();
            loader = null;
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
           return  UIGraphicItem.Instantiate()
            .ApplySelfTo(self => self.transform.SetParent(parent, false))
            .ApplySelfTo(self => self.name = itemName)
            .ApplySelfTo(self => self.Show());
        }     
    }
}