/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using TDE;
using System.Linq;
using UniRx;
using UnityEngine;

namespace QFramework.TDE
{
    public partial class UIContent : UIElement
	{
        //缓存
        private Dictionary<T_Graphic,UIImageItem> imageDict=new Dictionary<T_Graphic, UIImageItem>();
        private Dictionary<T_Graphic, UITextItem> textDict = new Dictionary<T_Graphic, UITextItem>();
        private Dictionary<T_Graphic, UILineItem> lineDict = new Dictionary<T_Graphic, UILineItem>();
        private Dictionary<T_Graphic, GameObject> widgetDict = new Dictionary<T_Graphic, GameObject>();

        //对象池
        private SimpleObjectPool<UIImageItem> imagePool;
        private SimpleObjectPool<UILineItem> linePool;
        private SimpleObjectPool<UITextItem> textPool;

        private RectTransform rect;
        private UITItem UITItem;
        private TSceneData model;

        ResLoader loader = ResLoader.Allocate();
        private void Awake()
		{}
             
        public void Add(T_Graphic graphicItem)
        {
            if (graphicItem.IsNull()) return ; 
            switch (graphicItem.graphicType)
            {
                case GraphicType.Image:
                    UIImageItem UIImageItem = imagePool.Allocate();
                    T_Image image = graphicItem as T_Image;
                    UIImageItem
                        .ApplySelfTo(self =>self.Init(model, image))               
                        .ApplySelfTo(self =>  imageDict.Add(graphicItem, UIImageItem));break;
                case GraphicType.Text:
                    UITextItem UITextItem = textPool.Allocate();
                    UITextItem
                        .ApplySelfTo(self =>self.Init(model,graphicItem))                     
                        .ApplySelfTo(self =>textDict.Add(graphicItem, UITextItem)); break;
                case GraphicType.Line:
                    UILineItem UILineItem = linePool.Allocate();
                    //初始化
                    UILineItem
                        .ApplySelfTo(self => self.Init(graphicItem))
                        .ApplySelfTo(self => lineDict.Add(graphicItem, UILineItem)); break;
                case GraphicType.Widget:
                    T_Widget widget = graphicItem as T_Widget;
                    GameObject prefab = loader.LoadSync<GameObject>(widget.prefabName);
                    if (prefab.IsNull()) Log.LogError("没有+widget.prefabName名称的预制体");
                    GameObject go = prefab.Instantiate();
                    UIWidget element = go.GetComponent<UIWidget>();
                    if (element.IsNull()) Log.LogError("控件预制体上没有 对象为 UIWidget 的脚本");
                    element.Init(model, widget);
                    widgetDict.Add(widget, go); break;

                default: break;
            }
        }

        internal void Remove(T_Graphic value)
        {
            if (value.IsNull())  return ;

            //将选中状态
            value.Destroy();
            switch (value.graphicType)
            {
                case GraphicType.Image:
                    if (imageDict.ContainsKey(value))
                    {
                        imagePool.Recycle(imageDict[value]);
                        imageDict.Remove(value);
                    }break;
                case GraphicType.Text:
                    if (textDict.ContainsKey(value))
                    {
                        textPool.Recycle(textDict[value]);
                        textDict.Remove(value);
                    }break;
                case GraphicType.Line:
                    if (lineDict.ContainsKey(value))
                    {
                        linePool.Recycle(lineDict[value]);
                        lineDict.Remove(value);
                    } break;
                case GraphicType.Widget:
                    if (widgetDict.ContainsKey(value))
                    {
                        Destroy(widgetDict[value]);
                        widgetDict.Remove(value);
                    }
                    break;
                default:break;
            }
            value = null;
           
        }

        public IEnumerator GenerateGraphicItem(TSceneData model)
        {
            this.model = model;
            IEnumerator ie = ClearGraphicItem();
            yield return StartCoroutine(ie);
            AllGraphicClear(); 
            model.LineDataList.Where(item => item.IsNotNull()).ForEach(item =>  Add(item));
            yield return null;
            model.ImageDataList.Where(item => item.IsNotNull()).ForEach(item => Add(item));
            yield return null;
            model.TextDataList.Where(item => item.IsNotNull()).ForEach(item => Add(item));
            yield return null;
            model.WidgetDataList.Where(item => item.IsNotNull()).ForEach(item => Add(item));
            yield return null;
        }
        private IEnumerator ClearGraphicItem()
        {
            imageDict.Where(item => item.Value.IsNotNull()).ForEach(item => imagePool.Recycle(item.Value));
            textDict.Where(item => item.Value.IsNotNull()).ForEach(item => textPool.Recycle(item.Value));
            lineDict.Where(item => item.Value.IsNotNull()).ForEach(item => linePool.Recycle(item.Value));

            T_Graphic[] keys = widgetDict.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                Remove(keys[i]);
            }
            yield return null;
        }
        private void AllGraphicClear()
        {
            imageDict.Clear();
            textDict.Clear();
            lineDict.Clear();
            widgetDict.Clear();
        }
        public void SetContentWidth(float width)
        {
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            Global.currentCanvasWidth.Value = width;
        }
        public void SetContentHeight(float height)
        {
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            Global.currentCanvasheight.Value = height;
        }

        public void Init(UITItem UITItem)
        {
            this.UITItem = UITItem;
            rect = transform as RectTransform;
            BGButton.onClick.AddListener(() => Global.OnClick());
            Global.LineParent = LineParent as RectTransform;
            Global.textParent = TextParent as RectTransform;
            Global.imageParent = ImageParent as RectTransform;

            imagePool = new SimpleObjectPool<UIImageItem>(() => UITItem.UIImageItem.Instantiate().Parent(ImageParent), item => item.Hide(), 20);
            linePool = new SimpleObjectPool<UILineItem>(() => UITItem.UILineItem.Instantiate().Parent(LineParent), item => item.Hide(), 20);
            textPool = new SimpleObjectPool<UITextItem>(() => UITItem.UITextItem.Instantiate().Parent(TextParent), item => item.Hide(), 20);

        }
        protected override void OnBeforeDestroy(){
            loader.Recycle2Cache();
            loader = null;
        }
    }
}