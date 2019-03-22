/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.IO;
using TDE;

namespace QFramework.TDE
{
	public partial class UIGraphicControlContent : UIElement
	{
		private void Awake(){}

		protected override void OnBeforeDestroy(){}

        //存储当前所有的Sprite
        public static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

        //获取Sprite
        public static Sprite GetSprite(string key)
        {
            key = key.Trim();
            //int spriteKey;
            if (sprites != null && sprites.ContainsKey(key)) return sprites[key];
            return null;
        }

        internal void GenerateUIGrrphicsItem(UIGraphicItem UIGraphicItem,UIimg UIimg)
        {
            string fileName = "2DEditorGraphics";
            string path = Application.streamingAssetsPath + "/"+ fileName;

           DirectoryInfo dir = new DirectoryInfo(path);

            DirectoryInfo[] childDirs = dir.GetDirectories();

            foreach (DirectoryInfo i in childDirs)
            {
                if ( i.Parent.Name.Equals(fileName))
                {
                    UIGraphicItem GraphicItem= CreateGraphicItem(i, transform, UIGraphicItem, UIimg);
                    GraphicItem.Init(sprites, i, UIimg);
                }               
            }
        }
        //生成GraphicItem
        private UIGraphicItem CreateGraphicItem(DirectoryInfo i, Transform parent, UIGraphicItem UIGraphicItem,UIimg UIimg)
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