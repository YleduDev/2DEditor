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
using System.Linq;

namespace QFramework.TDE
{
	public partial class UIGraphicItem : UIElement
	{
        TSceneData model;
        Dictionary<string, Sprite> sprites;
        DirectoryInfo dir;
        UIimg UIimg;
        RectTransform Viewport;

        private void Awake(){}

		protected override void OnBeforeDestroy(){}

        public void Init(DirectoryInfo dir,UIimg UIimg,RectTransform Viewport,TSceneData model)
        {
            this.Viewport = Viewport;
            this.model = model;
            //自身及子对象事件注册等
            Image buttonImg = Button.GetComponent<Image>();
            //按钮注册事件
            Button.onClick.AddListener(() => {
                if (EditorContent.IsActive()) {
                    EditorContent.Hide(); buttonImg?.LocalRotation(Quaternion.Euler(new Vector3(0, 0, 90))); }
                else { EditorContent.Show(); buttonImg?.LocalRotation(Quaternion.Euler(new Vector3(0, 0, 0))); } });

            Text.text = dir.Name;

            EditorContent.Show(); buttonImg?.LocalRotation(Quaternion.Euler(new Vector3(0, 0, 0)));

            //初始化Model
            sprites = Global.sprites;
            this.dir = dir;
            this.UIimg = UIimg;

            UpdataGraphicItem();
        }

        //更新Item
        public void UpdataGraphicItem()
        {
            //先清除所有img（先不考虑对象池
            EditorContent.transform.ApplySelfTo(self => { if(self.childCount> 0 )foreach (Transform item in self){Destroy(item.gameObject);}});
            //获取文件夹下所有png文件信息
            FileInfo[] files = dir.GetFiles("*.png");
            //生成sprite并存储缓存
            GetTexture(files);
            //没个item下生成子对象
            foreach (FileInfo file in files)
            {
                GenerateUIImg(EditorContent.transform, UIimg, file.FullName);
            }
        }

        //获取 文件目录下的所有png文件并生成sprites存储缓存
        private void GetTexture(FileInfo[] files)
        {
            foreach (FileInfo file in files)
            {
                FileStream fs = new FileStream(file.FullName, FileMode.Open);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(buffer);
                tex.Apply();
                tex.name = file.Name.Replace(".png", "");
                String key = file.FullName;
                //防止字典异常
                sprites[key]= Global.ChangeToSprite(tex);
            }
        }
       
        //生成UIimg对象
        private void GenerateUIImg(Transform parent, UIimg UIimg, string spriteFullName)
        {
             UIimg.Instantiate()
                .ApplySelfTo(self => self.transform.SetParent(parent, false))
                .ApplySelfTo(self => self.GetComponent<Image>().sprite = Global.GetSprite(spriteFullName))
                .ApplySelfTo(self=> self.Init(spriteFullName, Viewport,model))
                .Show();
        }
    }
}