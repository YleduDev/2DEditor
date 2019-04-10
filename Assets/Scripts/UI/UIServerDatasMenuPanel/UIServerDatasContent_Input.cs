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
        ServerData model;
        UIServerDataItem prefab;
        UIServerDataItem UIServerDataItem;
        public ReactiveProperty<AssetNode> assetNode = new ReactiveProperty<AssetNode>();
        public ReactiveProperty<WebSocketMessage> Message = new ReactiveProperty<WebSocketMessage>();
        private void Awake()
		{
		}

		protected override void OnBeforeDestroy()
		{
		}

        public void InputDataChange( string id)
        {
            //清除子物体
            HeidChild();

            var asset = model.GetAssetForId(id);
            if (asset.IsNull()) return;
            assetNode.Value = asset;
            //生成 id产物
            if (!UIServerDataItem)
                UIServerDataItem = prefab.Instantiate()
                .ApplySelfTo(self => self.transform.SetParent(transform, false))
                .ApplySelfTo(self => assetNode.Subscribe(value => self.assetNode.Value = value))
                .ApplySelfTo(self => self.Message.Subscribe(vaule => Message.Value = vaule))
                .ApplySelfTo(self => self.Init())
                .ApplySelfTo(self => self.Show());
          else UIServerDataItem
                .ApplySelfTo(self => self.Show());
        }

        internal void Init(ServerData model,UIServerDataItem UIServerDataItem)
        {
            this.model = model;
            this.prefab = UIServerDataItem;
        }

        void HeidChild()
        {
            if (UIServerDataItem)UIServerDataItem.HeidInit();
        }
    }
}