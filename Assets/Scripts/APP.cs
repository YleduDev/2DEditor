using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using QFramework.TDE;
using UniRx;
namespace TDE
{
    /// <summary>
    /// 2d编辑入口
    /// </summary>
    public class APP : MonoBehaviour
    {
        public void Awake()
        {
            #region test 数据
            TSceneData model = new TSceneData();
            model.graphicDataList = new List<T_Graphic>(3);
            model.graphicDataList.Add(new T_Text() {
                graphicType = GraphicType.Text,
                localScale =Vector3.one,
                localPos = new Vector2ReactiveProperty(new Vector2(-645f, 395f)),
                widht = new FloatReactiveProperty(100f),
                height = new FloatReactiveProperty(100f)
            });

            model.graphicDataList.Add (new T_Image() {
                graphicType = GraphicType.Image,
                localScale = Vector3.one,
                localPos = new Vector2ReactiveProperty(new Vector2(776f, -374f)),
                widht = new FloatReactiveProperty(100f),
                height = new FloatReactiveProperty(100f)
            });

            model.graphicDataList.Add(new T_Line() {
                graphicType = GraphicType.Line,
                localScale = Vector3.one,
                localPos = new Vector2ReactiveProperty( new Vector2(169.8f, 83.2f)),
                widht =new FloatReactiveProperty( 110f),
                height = new FloatReactiveProperty(3f)
            });
            #endregion
            ResMgr.Init();
            UIMgr.SetResolution(1920, 1080, 0);
            UIMgr.OpenPanel<UIShowPanel>(new UIShowPanelData() { model=model});
        }

    }
    //场景对象
    public class TSceneData
    {
        public List<T_Graphic> graphicDataList;
    }
    //所以场景对象
    public class TSceneDataList
    {
        public Dictionary<string, TSceneData> sceneDataDict;
    }
}
