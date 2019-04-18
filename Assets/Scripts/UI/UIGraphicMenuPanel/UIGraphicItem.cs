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
        //TSceneData model;
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

        void OnInit(UIimg UIimg, RectTransform Viewport)
        {
            rect = transform as RectTransform;
            //��ʼ��Model
            this.Viewport = Viewport;

            //�����Ӷ����¼�ע���
            Image buttonImg = Button.GetComponent<Image>();
            //��ťע���¼�
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
            EditorContent.Show(); buttonImg?.LocalRotation(Quaternion.Euler(new Vector3(0, 0, 0)));
            this.UIimg = UIimg;
        }

        public void Init(KeyValuePair<string, List<string>> kv,UIimg UIimg,RectTransform Viewport)
        {
            OnInit(UIimg, Viewport);
            //��ʼ��Model
            Text.text = kv.Key;
            imgListNmae = kv.Value;
            UpdataGraphicItem(imgListNmae);
        }

        public void Init(DirectoryInfo dir, UIimg UIimg, RectTransform Viewport)
        {
            OnInit(UIimg, Viewport);
            //��ֵitem ����
            Text.text = dir.Name;
            UpdaeGraphicItemForDirectory(dir);
        }

        //����Item
        public void UpdataGraphicItem(List<string> SpriteListNmae)
        {
            //���������img���Ȳ����Ƕ���� //�ֵ���Ƴ�Ŀǰ��û�п���
            EditorContent.transform.ApplySelfTo(self => { if(self.childCount> 0 )foreach (Transform item in self){Destroy(item.gameObject);}});
            
            foreach (var fileName in SpriteListNmae)
            {
                GenerateUIImg(EditorContent.transform, UIimg, fileName);
            }
        }

        public void UpdaeGraphicItemForDirectory(DirectoryInfo dir)
        {
            //���ع��������е�PNG�ļ�
            //���������img���Ȳ����Ƕ���� //�ֵ���Ƴ�Ŀǰ��û�п���
            EditorContent.transform.ApplySelfTo(self => { if (self.childCount > 0) foreach (Transform item in self) { Destroy(item.gameObject); } });

            FileInfo[] files = dir.GetFiles("*.png");

            GetTexture2CreateImg(files);
        }

        ////��ȡ �ļ�Ŀ¼�µ�����png�ļ�������sprites�洢����
        private void GetTexture2CreateImg(FileInfo[] files)
        {
            foreach (FileInfo file in files)
            {
                //FileStream fs = new FileStream(file.FullName, FileMode.Open);
                //byte[] buffer = new byte[fs.Length];
                //fs.Read(buffer, 0, buffer.Length);
                //fs.Close();
                byte[] buffer=Base64Helper.ConvertFileToBase64Bytes(file.FullName);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(buffer);
                tex.Apply();
                tex.name = file.FullName;
                Global.GetSprite(tex);

                GenerateUIImg(EditorContent.transform, UIimg, file.FullName)
                    .ApplySelfTo(self=>self.SettextrueData(Convert.ToBase64String(buffer)));
            }
        }
       // private void DirectoryInitPNG

        //����UIimg����
        private UIimg GenerateUIImg(Transform parent, UIimg UIimg, string spriteKey)
        {
            UIimg img = UIimg.Instantiate()
                .ApplySelfTo(self => self.transform.SetParent(parent, false))
                .ApplySelfTo(self => self.Image.sprite = Global.GetSprite(spriteKey)? Global.GetSprite(spriteKey):Global.GetSprite(loader.LoadSync<Texture2D>(spriteKey)))
                .ApplySelfTo(self => {
                    //�ж��Ƿ��Ǳ������õ�ͼԪ
                    if (spriteKey.Contains(@"\") && spriteKey.Contains(".png"))
                    {
                        self.txt.text = spriteKey.Substring(spriteKey.LastIndexOf(@"\") + 1).Replace(".png", "");
                        self.isConfigImg = true;
                    }
                    else self.txt.text = spriteKey;})
                .ApplySelfTo(self => self.Init(spriteKey, Viewport))
                .ApplySelfTo(self => self.Show());

            UIImgDict.Add(spriteKey, img);
            return img;
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
                //ǿ��ˢ��    
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
            //ǿ��ˢ��    
            StartCoroutine(Global.UpdateLayout(rect));
        }       
    }
}