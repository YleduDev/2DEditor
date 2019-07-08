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
                BringToFrontBtnClick();
                this.CloseItem();
            });
            atTheBottomBtn.onClick.AddListener(() =>
            {
                BringToFrontBtnClick();
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
            Debug.Log("Current=>" + Global.OnSelectedGraphic.Value.siblingIndex.Value);
        }

        public void UpLayerBtnClick()
        {
            BtnClickGlobal.AddRetreatSceneData();
            UpOrDownLayer(1);
        }

        public  void DownLayerBtnClick()
        {
            BtnClickGlobal.AddRetreatSceneData();
            UpOrDownLayer(-1);
        }

        public  void BringToFrontBtnClick()
        {
            BtnClickGlobal.AddRetreatSceneData();
            UpOrDownLayer(0,true);
        }

        public  void AtTheBottomBtnClick()
        {
            BtnClickGlobal.AddRetreatSceneData();
            UpOrDownLayer(0,true,false);
        }

        public  void UpOrDownLayer(int offset = 0, bool isTopORBottom = false, bool isTop = true)
        {
            if (Global.OnSelectedGraphic.Value.IsNull()) return;
            if (isTopORBottom)
            {
                if (isTop)
                {
                    Global.OnSelectedGraphic.Value.siblingIndex.Value = GetChildCount() - 1;
                }
                else
                {
                    Global.OnSelectedGraphic.Value.siblingIndex.Value = 0;
                }
            }
            else
            {
                int temp = Global.OnSelectedGraphic.Value.siblingIndex.Value;
                Global.OnSelectedGraphic.Value.siblingIndex.Value = Mathf.Clamp(temp + offset, 0, GetChildCount() - 1);
            }
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