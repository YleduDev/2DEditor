/****************************************************************************
 * 2019.4 DESKTOP-IVCS95Q
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TDE;
using UniRx;

namespace QFramework.TDE
{
    public partial class Operation : UIElement
    {
        private void Awake()
        {
        }
        public void Init()
        {
            //saveBtn.onClick.AddListener(SaveBtnClick);
            //restoreBtn.onClick.AddListener(RestoreBtnClick);
            //deleteBtn.onClick.AddListener(DeleteBtnClick);
            //forwardBtn.onClick.AddListener(ForwardBtnClick);
             //uploadBtn.onClick.AddListener(UploadBtnClick);
          // retreatBtn.onClick.AddListener(RetreatBtnClick);


            BtnClickGlobal.ForwardSceneData.ObserveCountChanged().Subscribe(count =>
            {
                if (count == 0)
                {
                    Debug.Log("count=>Forward");
                    forwardBtn.interactable = false;
                }
                else
                {
                    forwardBtn.interactable = true;
                }
            });

            BtnClickGlobal.RetreatSceneData.ObserveCountChanged().Subscribe(count =>
            {
                if (count == 0)
                {
                    Debug.Log("count=>Retreat");
                    retreatBtn.interactable = false;
                    restoreBtn.interactable = false;
                }
                else
                {
                    retreatBtn.interactable = true ;
                    restoreBtn.interactable = true;
                }
            });        
        }

        public void SaveBtnClick()
        {
            Global.currentSceneData.Value.Save();
        }

        public void RestoreBtnClick()
        {
            BtnClickGlobal.GetRestoreSceneData();
        }

        public void DeleteBtnClick()
        {
            BtnClickGlobal.AddRetreatSceneData();
            Global.currentSceneData.Value.Remove(Global.OnSelectedGraphic.Value);
        }

        public void ForwardBtnClick()
        {
            Debug.Log("Forward");
            BtnClickGlobal.GetForwardSceneData();
        }

        public void UploadBtnClick()
        {

        }

        public void RetreatBtnClick()
        {
            Debug.Log("Retreat");
            BtnClickGlobal.GetRetreatSceneData();
        }


		protected override void OnBeforeDestroy()
		{
		}
	}
}