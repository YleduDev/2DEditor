/****************************************************************************
 * 2019.4 DESKTOP-IVCS95Q
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Runtime.InteropServices;
using TDE;

namespace QFramework.TDE
{
    public partial class DocumentItem : UIElement
    {
        public void Init()
        {
            newCreateBtn.onClick.AddListener(NewCreateBtnClick);

            openBtn.onClick.AddListener(OpenBtnClick);

            RecentlyOpenBtn.onClick.AddListener(RecentlyOpenBtnClick);

            saveBtn.onClick.AddListener(SaveBtnClick);

            saveAsBtn.onClick.AddListener(SaveAsBtnClick);

            uploadBtn.onClick.AddListener(UploadBtnClick);

            printBtn.onClick.AddListener(PrintBtnClick);

            quitBtn.onClick.AddListener(QuitBtnClick);

            CanvasScalePanel.Init(this);
        }



        public void NewCreateBtnClick()
        {
           Global.currentSceneData.Value= TSceneData.Load("");
        }

        public void OpenBtnClick()
        {
            //OpenFileWin();
            UIMgr.OpenPanel<UIScenesScrollViewPanel>();
            this.Hide();
        }

        public void RecentlyOpenBtnClick()
        {
            //Global.currentSceneData.Value = TSceneData.Load(PlayerPrefs.GetString("OnDrawing", ""));
        }

        public void SaveBtnClick()
        {
          
        }

        public void SaveAsBtnClick()
        {
            SaveAsFile();
            this.Hide();
        }

        public void UploadBtnClick() {
            UIMgr.OpenPanel<UIUPloadPanel>();
        }

        public void PrintBtnClick() { }

        public void QuitBtnClick() { }



        #region 外部系统打开方法

        public void OpenFileWin()
        {

            //GameObject.FindWithTag("moviePlayer").GetComponent<VideoPlayer>().Pause();

            OpenFileName ofn = new OpenFileName();

            ofn.structSize = Marshal.SizeOf(ofn);

            ofn.filter = "All Files\0*.*\0\0";

            ofn.file = new string(new char[256]);

            ofn.maxFile = ofn.file.Length;

            ofn.fileTitle = new string(new char[64]);

            ofn.maxFileTitle = ofn.fileTitle.Length;
            string path = Application.streamingAssetsPath;
            path = path.Replace('/', '\\');
            //默认路径  
            ofn.initialDir = path;
            //ofn.initialDir = "D:\\MyProject\\UnityOpenCV\\Assets\\StreamingAssets";  
            ofn.title = "Open Project";

            ofn.defExt = "JPG";//显示文件的类型  
                               //注意 一下项目不一定要全选 但是0x00000008项不要缺少  
            ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR  

            if (LocalDialog.GetOpenFileName(ofn))
            {
                //Debug.Log("Selected file with full path: {0}" + ofn.file);
            }
            //此处更改了大部分答案的协程方法，在这里是采用unity的videoplayer.url方法播放视频；
            //
            /*而且我认为大部分的其他答案，所给的代码并不全，所以，想要其他功能的人，可以仿照下面的代码，直接在此类中写功能。
            //*/


        }

        public void SaveAsFile()
        {

            OpenFileName openFileName = new OpenFileName();
            openFileName.structSize = Marshal.SizeOf(openFileName);
            //openFileName.filter = "(*." + type + ")\0*." + type + "";
            openFileName.filter = "All Files\0*.*\0\0";
            openFileName.file = new string(new char[256]);
            openFileName.maxFile = openFileName.file.Length;
            openFileName.fileTitle = new string(new char[64]);
            openFileName.maxFileTitle = openFileName.fileTitle.Length;
            openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默认路径
            openFileName.title = "选择文件";
            openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

            if (LocalDialog.GetSaveFileName(openFileName))//点击系统对话框框保存按钮
            {
                //TODO

            }
        }

        #endregion


        #region 鼠标Enter，Exit方法

        public void RecentlyOpenContentShow()
        {
            recentlyOpenContent.transform.position = RecentlyOpenBtn.transform.position;
            recentlyOpenContent.Show();
        }

        public void RecentlyOpenContentHide()
        {
            recentlyOpenContent.Hide();
        }

        #endregion


        private void OnDisable()
        {
            RecentlyOpenContentHide();
        }

        #region 快捷键

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {

                if (Input.GetKey(KeyCode.N))
                {
                    Show();
                }

                if (Input.GetKey(KeyCode.O))
                {
                    OpenFileWin();
                }

                if (Input.GetKey(KeyCode.S))
                {

                }

                if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKey(KeyCode.S))
                {
                    SaveAsFile();
                }

                if (Input.GetKey(KeyCode.P))
                {

                }

                if (Input.GetKey(KeyCode.Q))
                {

                }
            }



        }

        #endregion

        protected override void OnBeforeDestroy()
        {
        }
    }
}