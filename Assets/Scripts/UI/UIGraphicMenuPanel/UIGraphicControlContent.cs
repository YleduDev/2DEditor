/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.IO;

namespace QFramework.TDE
{
	public partial class UIGraphicControlContent : UIElement
	{
		private void Awake()
		{
		}

		protected override void OnBeforeDestroy()
		{
		}

        internal void GenerateUIGrrphicsItem(UIGraphicItem UIGraphicItem,UIimg UIimg)
        {
            string streamingPath = Application.streamingAssetsPath;

            DirectoryInfo dir = new DirectoryInfo(streamingPath + "/2DEditorGraphics");

            FileSystemInfo[] file = dir.GetFileSystemInfos();

            foreach (FileSystemInfo i in file)
            {
                if (i is DirectoryInfo)
                {
                    CreateEditor((DirectoryInfo)i, transform, UIGraphicItem);
                }               
            }
        }



        private void CreateEditor(DirectoryInfo i, Transform parent, UIGraphicItem UIGraphicItem)
        {
            UIGraphicItem uiGraphicItem= UIGraphicItem.Instantiate();
            uiGraphicItem.ApplySelfTo(self => self.transform.SetParent(parent,false))
                .ApplySelfTo(self => self.name = i.Name).Show();

            Image  tempContent = uiGraphicItem.EditorContent;

            uiGraphicItem.Button.onClick.AddListener(() => {
                if (tempContent.IsActive()){ tempContent.Hide();tempContent.LocalRotation(Quaternion.Euler(new Vector3(0, 0, 90)));}
                else {tempContent.Show(); tempContent.LocalRotation(Quaternion.Euler(new Vector3(0, 0, 0))); }});


            uiGraphicItem.Text.text = i.Name;

            // GetAllFiles(i, tempContent.transform);

            tempContent.Hide();
        }


        //private void CreateEditorContent(FileSystemInfo i, Transform parent)
        //{
        //    string str = i.FullName;

        //    string path = Application.streamingAssetsPath;

        //    string strType = str.Substring(path.Length);

        //    if (strType.Substring(strType.Length - 3).ToLower() == "png")
        //    {

        //        if (dic.ContainsKey(strType))
        //        {
        //            //dic[strType] = spriteImg;
        //        }
        //        else
        //        {

        //            GameObject img = Resources.Load<GameObject>("Prefabs/Img");
        //            GameObject tempImg = Instantiate(img, parent);

        //            Image spriteImg = tempImg.GetComponent<Image>();

        //            strType = strType.Replace("\\", "/");
        //            //Debug.Log(strType);
        //            dic.Add(strType, spriteImg);
        //        }
        //    }
        //}
    }
}