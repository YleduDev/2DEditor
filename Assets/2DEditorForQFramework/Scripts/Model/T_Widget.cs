using System.Collections;
using System.Collections.Generic;
using TDE;
using UnityEngine;
using System;
using UniRx;

    /// <summary>
    /// ¿Ø¼þ
    /// </summary>
    [System.Serializable]
    public class T_Widget : T_Image
    {

        public string prefabName;

        public List<T_Line> LineDataList;

        public List<T_Image> ImageDataList;

        public List<T_Text> TextDataList;

        public T_Widget() : base() { graphicType = GraphicType.Widget; }

        public T_Widget(T_Image image, string prefabName) : base(image)
        {
            graphicType = GraphicType.Widget;
            this.prefabName = prefabName;
        }


        public void BindDataEvent(Action<ReactiveProperty<WebSocketMessage>> act)
        {
            if (LineDataList != null && LineDataList.Count > 0)
                for (int i = 0; i < LineDataList.Count; i++)
                {
                    if (LineDataList[i].AssetNodeData.Value != null)
                        act?.Invoke(LineDataList[i].AssetNodeData);
                }
            if (ImageDataList != null && ImageDataList.Count > 0)
                for (int i = 0; i < ImageDataList.Count; i++)
                {
                    if (ImageDataList[i].AssetNodeData.Value != null)
                        act?.Invoke(ImageDataList[i].AssetNodeData);
                }
            if (TextDataList != null && TextDataList.Count > 0)
                for (int i = 0; i < TextDataList.Count; i++)
                {
                    if (TextDataList[i].AssetNodeData.Value != null)
                        act?.Invoke(TextDataList[i].AssetNodeData);
                }
        }

        public override void Destroy()
        {
            base.Destroy();
            BindDataEvent(data => Global.RemoveBindData(data));
        }
    }

