using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using QFramework.TDE;
using UniRx;
namespace TDE
{
    /// <summary>
    /// 2d�༭���
    /// </summary>
    public class APP : MonoBehaviour
    {
        public void Awake()
        {
            #region test ����
            TSceneData model = new TSceneData();
            model.graphicDataList.Add(new T_Text() {
                graphicType = GraphicType.Text,
                localScale = new Vector3ReactiveProperty(Vector3.one),
                localPos = new Vector2ReactiveProperty(new Vector2(-645f, 395f)),
                locaRotation = new QuaternionReactiveProperty(Quaternion.identity),
                widht = new FloatReactiveProperty(100f),
                height = new FloatReactiveProperty(100f)
            });

            model.graphicDataList.Add (new T_Image() {
                graphicType = GraphicType.Image,
                localScale =new Vector3ReactiveProperty(Vector3.one),
                localPos = new Vector2ReactiveProperty(new Vector2(776f, -374f)),
                locaRotation = new QuaternionReactiveProperty(Quaternion.identity),
                widht = new FloatReactiveProperty(100f),
                height = new FloatReactiveProperty(100f)
            });

            model.graphicDataList.Add(new T_Line() {
                graphicType = GraphicType.Line,
                localScale =new Vector3ReactiveProperty(Vector3.one),
                localPos = new Vector2ReactiveProperty( new Vector2(169.8f, 83.2f)),
                locaRotation = new QuaternionReactiveProperty(Quaternion.identity),
                widht =new FloatReactiveProperty( 110f),
                height = new FloatReactiveProperty(3f)
            });
            #endregion
            ResMgr.Init();
            UIMgr.SetResolution(1920, 1080, 0);
            UIMgr.OpenPanel<UIShowPanel>(new UIShowPanelData() { model=model});
        }

    }
    //��������
    public class TSceneData
    {
        public ReactiveCollection<T_Graphic> graphicDataList=new ReactiveCollection<T_Graphic>();
        public void Add(T_Graphic model)
        {
            graphicDataList.Add(model);
        }
    }
    //���Գ�������
    public class TSceneDataList
    {
        public Dictionary<string, TSceneData> sceneDataDict;
    }
}
