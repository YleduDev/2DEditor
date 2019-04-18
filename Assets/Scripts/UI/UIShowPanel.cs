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
    using global::TDE;
    using UniRx;

    public class UITItem
    {
        public UITextItem UITextItem;
        public UIImageItem UIImageItem;
        public UILineItem UILineItem;
        public UITItem(UITextItem UITextItem, UIImageItem UIImageItem, UILineItem UILineItem)
        {
            this.UIImageItem = UIImageItem;
            this.UITextItem = UITextItem;
            this.UILineItem = UILineItem;
        }
    }

    public class UIShowPanelData : QFramework.UIPanelData
    {
        //new 是为了测试 可以加一些测试数据
        public TSceneData model=new TSceneData();
    }
    
    public partial class UIShowPanel : QFramework.UIPanel
    {
        UITItem UITItem;
        protected override void ProcessMsg(int eventId, QFramework.QMsg msg)
        {
           
        }       
        protected override void OnInit(QFramework.IUIData uiData)
        {
            UITItem = new UITItem(UITextItem, UIImageItem, UILineItem);
            UIContent.Init();

          //  ModelChangeInit(uiData);
        }
        
        public void ModelChangeInit(QFramework.IUIData uiData)
        {
            mData = uiData as UIShowPanelData ?? new UIShowPanelData();
            UIContent.GenerateGraphicItem(mData.model, UITItem);

            //订阅 model添加
            mData.model.ImageDataList.ObserveAdd().Subscribe(item => { UIContent.Add(item.Value); });
            mData.model.ImageDataList.ObserveRemove().Subscribe(item => { UIContent.Remove(item.Value); });

            mData.model.LineDataList.ObserveAdd().Subscribe(item => { UIContent.Add(item.Value); });
            mData.model.LineDataList.ObserveRemove().Subscribe(item => { UIContent.Remove(item.Value); });

            mData.model.TextDataList.ObserveAdd().Subscribe(item => { UIContent.Add(item.Value); });
            mData.model.TextDataList.ObserveRemove().Subscribe(item => { UIContent.Remove(item.Value); });

            //订阅画布大小
            mData.model.canvasWidth.Subscribe(f => { UIContent.SetContentWidth(f); });
            mData.model.canvasHeight.Subscribe(f => { UIContent.SetContentHeight(f); });
        }

        protected override void OnOpen(QFramework.IUIData uiData)
        {
            ModelChangeInit(uiData);
        }
        
        protected override void OnShow()
        {
        }
        
        protected override void OnHide()
        {
        }
        
        protected override void OnClose()
        {
        }
    }
}
