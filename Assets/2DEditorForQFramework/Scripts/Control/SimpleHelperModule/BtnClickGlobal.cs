using QFramework;
using System.Collections;
using System.Collections.Generic;
using TDE;
using UniRx;
using UnityEngine;

using System;
using UnityEngine.UI;
public class BtnClickGlobal{

    #region TypeImgShow

    public static Dictionary<string, Image> fontDic = new Dictionary<string, Image>();


    public static void BtnImgsHide()
    {
        fontDic.ForEach(img =>
        {
            img.Value.Hide();
        });
    }

    public static void BtnImgsHide(string typeName)
    {
        if (fontDic.ContainsKey(typeName))
        {
            fontDic[typeName].Hide();
        }
    }

    public static void BtnImgShow(string typeName)
    {
        if(fontDic.ContainsKey(typeName))
           fontDic[typeName].LocalPosition(Vector3.zero).Show();
    }


    public static void TextBtnImgsShow(T_Text text)
    {
        BtnImgsHide();

        if (text.fontStyle.Value == FontStyleSelf.BoldAndItalic)
        {
            BtnImgShow(FontStyleSelf.Bold.ToString());
            BtnImgShow(FontStyleSelf.Italic.ToString());
        }
        BtnImgShow(text.fontStyle.Value.ToString());

        BtnImgShow(text.paragraphHorizontalAlignMentType.Value.ToString());
        
        BtnImgShow(text.paragraphVerticalAlignmentType.Value.ToString());
    }
    #endregion


    #region     TextBtnsShow

    public static List<Selectable> textBtnsList = new List<Selectable>();

    public static void TextBtnsShow(bool interactable = true)
    {
        textBtnsList.ForEach(item=>
        {
            item.interactable = interactable;
        });
    }

    #endregion


    #region MyRegion

    //public static Queue<string> sceneDataQueue = new Queue<string>();

    //public static Stack<string> sceneDataStack = new Stack<string>();

    //public static ReactiveCollection<string> sta = new ReactiveCollection<string>(sceneDataQueue);


    public static ReactiveCollection<string> ForwardSceneData = new ReactiveCollection<string>();

    public static ReactiveCollection<string> RetreatSceneData = new ReactiveCollection<string>();

    public static ReactiveProperty<string> RestoreSceneData=new ReactiveProperty<string>();

    public static void AddForwardSceneData()
    {
        if (Global.currentSceneData.Value.IsNotNull())
            ForwardSceneData.Add(Global.currentSceneData.Value.Save());
    }

    public static void AddRetreatSceneData(bool isClearForward=true)
    {
        if (Global.currentSceneData.Value.IsNotNull())
            RetreatSceneData.Add(Global.currentSceneData.Value.Save());
        if (isClearForward) ForwardSceneData.Clear();
    }

    public static void GetForwardSceneData()
    {
        AddRetreatSceneData(false);

        string temp = ForwardSceneData[ForwardSceneData.Count - 1];
        Global.currentSceneData.Value = TSceneData.Load(temp);
        ForwardSceneData.Remove(temp);
    }

    public static void GetRetreatSceneData()
    {
        AddForwardSceneData();

        string temp= RetreatSceneData[RetreatSceneData.Count - 1];
        Global.currentSceneData.Value = TSceneData.Load(temp);
        RetreatSceneData.Remove(temp);
    }

    public static void GetRestoreSceneData()
    {
        ForwardSceneData.Clear();
        RetreatSceneData.Clear();
        Global.currentSceneData.Value = TSceneData.Load(RestoreSceneData.Value);
    }
    #endregion

    #region EditorItem


    public static void RestoreBtnClick()
    {

    }

    public static void ForwardBtnClick()
    {

    }

    public static void retreatBtnClick()
    {

    }

    public static bool isCopy = true;
    static Vector2 offsetPos = new Vector2(10, -10);
    public static void CopyBtnClick()
    {
        isCopy = true;
    }

    public static void PasteBtnClick()
    {
        if (isCopy)
        {
            //if (Global.OnChecksGraphic.IsNotNull())
            //{
            //    Global.OnChecksGraphic.ForEach(item =>
            //    {
            //        Global.currentSceneData.Value.Add(CreateGraphic(item));
            //    });
            //}

            if (Global.OnSelectedGraphic.Value.IsNotNull())
                Global.currentSceneData.Value.Add(CreateGraphic(Global.OnSelectedGraphic.Value));
        }
        else
        {
            //Global.OnChecksGraphic.ForEach(item =>
            //{
            //    item.localPos.Value += offsetPos;
            //});
            if (Global.OnSelectedGraphic.Value.IsNotNull())
                Global.OnSelectedGraphic.Value.localPos.Value += offsetPos;
        }
    }

    public static void ShearBtnClick()
    {
        isCopy = false;
    }
    public static void DeleteBtnClick()
    {

    }

    public static void ArrangeBtnClick()
    {

    }
    #endregion

    #region ArrangeContent

    public static void GroupBtnClick()
    {

    }

    public static void CancelGroupBtnClick()
    {

    }

    public static void AlignBtnClick()
    {

    }

    public static void UpLayerBtnClick()
    {

    }

    public static void DownLayerBtnClick()
    {

    }

    public static void BringToFrontBtnClick()
    {

    }

    public static void AtTheBottomBtnClick()
    {

    }

    static T_Graphic CreateGraphic(T_Graphic graphic)
    {
        T_Graphic newGraphic;
        switch (graphic.graphicType)
        {
            case GraphicType.Image:
                newGraphic = new T_Image((T_Image)graphic);
                break;
            case GraphicType.Text:
                newGraphic = new T_Text((T_Text)graphic);
                break;
            case GraphicType.Line:
                newGraphic = new T_Line((T_Line)graphic);
                break;
            default:
                return null;
        }
        newGraphic.localPos.Value += offsetPos;
        return newGraphic;
    }
    #endregion


}
