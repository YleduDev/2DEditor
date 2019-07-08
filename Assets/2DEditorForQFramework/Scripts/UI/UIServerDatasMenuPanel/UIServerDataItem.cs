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
	public partial class UIServerDataItem : UIElement
	{
        public ReactiveProperty<AssetNode> assetNode=new ReactiveProperty<AssetNode>();
        public ReactiveProperty<WebSocketMessage> Message = new ReactiveProperty<WebSocketMessage>();
        Toggle toggle;
        public void Init()
        {
            assetNode.Subscribe(asset => txt.text = asset.name + "(" + asset.kindStr + ")");
            toggle = GetComponent<Toggle>();
            ToggleGroup group = transform.parent.GetComponent<ToggleGroup>();
            if (toggle) toggle.group = group;
             toggle?.onValueChanged.AddListener(ob => {
                 Message.Value = ob ? new WebSocketMessage()
                 {
                     Id = assetNode.Value.id,
                     State = assetNode.Value.state,
                     Data = assetNode.Value.valueStr,
                     Value= assetNode.Value.value
                 } : null;
                 toggle.targetGraphic.color = ob ? new Color(0,0,255,155): Color.white;
             });
        }

		private void Awake()
		{
		}

        public void HeidInit()
        {
            gameObject.Hide();
            toggle.isOn = false;
        }
		protected override void OnBeforeDestroy()
		{
		}
	}
}