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
	public partial class ArrangeContent : UIElement
	{
		private void Awake()
		{
            //groupBtn.onClick.AddListener(GroupBtnClick);
            //cancelGrpupBtn.onClick.AddListener(CancelGroupBtnClick);
            //alignBtn.onClick.AddListener(AlignBtnClick);
            //upLayerBtn.onClick.AddListener(UpLayerBtnClick);
            //downLayerBtn.onClick.AddListener(DownLayerBtnClick);
            //bringToFrontBtn.onClick.AddListener(BringToFrontBtnClick);
            //atTheBottomBtn.onClick.AddListener(AtTheBottomBtnClick);

            groupBtn.onClick.AddListener(()=>
            {
                GroupBtnClick();
                this.CloseItem();
            });
            cancelGrpupBtn.onClick.AddListener(() =>
            {
                CancelGroupBtnClick();
                this.CloseItem();
            });
            alignBtn.onClick.AddListener(() =>
            {
                AlignBtnClick();
                this.CloseItem();
            });
            upLayerBtn.onClick.AddListener(() =>
            {
                UpLayerBtnClick();
                this.CloseItem();
            });
            downLayerBtn.onClick.AddListener(() =>
            {
                DownLayerBtnClick();
                this.CloseItem();
            });
            bringToFrontBtn.onClick.AddListener(() =>
            {
                // BtnClickGlobal.AddRetreatSceneData();
                if (Global.OnSelectedGraphic.Value == null) return;
                Global.OnSelectedGraphic.Value.siblingType.Value = SiblingEditorType.UpEnd;
                this.CloseItem();
            });
            atTheBottomBtn.onClick.AddListener(() =>
            {
                if (Global.OnSelectedGraphic.Value == null) return;
                Global.OnSelectedGraphic.Value.siblingType.Value = SiblingEditorType.DonwEnd;
                this.CloseItem();
            });
        }

        void CloseItem()
        {
            this.transform.parent.Hide();
            this.Hide();
        }

		protected override void OnBeforeDestroy()
		{
		}

        private void OnDisable()
        {

        }

        #region ArrangeContent

        public static void GroupBtnClick()
        {

        }

        public static void CancelGroupBtnClick()
        {
            
        }

        public static void AlignBtnClick()
        {
           // Debug.Log("Current=>" + Global.OnSelectedGraphic.Value.siblingIndex.Value);
        }

        public void UpLayerBtnClick()
        {
           // BtnClickGlobal.AddRetreatSceneData();
            if (Global.OnSelectedGraphic.Value == null) return;
            Global.OnSelectedGraphic.Value.siblingType.Value = SiblingEditorType.UPOne;
        }

        public  void DownLayerBtnClick()
        {
           // BtnClickGlobal.AddRetreatSceneData();
            if (Global.OnSelectedGraphic.Value == null) return;
            Global.OnSelectedGraphic.Value.siblingType.Value = SiblingEditorType.DonwOne;
        }

        public  void BringToFrontBtnClick()
        {
          

        }

        public  void AtTheBottomBtnClick()
        {
            //BtnClickGlobal.AddRetreatSceneData();
            

        }

        public  int GetChildCount()
        {
            switch (Global.OnSelectedGraphic.Value.graphicType)
            {
                case GraphicType.Image:
                    return Global.imageParent.childCount;
                case GraphicType.Text:
                    return Global.textParent.childCount;
                case GraphicType.Line:
                    return Global.LineParent.childCount;
                default:
                    return 1;
            }

        }


        #endregion
    }
}