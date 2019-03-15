/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using System.Collections.Generic;
using TDE;
using System.Linq;

namespace QFramework.TDE
{
    public partial class UIContent : UIElement
	{
        //»º´æ
        private Dictionary<T_Graphic,UIImageItem> imageDict=new Dictionary<T_Graphic, UIImageItem>();
        private Dictionary<T_Graphic, UITextItem> textDict = new Dictionary<T_Graphic, UITextItem>();
        private Dictionary<T_Graphic, UILineItem> lineDict = new Dictionary<T_Graphic, UILineItem>();
        
        private UITItem UITItem;
        private TSceneData model;
    
        private void Awake()
		{
            BGButton.onClick.AddListener(() => Global.OnClick());
        }
             
        public void Add(T_Graphic graphicItem)
        {
            if (graphicItem.IsNull()) return;
            switch (graphicItem.graphicType)
            {
                case GraphicType.Image:
                    UIImageItem UIImageItem = UITItem.UIImageItem.Instantiate();
                    UIImageItem
                        .ApplySelfTo(self =>self.Init(model, graphicItem, ImageParent, LineParent))               
                        .ApplySelfTo(self =>imageDict.Add(graphicItem, UIImageItem));break;
                case GraphicType.Text:
                    UITextItem UITextItem = UITItem.UITextItem.Instantiate();
                    UITextItem
                        .ApplySelfTo(self =>self.Init(graphicItem, TextParent))                     
                        .ApplySelfTo(self =>textDict.Add(graphicItem, UITextItem)); break;
                case GraphicType.Line:
                    UILineItem UILineItem = UITItem.UILineItem.Instantiate();
                    //³õÊ¼»¯
                    UILineItem
                        .ApplySelfTo(self => self.Init(graphicItem, LineParent))
                        .ApplySelfTo(self => lineDict.Add(graphicItem, UILineItem)); break;
                default: break;
            }
        }

        internal void Remove(T_Graphic value)
        {
            if (value.IsNull()) return;
            switch (value.graphicType)
            {
                case GraphicType.Image:
                    if (imageDict.ContainsKey(value))
                    {
                        Destroy(imageDict[value].gameObject);
                        imageDict.Remove(value);
                    }break;
                case GraphicType.Text:
                    if (textDict.ContainsKey(value))
                    {
                        Destroy(textDict[value].gameObject);
                        textDict.Remove(value);
                    }break;
                case GraphicType.Line:
                    if (lineDict.ContainsKey(value))
                    {
                        Destroy(lineDict[value].gameObject);
                        lineDict.Remove(value);
                    } break;
                default:break;
            }
            value = null;
        }

        public void GenerateGraphicItem(TSceneData model, UITItem UITItem)
        {
            this.model = model;
            imageDict.Clear();
            textDict.Clear();
            lineDict.Clear();
            this.UITItem = UITItem;
            model.graphicDataList.Where(item=> item.IsNotNull()).ForEach(item => Add(item));
        }

        protected override void OnBeforeDestroy(){}
    }
}