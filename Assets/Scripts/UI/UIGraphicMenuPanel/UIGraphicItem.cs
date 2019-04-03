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
        List<string> imgListNmae;
        UIimg UIimg;
        RectTransform Viewport;

        ResLoader loader = ResLoader.Allocate();
        private void Awake(){}

		protected override void OnBeforeDestroy(){
            loader.Recycle2Cache();
            loader = null;
        }

        public void Init(KeyValuePair<string, List<string>> kv,UIimg UIimg,RectTransform Viewport,TSceneData model)
        {
            this.Viewport = Viewport;
            this.model = model;
            //�����Ӷ����¼�ע���
            Image buttonImg = Button.GetComponent<Image>();
            //��ťע���¼�
            Button.onClick.AddListener(() => {
                if (EditorContent.IsActive()) {
                    EditorContent.Hide(); buttonImg?.LocalRotation(Quaternion.Euler(new Vector3(0, 0, 90))); }
                else { EditorContent.Show(); buttonImg?.LocalRotation(Quaternion.Euler(new Vector3(0, 0, 0))); } });

            Text.text = kv.Key;
            imgListNmae = kv.Value;

            EditorContent.Show(); buttonImg?.LocalRotation(Quaternion.Euler(new Vector3(0, 0, 0)));

            //��ʼ��Model

            this.UIimg = UIimg;

            UpdataGraphicItem();
        }

        //����Item
        public void UpdataGraphicItem()
        {
            //���������img���Ȳ����Ƕ����
            EditorContent.transform.ApplySelfTo(self => { if(self.childCount> 0 )foreach (Transform item in self){Destroy(item.gameObject);}});
            //��ȡ�ļ���������png�ļ���Ϣ
            //FileInfo[] files = dir.GetFiles("*.png");
            //����sprite���洢����
            //GetTexture(files);
            //û��item�������Ӷ���
            foreach (var fileName in imgListNmae)
            {
                GenerateUIImg(EditorContent.transform, UIimg, fileName);
            }
        }

        ////��ȡ �ļ�Ŀ¼�µ�����png�ļ�������sprites�洢����
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
        //        //��ֹ�ֵ��쳣
        //        sprites[key]= Global.ChangeToSprite(tex);
        //    }
        //}
       
        //����UIimg����
        private void GenerateUIImg(Transform parent, UIimg UIimg, string spriteFullName)
        {
             UIimg.Instantiate()
                .ApplySelfTo(self => self.transform.SetParent(parent, false))
                .ApplySelfTo(self => self.GetComponent<Image>().sprite = Global.GetSprite(loader.LoadSync<Texture2D>(spriteFullName)))
                .ApplySelfTo(self=> self.Init(spriteFullName, Viewport,model))
                .Show();
        }
    }
}