//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QFramework.TDE
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    
    
    public partial class UIServerDatasMenuPanel
    {
        
        public const string NAME = "UIServerDatasMenuPanel";
        
        [SerializeField()]
        public UIServerDataItem UIServerDataItem;
        
        [SerializeField()]
        public UIServerDataPathItem UIServerDataPathItem;
        
        [SerializeField()]
        public UIServerDatasCloseButton UIServerDatasCloseButton;
        
        [SerializeField()]
        public RectTransform UIServerDatasContent;
        
        [SerializeField()]
        public Button UIConfirmButton;
        
        [SerializeField()]
        public Button UIResetutton;
        
        private UIServerDatasMenuPanelData mPrivateData = null;
        
        public UIServerDatasMenuPanelData mData
        {
            get
            {
                return mPrivateData ?? (mPrivateData = new UIServerDatasMenuPanelData());
            }
            set
            {
                mUIData = value;
                mPrivateData = value;
            }
        }
        
        protected override void ClearUIComponents()
        {
            UIServerDataItem = null;
            UIServerDataPathItem = null;
            UIServerDatasCloseButton = null;
            UIServerDatasContent = null;
            UIConfirmButton = null;
            UIResetutton = null;
            mData = null;
        }
    }
}
