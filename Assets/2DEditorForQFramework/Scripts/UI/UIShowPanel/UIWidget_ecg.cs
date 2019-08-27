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

            AssetNodeDataOnInit = true;
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
            //这种留 2 位小数的方法 迭代比较差（不适用 不是float 的报错）
            //不过这种 组件 属于特殊需求个别 组件 比较具有 单一性
             .ApplySelfTo(self => self.widget.TextDataList[0].content.ObserveOnMainThread().Subscribe(str => { if (A_voltage && str.IsNotNullAndEmpty()) A_voltage.text = CheckStr(str); else A_voltage.text = ""; }))
            .ApplySelfTo(self => self.widget.TextDataList[0].AssetNodeData.Subscribe(data => {
                if (data.IsNotNull())
                {
                    OnAssetNodeInit(data, mes => self.widget.TextDataList[0].content.Value = mes.Value);                   
                }
            }))
            .ApplySelfTo(self => self.widget.TextDataList[1].content.ObserveOnMainThread().Subscribe(str => { if (A_electric && str.IsNotNullAndEmpty()) A_electric.text = CheckStr(str); else A_electric.text = ""; }))
            .ApplySelfTo(self => self.widget.TextDataList[1].AssetNodeData.Subscribe(data => {
                if (data.IsNotNull())
                {
                    OnAssetNodeInit(data, mes => self.widget.TextDataList[1].content.Value = mes.Value);
                    //todo
                }
            }))
            .ApplySelfTo(self => self.widget.TextDataList[2].content.ObserveOnMainThread().Subscribe(str => { if (B_voltage && str.IsNotNullAndEmpty()) B_voltage.text = CheckStr(str); else B_voltage.text = ""; }))
            .ApplySelfTo(self => self.widget.TextDataList[2].AssetNodeData.Subscribe(data => {
                if (data.IsNotNull())
                {
                    OnAssetNodeInit(data, mes => self.widget.TextDataList[2].content.Value = mes.Value);
                    //todo                  
                }
            }))
             .ApplySelfTo(self => self.widget.TextDataList[3].content.ObserveOnMainThread().Subscribe(str => { if (B_electric && str.IsNotNullAndEmpty()) B_electric.text = CheckStr(str); else B_electric.text = ""; }))
            .ApplySelfTo(self => self.widget.TextDataList[3].AssetNodeData.Subscribe(data => {
                if (data.IsNotNull())
                {
                    OnAssetNodeInit(data, mes => self.widget.TextDataList[3].content.Value = mes.Value);
                    //todo
                }
            }))
             .ApplySelfTo(self => self.widget.TextDataList[4].content.ObserveOnMainThread().Subscribe(str => { if (C_voltage && str.IsNotNullAndEmpty()) C_voltage.text = CheckStr(str); else C_voltage.text = ""; }))
            .ApplySelfTo(self => self.widget.TextDataList[4].AssetNodeData.Subscribe(data => {
                if (data.IsNotNull())
                {
                    OnAssetNodeInit(data, mes => self.widget.TextDataList[4].content.Value = mes.Value);
                    //todo
                }
            }))
            .ApplySelfTo(self => self.widget.TextDataList[5].content.ObserveOnMainThread().Subscribe(str=> { if (C_electric && str.IsNotNullAndEmpty()) C_electric.text = CheckStr(str); else C_electric.text = ""; }))
            .ApplySelfTo(self => self.widget.TextDataList[5].AssetNodeData.Subscribe(data => {
                
                if (data.IsNotNull())
                {
                    OnAssetNodeInit(data, mes => self.widget.TextDataList[5].content.Value = mes.Value);                                      
                }
            }))
            .ApplySelfTo(self => self.widget.TextDataList[6].content.ObserveOnMainThread().Subscribe(str => { if (ElectricCurrenttTxt && str.IsNotNullAndEmpty()) ElectricCurrenttTxt.text = CheckStr(str);
                else ElectricCurrenttTxt.text ="";
            }))
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
                        ServerData.GetAssetNodeForID(data.Path, (str) =>
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                try
                                {
                                    AssetNode asset = SerializeHelper.FromJson<AssetNode>(str);
                                    if (asset.IsNull()) { data = null; self.widget.ColorInit(); }
                                    else
                                    {
                                        data.Id = asset.id;
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
                    }
                    else
                    {
                        ServerData.GetAssetNodeForID(data.Path, (str) =>
                        {
                           // Debug.Log("Bind + "+data.Path);
                            if (!string.IsNullOrEmpty(str))
                            {
                                //AssetNode asset = SerializeHelper.FromJson<AssetNode>(str);
                                //ecgTitle.text = asset.name;
                                ServerData.GetAssetNodesForDeviceId(data.Path, (va) =>
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
            }));
        }
        private void OnAssetNodeInit(WebSocketMessage data,Action<WebSocketMessage> act=null)
        {
            if (!AssetNodeDataOnInit)
            {
                ServerData.GetAssetNodeForID(data.Path, (str) =>
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        try
                        {
                            AssetNode asset = SerializeHelper.FromJson<AssetNode>(str);
                            data.Data = asset.value;
                            data.Id = asset.id;
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

        private string CheckStr(string str)
        {
            if (str.IndexOf('.') == -1) return str;
            int index= str.IndexOf('.') + 3;
            if (str.Length-1 < index) index--;
            return str.Substring(0, index);
        }

        //子物体绑定
        private void BindChild(List<AssetNode> assetsList)
        {
            if (assetsList.IsNull() || assetsList.Count < 1) return;
            for (int i = 0; i < assetsList.Count; i++)
            {
                if (assetsList[i].caption.Contains("A相电压"))
                    widget.TextDataList[0].SetAssetNodeData(new WebSocketMessage() { Path = assetsList[i].fullName, Data = assetsList[i].value, Id = assetsList[i].id, State = assetsList[i].state, Value = assetsList[i].value, });
                else if (assetsList[i].caption.Contains("A相电流"))
                    widget.TextDataList[1].SetAssetNodeData(new WebSocketMessage() { Path = assetsList[i].fullName, Data = assetsList[i].value, Id = assetsList[i].id, State = assetsList[i].state, Value = assetsList[i].value, });
                else if (assetsList[i].caption.Contains("B相电压"))
                    widget.TextDataList[2].SetAssetNodeData(new WebSocketMessage() { Path = assetsList[i].fullName, Data = assetsList[i].value, Id = assetsList[i].id, State = assetsList[i].state, Value = assetsList[i].value, });
                else if (assetsList[i].caption.Contains("B相电流"))
                {
                    //Debug.Log("B相电流:" + assetsList[i].value);
                    widget.TextDataList[3].SetAssetNodeData(new WebSocketMessage() { Path = assetsList[i].fullName, Data = assetsList[i].value, Id = assetsList[i].id, State = assetsList[i].state, Value = assetsList[i].value, });
                }
                else if (assetsList[i].caption.Contains("C相电压"))
                    widget.TextDataList[4].SetAssetNodeData(new WebSocketMessage() { Path = assetsList[i].fullName, Data = assetsList[i].value, Id = assetsList[i].id, State = assetsList[i].state, Value = assetsList[i].value, });
                else if (assetsList[i].caption.Contains("C相电流"))
                {
                  //  Debug.Log("C相电流:" + assetsList[i].id+ assetsList[i].value);
                    widget.TextDataList[5].SetAssetNodeData(new WebSocketMessage() { Path = assetsList[i].fullName, Data = assetsList[i].value, Id = assetsList[i].id, State = assetsList[i].state, Value = assetsList[i].value, });
                }
                else if (assetsList[i].catalogId.Contains("1005"))
                {
                    //Debug.Log("1005:"+assetsList[i].value);
                    widget.TextDataList[6].SetAssetNodeData(new WebSocketMessage() { Path = assetsList[i].fullName, Data = assetsList[i].value, Id = assetsList[i].id, State = assetsList[i].state, Value = assetsList[i].value, });
                }
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