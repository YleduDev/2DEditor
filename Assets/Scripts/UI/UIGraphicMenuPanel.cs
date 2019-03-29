//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QFramework.TDE
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    using UniRx;
    using global::TDE;
    using System.Runtime.InteropServices;
    using System.IO;
    //using System.Windows.Forms;
    #region OpenFileName数据接收类
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class OpenFileName
    {
        public int structSize = 0;
        public IntPtr dlgOwner = IntPtr.Zero;
        public IntPtr instance = IntPtr.Zero;
        public String filter = null;
        public String customFilter = null;
        public int maxCustFilter = 0;
        public int filterIndex = 0;
        public String file = null;
        public int maxFile = 0;
        public String fileTitle = null;
        public int maxFileTitle = 0;
        public String initialDir = null;
        public String title = null;
        public int flags = 0;
        public short fileOffset = 0;
        public short fileExtension = 0;
        public String defExt = null;
        public IntPtr custData = IntPtr.Zero;
        public IntPtr hook = IntPtr.Zero;
        public String templateName = null;
        public IntPtr reservedPtr = IntPtr.Zero;
        public int reservedInt = 0;
        public int flagsEx = 0;
    }
    #endregion

    #region 系统函数调用类
    public class LocalDialog
    {
        //链接指定系统函数       打开文件对话框
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool GetOpenFileName([InAttribute, OutAttribute] OpenFileName ofn);
        public static bool GetOFN([InAttribute, OutAttribute] OpenFileName ofn)
        {
            return GetOpenFileName(ofn);
        }

        //链接指定系统函数        另存为对话框
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool GetSaveFileName([InAttribute, OutAttribute] OpenFileName ofn);
        public static bool GetSFN([InAttribute, OutAttribute] OpenFileName ofn)
        {
            return GetSaveFileName(ofn);
        }
    }
    #endregion
    public class UIGraphicMenuPanelData : QFramework.UIPanelData
    {
        public TSceneData model;
    }
    
    public partial class UIGraphicMenuPanel : QFramework.UIPanel
    {
        public static  Vector2ReactiveProperty TitleImgLocalPos = new Vector2ReactiveProperty();
        public static BoolReactiveProperty TitleImgActive = new BoolReactiveProperty(false);
        public static ReactiveProperty<Sprite> TitleSprite = new ReactiveProperty<Sprite>();

        RectTransform rectGraphicView;
        

        protected override void ProcessMsg(int eventId, QFramework.QMsg msg)
        {
            
        }
        
        protected override void OnInit(QFramework.IUIData uiData)
        {
            mData = uiData as UIGraphicMenuPanelData ?? new UIGraphicMenuPanelData();
            rectGraphicView = UIGraphicsView.transform as RectTransform;
            UIGraphicControlContent.GenerateUIGrrphicsItem(UIGraphicItem,UIimg, rectGraphicView, mData.model);

            //订阅titleImg相关属性
            TitleImgLocalPos.Subscribe(TitleImgLocalPosChange);
            TitleImgActive.Subscribe(bo=> { if (bo) TitleImg.Show(); else TitleImg.Hide(); });
            TitleSprite.Subscribe(sprite => TitleImg.sprite = sprite);
            UILocalEditorButton.onClick.AddListener(()=> GetPather(UnityEngine.Application.streamingAssetsPath+Global.allGraphicsFillName));
        }



        void TitleImgLocalPosChange(Vector2 ve)
        {
            
            TitleImg.LocalPosition(ve);
        }

        /// <summary>
        /// 获得路径
        /// </summary>
        private void GetPather(string path)
        {

            OpenFileName openFileName = new OpenFileName();
            openFileName.structSize = Marshal.SizeOf(openFileName);

            openFileName.filter = "图片文件(*.jpg,*.png,*.bmp)\0*.jpg;*.png;*.bmp";
            openFileName.file = new string(new char[256]);
            openFileName.maxFile = openFileName.file.Length;
            openFileName.fileTitle = new string(new char[64]);
            openFileName.maxFileTitle = openFileName.fileTitle.Length;
            openFileName.initialDir = "d:\\";//默认路径
            openFileName.title = "选择图片";

            openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

            if (LocalDialog.GetSaveFileName(openFileName))
            {
                FileStreamLoadTexture(openFileName.file,path);
            }
            //创建窗口对象
            //OpenFileDialog ofd = new OpenFileDialog();

            ////操作设置----设置文件格式筛选
            //ofd.Filter = "所有文件(*.*)|*.*|png文件|*.png|jpg文件|*.jpg";

            ////操作设置----起始目录
            //ofd.InitialDirectory = "d:\\";

            ////打开文件夹目录，选择文件
            //DialogResult resurt = ofd.ShowDialog();

            //if (resurt == DialogResult.OK)
            //{
            //    //ofd.FileName是得到选择文件的地址 OpenFile是对它裁剪的入口

            //    FileStreamLoadTexture(ofd.FileName,path);
            //}

        }
        /// <summary>
        /// 文件流加载图片
        /// </summary>
        private void FileStreamLoadTexture(string fileName, string path)
        {
            try
            {
                //通过路径加载本地图片
                FileStream fs = new FileStream(fileName, FileMode.Open);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                Texture2D originalTex = new Texture2D(2, 2);
                var iSLoad = originalTex.LoadImage(buffer);
                originalTex.Apply();
                if (!iSLoad)
                {
                    Debug.Log("Texture存在但生成Texture失败");
                }
                //C:\Users\Lenovo\Pictures\程序员的桌面\code-wallpaper-15.jpg
                //string name = SchematicControl.GetEndStr(texPath, '\\');
                //name = SchematicControl.GetBeginStr(name, '.');
                originalTex.name = fileName;
                //GetTexture();
                File.WriteAllBytes(path + originalTex.name + ".png", originalTex.EncodeToPNG());
            }
            catch (Exception e)
            {
                e.Message.LogInfo();
            }
           
        }

        protected override void OnOpen(QFramework.IUIData uiData)
        {
        }
        
        protected override void OnShow()
        {
        }
        
        protected override void OnHide()
        {
        }
        
        protected override void OnClose()
        {
        }
    }
}
