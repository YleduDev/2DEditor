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
using System.Collections;

namespace QFramework.TDE
{
	public partial class UIGraphicItem : UIElement
	{
        RectTransform rect;
        TSceneData model;
        List<string> imgListNmae;
        UIimg UIimg;
        RectTransform Viewport;
        private Dictionary<string, UIimg> UIImgDict = new Dictionary<string, UIimg>();

        ResLoader loader = ResLoader.Allocate();
        private void Awake(){}

		protected override void OnBeforeDestroy(){
            loader.Recycle2Cache();
            loader = null;
        }

        public void Init(KeyValuePair<string, List<string>> kv,UIimg UIimg,RectTransform Viewport,TSceneData model)
        {
            rect = transform as RectTransform;
            this.Viewport = Viewport;
            this.model = model;
            //自身及子对象事件注册等
            Image buttonImg = Button.GetComponent<Image>();
            //按钮注册事件
            Button.onClick.AddListener(() => {
                if (EditorContent.IsActive())
                {
                    EditorContent.Hide(); buttonImg?.LocalRotation(Quaternion.Euler(new Vector3(0, 0, 90)));
                    StartCoroutine(Global.UpdateLayout(rect));
                }
                else
                {
                    EditorContent.Show(); buttonImg?.LocalRotation(Quaternion.Euler(new Vector3(0, 0, 0)));
                    StartCoroutine(Global.UpdateLayout(rect));
                }
            });

            Text.text = kv.Key;
            imgListNmae = kv.Value;


            EditorContent.Show(); buttonImg?.LocalRotation(Quaternion.Euler(new Vector3(0, 0, 0)));

            //初始化Model

            this.UIimg = UIimg;

            UpdataGraphicItem();
        }

        //更新Item
        public void UpdataGraphicItem()
        {
            //先清除所有img（先不考虑对象池 //字典的移出目前都没有考虑
            EditorContent.transform.ApplySelfTo(self => { if(self.childCount> 0 )foreach (Transform item in self){Destroy(item.gameObject);}});
            //获取文件夹下所有png文件信息
            //FileInfo[] files = dir.GetFiles("*.png");
            //生成sprite并存储缓存
            //GetTexture(files);
            //没个item下生成子对象
            foreach (var fileName in imgListNmae)
            {
                GenerateUIImg(EditorContent.transform, UIimg, fileName);
            }
        }

        ////获取 文件目录下的所有png文件并生成sprites存储缓存
        //private void GetTexture(FileInfo[] files)
        //{
        //    foreach (FileInfo file in files)
        //    {
        //        FileStream fs = new FileStream(file.FullName, FileMode.Open);
        //        byte[] buffer = new byte[fs.Length];
        //        fs.Read(buffer, 0, buffer.Length);
        //        fs.Close();
        //        Texture2D tex = new Texture2D(2, 2);
        //        tex.LoadImage(buffer);
        //        tex.Apply();
        //        tex.name = file.Name.Replace(".png", "");
        //        String key = file.FullName;
        //        //防止字典异常
        //        sprites[key]= Global.ChangeToSprite(tex);
        //    }
        //}
       
        //生成UIimg对象
        private void GenerateUIImg(Transform parent, UIimg UIimg, string spriteFullName)
        {
            UIimg img = UIimg.Instantiate()
                .ApplySelfTo(self => self.transform.SetParent(parent, false))
                .ApplySelfTo(self => self.Image.sprite = Global.GetSprite(loader.LoadSync<Texture2D>(spriteFullName)))
                .ApplySelfTo(self => self.txt.text = spriteFullName)
                .ApplySelfTo(self => self.Init(spriteFullName, Viewport, model))
                .ApplySelfTo(self => self.Show());

            UIImgDict.Add(spriteFullName, img);
        }


        public bool  CheckUIimgName(string check)
        {
            bool show = false;
            if (UIImgDict.IsNull() && UIImgDict.Count <= 0) return show;
            UIImgDict.ForEach(item =>
            {
                if (!item.Key.Contains(check))
                {
                    item.Value.Hide();
                }
                else
                {
                    item.Value.Show();
                    show = true;
                    
                }
            });
            if (show)
            {
                Show();
                //强制刷新    
                StartCoroutine(Global.UpdateLayout(rect));
            }
            else
                Hide();
            return show;       
        }

        private void SetUIImgActive(bool bo)
        {
            if (UIImgDict.IsNull() && UIImgDict.Count <= 0) return;
            UIImgDict.ForEach(item => {
                if (item.Value.IsNull()) return;
                if (bo) item.Value.Show();
                else if(!bo) item.Value.Hide();
            });
        }

        public void ShowSelf2AllChild()
        {
            SetUIImgActive(true);
            Show();
        }


        
    }
}