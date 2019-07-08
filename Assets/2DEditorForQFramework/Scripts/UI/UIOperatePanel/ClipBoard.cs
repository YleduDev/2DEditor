/****************************************************************************
 * 2019.4 DESKTOP-IVCS95Q
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TDE;

namespace QFramework.TDE
{
	public partial class ClipBoard : UIElement
	{
		private void Awake()
		{
            pasteBtn.onClick.AddListener(EditorItem.PasteBtnClick);

            copyBtn.onClick.AddListener(EditorItem.CopyBtnClick);

            shearBtn.onClick.AddListener(EditorItem.ShearBtnClick);

        }


        #region 按钮方法由静态引用

        //bool isCopy = true;
        //Vector2 offsetPos = new Vector2(10, -10);
        //void BtnsClickRegister()
        //{
        //    pasteBtn.onClick.AddListener(() =>
        //    {
        //        if (isCopy)
        //        {
        //            //if (Global.OnChecksGraphic.IsNotNull())
        //            //{
        //            //    Global.OnChecksGraphic.ForEach(item =>
        //            //    {
        //            //        Global.currentSceneData.Value.Add(CreateGraphic(item));
        //            //    });
        //            //}

        //            if (Global.OnSelectedGraphic.Value.IsNotNull())
        //                Global.currentSceneData.Value.Add(CreateGraphic(Global.OnSelectedGraphic.Value));
        //        }
        //        else
        //        {
        //            //Global.OnChecksGraphic.ForEach(item =>
        //            //{
        //            //    item.localPos.Value += offsetPos;
        //            //});
        //            if (Global.OnSelectedGraphic.Value.IsNotNull())
        //                Global.OnSelectedGraphic.Value.localPos.Value += offsetPos;
        //        }
        //    });

        //    shearBtn.onClick.AddListener(() =>
        //    {
        //        isCopy = false;
        //    });

        //    copyBtn.onClick.AddListener(() =>
        //    {
        //        isCopy = true;

        //    });
        //}

        //T_Graphic CreateGraphic(T_Graphic graphic)
        //{
        //    T_Graphic newGraphic;
        //    switch (graphic.graphicType)
        //    {
        //        case GraphicType.Image:
        //            newGraphic = new T_Image((T_Image)graphic);
        //            break;
        //        case GraphicType.Text:
        //            newGraphic = new T_Text((T_Text)graphic);
        //            break;
        //        case GraphicType.Line:
        //            newGraphic = new T_Line((T_Line)graphic);
        //            break;
        //        default:
        //            return null;
        //    }
        //    newGraphic.localPos.Value += offsetPos;
        //    return newGraphic;
        //}

        #endregion
        protected override void OnBeforeDestroy()
		{
		}
	}
}