/****************************************************************************
 * 2019.5 LAPTOP-R0ONNKOC
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TDE;
using UnityEngine.EventSystems;
using UniRx;

namespace QFramework.TDE
{
    public partial class UIWidget_ecg : UIWidget
    {

        List<T_Image> tImages;
        List<T_Text> tTexts ;
        public override void Init(TSceneData model, T_Widget widget)
        {
            base.Init(model, widget);
            tImages = new List<T_Image>(1);
            tTexts = new List<T_Text>(7);

            for (int i = 0; i < tImages.Capacity; i++){tImages.Add(new T_Image());}
            for (int i = 0; i < tTexts.Capacity; i++){tTexts.Add(new T_Text());}

          if(widget.ImageDataList==null)  widget.ImageDataList = tImages;
          if (widget.TextDataList == null) widget.TextDataList = tTexts;

            SubscribeInit();
            //编辑面板初始化
            EditorBoxInit(widget);
        }
        //  值订阅
        protected override  void SubscribeInit()
        {
            base.SubscribeInit();

            this.ApplySelfTo(self => self.widget.isSelected.Subscribe(bo =>
            {
                if (bo) { if (self.UIEditorBox) self.UIEditorBox.Show(); }
                else { if (self.UIEditorBox) self.UIEditorBox.Hide(); }
            }))
            .ApplySelfTo(self => self.widget.ImageDataList[0].AssetNodeData.Subscribe(data => {
                if (data.IsNotNull())
                {
                    OnAssetNodeInit(data);
                    //todo
                }
            }))
             .ApplySelfTo(self => self.widget.TextDataList[0].content.ObserveOnMainThread().Subscribe(str => { if (A_voltage) A_voltage.text = str; }))
            .ApplySelfTo(self => self.widget.TextDataList[0].AssetNodeData.Subscribe(data => {
                if (data.IsNotNull())
                {
                    OnAssetNodeInit(data, mes => self.widget.TextDataList[0].content.Value = mes.Value);                   
                }
            }))
            .ApplySelfTo(self => self.widget.TextDataList[1].content.ObserveOnMainThread().Subscribe(str => { if (A_electric) A_electric.text = str; }))
            .ApplySelfTo(self => self.widget.TextDataList[1].AssetNodeData.Subscribe(data => {
                if (data.IsNotNull())
                {
                    OnAssetNodeInit(data, mes => self.widget.TextDataList[1].content.Value = mes.Value);
                    //todo
                }
            }))
            .ApplySelfTo(self => self.widget.TextDataList[2].content.ObserveOnMainThread().Subscribe(str => { if (B_voltage) B_voltage.text = str; }))
            .ApplySelfTo(self => self.widget.TextDataList[2].AssetNodeData.Subscribe(data => {
                if (data.IsNotNull())
                {
                    OnAssetNodeInit(data, mes => self.widget.TextDataList[2].content.Value = mes.Value);
                    //todo                  
                }
            }))
             .ApplySelfTo(self => self.widget.TextDataList[3].content.ObserveOnMainThread().Subscribe(str => { if (B_electric) B_electric.text = str; }))
            .ApplySelfTo(self => self.widget.TextDataList[3].AssetNodeData.Subscribe(data => {
                if (data.IsNotNull())
                {
                    OnAssetNodeInit(data, mes => self.widget.TextDataList[3].content.Value = mes.Value);
                    //todo
                }
            }))
             .ApplySelfTo(self => self.widget.TextDataList[4].content.ObserveOnMainThread().Subscribe(str => { if (C_voltage) C_voltage.text = str; }))
            .ApplySelfTo(self => self.widget.TextDataList[4].AssetNodeData.Subscribe(data => {
                if (data.IsNotNull())
                {
                    OnAssetNodeInit(data, mes => self.widget.TextDataList[4].content.Value = mes.Value);
                    //todo
                }
            }))
            .ApplySelfTo(self => self.widget.TextDataList[5].content.ObserveOnMainThread().Subscribe(str=> { if (C_electric) C_electric.text = str; }))
            .ApplySelfTo(self => self.widget.TextDataList[5].AssetNodeData.Subscribe(data => {
                if (data.IsNotNull())
                {
                    OnAssetNodeInit(data, mes => self.widget.TextDataList[5].content.Value = mes.Value);                                      
                }
            }))
            .ApplySelfTo(self => self.widget.TextDataList[6].content.ObserveOnMainThread().Subscribe(str => { if (ElectricCurrenttTxt) ElectricCurrenttTxt.text = str; }))
            .ApplySelfTo(self => self.widget.TextDataList[6].AssetNodeData.Subscribe(data => {
                if (data.IsNotNull())
                {
                    OnAssetNodeInit(data,mes=> self.widget.TextDataList[6].content.Value= mes.Value);
                    //todo
                }
            }))
            .ApplySelfTo(self => self.widget.AssetNodeData.Subscribe(data => {
                if (data.IsNotNull())
                {
                    if (!AssetNodeDataOnInit)
                    {
                        ServerData.GetAssetNodeForID(data.Id, (str) =>
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                try
                                {
                                    AssetNode asset = SerializeHelper.FromJson<AssetNode>(str);
                                    if (asset.IsNull()) { data = null; self.widget.ColorInit(); }
                                    else
                                    {
                                        data.Data = asset.value;
                                        data.State = asset.state;
                                        data.Value = asset.value;
                                        //ecgTitle.text = asset.caption;
                                    }
                                }
                                catch (Exception e)
                                {

                                    Log.I(e.Message);
                                }
                                
                            }
                        });
                        AssetNodeDataOnInit = true;
                    }
                    else
                    {
                        ServerData.GetAssetNodeForID(data.Id, (str) =>
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                //AssetNode asset = SerializeHelper.FromJson<AssetNode>(str);
                                //ecgTitle.text = asset.name;
                                ServerData.GetAssetNodesForDeviceId(data.Id, (va) =>
                                {
                                    if (!string.IsNullOrEmpty(va))
                                    {
                                        try
                                        {
                                            List<AssetNode> queryAs = SerializeHelper.FromJson<List<AssetNode>>(va);
                                            BindChild(queryAs);
                                        }
                                        catch (Exception e)
                                        {
                                            Log.I(e.Message);
                                            Log.I("序列化失败？");
                                        }
                                    }
                                });
                            }
                        });
                    }
                }
                else AssetNodeDataOnInit = true;
            }));
        }
        private void OnAssetNodeInit(WebSocketMessage data,Action<WebSocketMessage> act=null)
        {
            if (!AssetNodeDataOnInit)
            {
                ServerData.GetAssetNodeForID(data.Id, (str) =>
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        try
                        {
                            AssetNode asset = SerializeHelper.FromJson<AssetNode>(str);
                            data.Data = asset.value;
                            data.State = asset.state;
                            data.Value = asset.value;
                            act?.Invoke(data);
                        }
                        catch (Exception e)
                        {
                            Log.I(e.Message);
                            Log.I("序列化失败？");
                        }
                    }
                });
            }else act?.Invoke(data);
        }

        //子物体绑定
        private void BindChild(List<AssetNode> assetsList)
        {
            if (assetsList.IsNull() || assetsList.Count < 1) return;
            for (int i = 0; i < assetsList.Count; i++)
            {
                if (assetsList[i].caption.Contains("A相电压"))
                    widget.TextDataList[0].SetAssetNodeData(new WebSocketMessage() { Data = assetsList[i].value, Id = assetsList[i].id, State = assetsList[i].state, Value = assetsList[i].value, });               
                else if (assetsList[i].caption.Contains("A相电流"))
                    widget.TextDataList[1].SetAssetNodeData(new WebSocketMessage() { Data = assetsList[i].value, Id = assetsList[i].id, State = assetsList[i].state, Value = assetsList[i].value, });
                else if (assetsList[i].caption.Contains("B相电压"))
                    widget.TextDataList[2].SetAssetNodeData(new WebSocketMessage() { Data = assetsList[i].value, Id = assetsList[i].id, State = assetsList[i].state, Value = assetsList[i].value, });
                else if (assetsList[i].caption.Contains("B相电流"))
                    widget.TextDataList[3].SetAssetNodeData(new WebSocketMessage() { Data = assetsList[i].value, Id = assetsList[i].id, State = assetsList[i].state , Value = assetsList[i].value, });
                else if (assetsList[i].caption.Contains("C相电压"))
                    widget.TextDataList[4].SetAssetNodeData(new WebSocketMessage() { Data = assetsList[i].value, Id = assetsList[i].id, State = assetsList[i].state, Value = assetsList[i].value, });
                else if (assetsList[i].caption.Contains("C相电流"))
                    widget.TextDataList[5].SetAssetNodeData(new WebSocketMessage() { Data = assetsList[i].value, Id = assetsList[i].id, State = assetsList[i].state, Value = assetsList[i].value, });            
                else if (assetsList[i].catalogId.Contains("1005"))
                    widget.TextDataList[6].SetAssetNodeData(new WebSocketMessage() { Data = assetsList[i].value, Id = assetsList[i].id, State = assetsList[i].state, Value = assetsList[i].value, });
            }           
        }
        //待优化 设计方式不太理想
        private void EditorBoxInit(T_Graphic model)
        {
            UIRotate uRot = UIRotate.GetComponent<UIRotate>();
            uRot.Init(model, transform);

            UICornerDrag LeftDownUIDrag = UILeftDown.GetComponent<UICornerDrag>();
            LeftDownUIDrag.Init(model, new Corner(UILeftUP.transform, UIRigghtUP.transform
                , UILeftDown.transform, UIRightDown.transform, parentRT
                ));
            UICornerDrag LeftUpUIDrag = UILeftUP.GetComponent<UICornerDrag>();
            LeftUpUIDrag.Init(model, new Corner(UILeftUP.transform, UIRigghtUP.transform
                , UILeftDown.transform, UIRightDown.transform, parentRT
                ));
            UICornerDrag RigghtUpUIDrag = UIRigghtUP.GetComponent<UICornerDrag>();
            RigghtUpUIDrag.Init(model, new Corner(UILeftUP.transform, UIRigghtUP.transform
                , UILeftDown.transform, UIRightDown.transform, parentRT
                ));
            UICornerDrag RightDownUIDrag = UIRightDown.GetComponent<UICornerDrag>();
            RightDownUIDrag.Init(model, new Corner(UILeftUP.transform, UIRigghtUP.transform
                , UILeftDown.transform, UIRightDown.transform, parentRT
                ));
        }
    }
}