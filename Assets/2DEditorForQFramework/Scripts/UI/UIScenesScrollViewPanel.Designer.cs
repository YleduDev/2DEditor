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
    
    
    public partial class UIScenesScrollViewPanel
    {
        
        public const string NAME = "UIScenesScrollViewPanel";
        
        [SerializeField()]
        public UISceneDataToggle UISceneDataToggle;
        
        [SerializeField()]
        public ScrollRect ScenesScrollView;
        
        [SerializeField()]
        public ToggleGroup Content;
        
        [SerializeField()]
        public Button ScenesScrollButton;
        
        [SerializeField()]
        public Button ScenesScrollCloseButton;
        
        private UIScenesScrollViewPanelData mPrivateData = null;
        
        public UIScenesScrollViewPanelData mData
        {
            get
            {
                return mPrivateData ?? (mPrivateData = new UIScenesScrollViewPanelData());
            }
            set
            {
                mUIData = value;
                mPrivateData = value;
            }
        }
        
        protected override void ClearUIComponents()
        {
            UISceneDataToggle = null;
            ScenesScrollView = null;
            Content = null;
            ScenesScrollButton = null;
            ScenesScrollCloseButton = null;
            mData = null;
        }
    }
}