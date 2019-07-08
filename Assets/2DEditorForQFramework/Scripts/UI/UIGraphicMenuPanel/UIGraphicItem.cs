/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
#if UNITY_WEBGL
#else
using System.IO;
#endif
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
        UIWidgetImg UIWidgetImg;
        RectTransform Viewport;
        private Dictionary<string, UIimg> UIImgDict = new Dictionary<string, UIimg>();
        private Dictionary<string, UIWidgetImg> UIWidgetImgDict = new Dictionary<string, UIWidgetImg>();

        ResLoader loader = ResLoader.Allocate();
        private void Awake(){}

		protected override void OnBeforeDestroy(){
            loader.Recycle2Cache();
            loader = null;
        }

        void OnInit(RectTransform Viewport)
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
            
        }

        internal void Init(string allWidgetsFillName, List<string> widgetList, UIWidgetImg uIWidgetImg, RectTransform viewport)
        {
            OnInit(viewport);
            this.UIWidgetImg = uIWidgetImg;
          //  Log.I(allWidgetsFillName);
            Text.text = allWidgetsFillName;
            UpdataGraphicItem(widgetList);
        }

        public void Init(KeyValuePair<string, List<string>> kv,UIimg UIimg,RectTransform Viewport)
        {
            OnInit( Viewport);
            //��ʼ��Model
            this.UIimg = UIimg;
            Text.text = kv.Key;
            imgListNmae = kv.Value;
            UpdataGraphicItem(imgListNmae);
            
        }
#if UNITY_WEBGL
#else
        DirectoryInfo dirInfo;
        UIDirButton UIDirBtn;
        string path;
        private Dictionary<string, Texture2D> dirTextureDict = new Dictionary<string, Texture2D>();
        private Dictionary<string, byte[]> dirByteDict = new Dictionary<string, byte[]>();

        
        public void Init(DirectoryInfo dir, UIimg UIimg,UIDirButton UIDirBtn, RectTransform Viewport,string path)
        {
            OnInit( Viewport);
            //��ֵitem ����
            this.UIimg = UIimg;
            Text.text = dir.Name;
            dirInfo=dir;
            this. UIDirBtn = UIDirBtn;
            this.path = path;
            UpdaeGraphicItemForDirectory();
        }

        public void UpdaeGraphicItemForDirectory()
        {
           if(dirInfo==null) return;
            //���ع��������е�PNG�ļ�
            //���������img���Ȳ����Ƕ���� //�ֵ���Ƴ�Ŀǰ��û�п���
            if (UIImgDict.IsNotNull()) UIImgDict.Clear();
            EditorContent.transform.ApplySelfTo(self => { if (self.childCount > 0) foreach (Transform item in self) { Destroy(item.gameObject); } });

            FileInfo[] files = dirInfo.GetFiles("*.png");

            GetTexture2CreateImg(files);          
            //���dirButton
            CreateDirButton(UpdaeGraphicItemForDirectory);
        }

        private void CreateDirButton(Action act)
        { 
            UIDirButton btn= UIDirBtn.Instantiate();
            btn.ApplySelfTo(self => self.transform.SetParent(EditorContent.transform, false))
               .ApplySelfTo(self => self.Show())
               .ApplySelfTo(self => self.Init(path, act));
        }

        ////��ȡ �ļ�Ŀ¼�µ�����png�ļ�������sprites�洢����
        private void GetTexture2CreateImg(FileInfo[] files)
        {
            foreach (FileInfo file in files)
            {
                Texture2D tex=null;
                byte[] buffer = null;
                if (!dirTextureDict.ContainsKey(file.FullName))
                {
                    buffer = Base64Helper.ConvertFileToBase64Bytes(file.FullName);
                    tex = new Texture2D(2, 2);
                    tex.LoadImage(buffer);
                    tex.Apply();
                    tex.name = file.FullName;

                    dirTextureDict.Add(file.FullName, tex);
                    dirByteDict.Add(file.FullName, buffer);
                }
                else
                {
                    tex = dirTextureDict[file.FullName];
                    buffer = dirByteDict[file.FullName];
                }

                Global.GetSprite(tex);

                GenerateUIImg(EditorContent.transform, UIimg, file.FullName)
                    .ApplySelfTo(self=>self.SettextrueData(Convert.ToBase64String(buffer)));
            }
        }
