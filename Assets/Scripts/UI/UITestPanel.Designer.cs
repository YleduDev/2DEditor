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
    
    
    public partial class UITestPanel
    {
        
        public const string NAME = "UITestPanel";
        
        [SerializeField()]
        public Button Button;
        
        [SerializeField()]
        public Button ButtonError;
        
        [SerializeField()]
        public Button ButtonWarningg;
        
        [SerializeField()]
        public Button ButtonNormal;
        
        private UITestPanelData mPrivateData = null;
        
        public UITestPanelData mData
        {
            get
            {
                return mPrivateData ?? (mPrivateData = new UITestPanelData());
            }
            set
            {
                mUIData = value;
                mPrivateData = value;
            }
        }
        
        protected override void ClearUIComponents()
        {
            Button = null;
            ButtonError = null;
            ButtonWarningg = null;
            ButtonNormal = null;
            mData = null;
        }
    }
}
