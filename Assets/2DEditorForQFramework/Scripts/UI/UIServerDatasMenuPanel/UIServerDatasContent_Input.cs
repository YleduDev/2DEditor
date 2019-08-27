/****************************************************************************
 * 2019.4 LAPTOP-R0ONNKOC
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UniRx;
using TDE;

namespace QFramework.TDE
{
	public partial class UIServerDatasContent_Input : UIElement
	{
        public  AssetCheckType check= AssetCheckType.name;
        public AssetKindType kind= AssetKindType.device;

        ServerData model;

        UIServerDataItem prefab;

        List< UIServerDataItem> UIServerDataItems=new List<UIServerDataItem>();
        SimpleObjectPool<UIServerDataItem> uiServerDataItemPool;
      //  public ReactiveProperty<AssetNode> assetNode = new ReactiveProperty<AssetNode>();
        public ReactiveProperty<WebSocketMessage> Message = new ReactiveProperty<WebSocketMessage>();
        private void Awake(){}
		protected override void OnBeforeDestroy(){}

        /// <summary>
        /// 输入框数据发生改变回调的方法
        /// </summary>
        /// <param name="id"></param>
        public void InputDataChange( string id) 
        {
            //清除子物体
            HeidChild();
            id = id.Trim();
            if (id.IsNullOrEmpty()) return;
           model.GetAssetForSizer(kind,check,id, (assets)=> {
               if (assets.IsNull()) return;
               //assetNode.Value = asset;
               //生成 id产物 
               for (int i = 0; i < assets.Count; i++)
               {
                   //Debug.Log(assets[i].fullName);
                   //Debug.Log(assets[i].id);

                   UIServerDataItem UIServerDataItem = uiServerDataItemPool.Allocate()
                 .ApplySelfTo(self => self.transform.SetParent(transform, false))
                 .ApplySelfTo(self => self.assetNode.Value = assets[i])
                 .ApplySelfTo(self => self.Message.Subscribe(vaule => { if (vaule.IsNotNull()) { Message.Value = vaule; } }))
                 .ApplySelfTo(self => self.Init())
                 .ApplySelfTo(self => self.Show());
                   UIServerDataItems.Add(UIServerDataItem);
               }                         
           });           
        }

        internal void Init(ServerData model,UIServerDataItem UIServerDataItem)
        {
            this.model = model;
            this.prefab = UIServerDataItem;
            uiServerDataItemPool = new SimpleObjectPool<UIServerDataItem>(() => prefab.Instantiate().ApplySelfTo(self=>self.transform.SetParent(transform,false)), item => item.Hide(), 100);
        }

        void HeidChild()
        {
            if (UIServerDataItems.Count > 0)
            {
                for (int i = 0; i < UIServerDataItems.Count; i++)
                {
                    uiServerDataItemPool.Recycle(UIServerDataItems[i]);
                }
            }
        }
    }
}