using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LineTest : MonoBehaviour {

    //private void Start()
    //{
    //    SchematicControl.Instance.LoadLineScene("3");  
    //}
    private void Update()
    {
      
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            //删除线
            SchematicControl.Instance.MySelectDelele();
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
           // Instantiate(SchematicControl.Instance.selectGraphic, SchematicControl.Instance.IcParent); 
        }
    }

    public void GetOne(string key)
    {
        SchematicControl.Instance.LoadLineScene(key);
    }
    //选择线段按钮注册事件
    public void ChangeLine(bool bo)
    {
        if (bo)
        {
           string path= EventSystem.current.currentSelectedGameObject.name;
            //Debug.Log(path); 
            SchematicControl.Instance.linePrefabPath = "Schematic/Line/" + path;
        }
    }

    public void ChangeLineLink(bool bo)
    {
        if (bo)
        {
            string path = EventSystem.current.currentSelectedGameObject.name;
            //Debug.Log(path);
            LinkType linkType;
            if (Enum.TryParse<LinkType>(path, out linkType))
                SchematicControl.Instance.linkType = linkType;
        }
    }
}
