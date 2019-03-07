/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TDE;
using System.Linq;

namespace QFramework.TDE
{
	public partial class UIImageContent : UIElement
	{
        public TSceneData model;

        private void Awake()
		{
		}

		protected override void OnBeforeDestroy()
		{
		}

        public void Add(T_Graphic graphicItem, UITItem UITItem)
        {
            if (graphicItem.IsNull()) return;
            switch (graphicItem.graphicType)
            {
                case GraphicType.Image:UITItem.UIImageItem.Instantiate().Init(graphicItem,ImageParent);
                    break;
                case GraphicType.Text: UITItem.UITextItem.Instantiate().Init(graphicItem,TextParent);
                    break;
                case GraphicType.Line:
                    break;
                default:
                    break;
            }
        }
     
        public void GenerateGraphicItem(TSceneData model,UITItem UITItem)
        {
            this.model = model;
            model.graphicDataList.ForEach(item => Add(item, UITItem));
        }
    }
}