#endif
        //����Item
        public void UpdataGraphicItem(List<string> SpriteListNmae)
        {
            //���������img���Ȳ����Ƕ���� //�ֵ���Ƴ�Ŀǰ��û�п���
            if (UIImgDict.IsNotNull()) UIImgDict.Clear();
            if (UIWidgetImgDict.IsNotNull()) UIWidgetImgDict.Clear();
            EditorContent.transform.ApplySelfTo(self => { if(self.childCount> 0 )foreach (Transform item in self){Destroy(item.gameObject);}});
            
            foreach (var fileName in SpriteListNmae)
            {
              if(UIimg.IsNotNull()) GenerateUIImg(EditorContent.transform, UIimg, fileName);
              else if (UIWidgetImgDict.IsNotNull()) GenerateUIImg(EditorContent.transform, UIWidgetImg, fileName);
            }
        }

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
                .ApplySelfTo(self => self.Init(spriteKey, Viewport,()=>  RemoveUIImg(spriteKey)))
                .ApplySelfTo(self => self.Show());

            UIImgDict.Add(spriteKey, img);
            return img;
        }
        //����UIWidgetImg����
        private UIWidgetImg GenerateUIImg(Transform parent, UIWidgetImg UIWidgetImg, string spriteKey)
        {
            UIWidgetImg widgetImg = UIWidgetImg.Instantiate()
                .ApplySelfTo(self => self.transform.SetParent(parent, false))
                .ApplySelfTo(self => self.Image.sprite = Global.GetSprite(spriteKey) ? Global.GetSprite(spriteKey) : Global.GetSprite(loader.LoadSync<Texture2D>(spriteKey)))
                .ApplySelfTo(self => self.txt.text = spriteKey)
                .ApplySelfTo(self => self.Init(spriteKey, Viewport))
                .ApplySelfTo(self => self.Show());

            UIWidgetImgDict.Add(spriteKey, widgetImg);
            return widgetImg;
        }

        private void RemoveUIImg(string key)
        {
            if (UIImgDict.ContainsKey(key))
            {
                Destroy(UIImgDict[key].gameObject);
                UIImgDict.Remove(key);
            }
#if UNITY_WEBGL
#else
            PictureMgrForWindows.Instance.DeleteFile(key);
#endif
        }
        //����ͼ 
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

        

        //����ͼ�� ����ͼ ����
        private void SetUIImgActive(bool bo)
        {
            if (UIImgDict.IsNull() && UIImgDict.Count <= 0) return;
            UIImgDict.ForEach(item => {
                if (item.Value.IsNull()) return;
                if (bo) item.Value.Show();
                else if(!bo) item.Value.Hide();
            });
        }

        //��ʾ�����������ͼ
        public void ShowSelf2AllChild()
        {
            SetUIImgActive(true);
            Show();
            //ǿ��ˢ��    
            StartCoroutine(Global.UpdateLayout(rect));
        }
        //ˢ��
        public void UpdateLayout()
        {
            StartCoroutine(Global.UpdateLayout(rect));
        }

        public void SetAllUIimgActive(bool bo)
        {
            if (UIImgDict.IsNotNull()) UIImgDict.Where(item => item.Value != null).ForEach(item => item.Value.SetUIimgBtnActive(bo));
        }

        public void SetDeleteBtnActive(bool bo)
        {
            bo = bo ? DeletBtn.Show() : DeletBtn.Hide();
        }
    }
}