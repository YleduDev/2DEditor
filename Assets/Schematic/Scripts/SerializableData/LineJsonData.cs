 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BindData
{
    public bool isBand;
    public BindTpye aType;
    public BindTpye bType;
    public string aBandNane;
    public string bBandNane;


    public void BingInitA(BindTpye aType, string name)
    {
        this.isBand = true;
        this.aType = aType;
        this.aBandNane = name;
    }
    public void BingInitB(BindTpye BType, string name)
    {
        this.isBand = true;
        this.bType = BType;
        this.bBandNane = name;
    }

    public void RelieveBind()
    {
        this.isBand = false;
        this.aType = BindTpye.Null;
        this.bType = BindTpye.Null;
        this.aBandNane = null;
        this.bBandNane = null;
    }

}
[Serializable]
public class LineData  {
    //线的宽度
    public float LineHigh;
    //line 对象预制体的路径
    public string linePrefabPath;

    public LinkType linkType;
    //起始点 local
    public Vector3Serializer a;
    //结点 local
    public Vector3Serializer b;

    //屏幕坐标
    public Vector3Serializer screenA;
    //结点 屏幕坐标
    public Vector3Serializer screenB;

    public Vector3Serializer breakPoint;
    public Vector3Serializer breakLocalPoint;
    //颜色
    public ColorSerializer color;
    //绑定对象数据 
    public BindData bindData;
    public LineData() { }
    public LineData(LineForSchematic line)
    {
        this.LineHigh = line.minPlx;
        this.a.Fill(line.pointA);
        this.b.Fill(line.pointB);
        this.screenA.Fill(line.screenA);
        this.screenB.Fill(line.screenB);
        this.breakPoint.Fill (line.breakWorldPos);
        breakLocalPoint.Fill(line.breakLocalPos);
        this.bindData = line.bindData;
        this.linePrefabPath = line.prefabPath;
        this.color.Fill( line.mainColor);
        this.linkType = line.linkType;
    }
}
//图元数据
[Serializable]
public class PelData
{
    //tran
    public string key;
    public Vector3Serializer icLocaPos;
    public Vector3Serializer icLocalEuler;
    public Vector3Serializer icLocalScale;
    //预制体路径
    public string prefabPath;

    public ColorSerializer color;

    public float width;
    public float height;

    public PelData() { }
    public PelData(IconForSchematic iconSch)
    {
        this.icLocaPos.Fill(iconSch.transform.localPosition);
        this.icLocalEuler.Fill(iconSch.transform.localEulerAngles);
        this.icLocalScale.Fill(iconSch.transform.localScale);
        this.key = iconSch.spriteKey;
        this.prefabPath = iconSch.prefabPath;
        this.color.Fill(iconSch.mainColor);
        RectTransform rect = iconSch.transform as RectTransform;
        this. width = rect.rect.width;
        this.height = rect.rect.height;
    }

}

[Serializable]
public class TextData
{
    public string text;
    public Vector3Serializer icLocaPos;
    public Vector3Serializer icLocalEuler;
    public Vector3Serializer icLocalScale;
    //预制体路径
    public string prefabPath;

    public ColorSerializer textColor;

    public float width;
    public float height;
    public TextData() { }
    public TextData(TextForSchematic textSch)
    {
        this.icLocaPos.Fill(textSch.transform.localPosition);
        this.icLocalEuler.Fill(textSch.transform.localEulerAngles);
        this.icLocalScale.Fill(textSch.transform.localScale);
        this.text = textSch.text;
        this.prefabPath = textSch.prefabPath;
        this.textColor.Fill(textSch.mainColor);
        RectTransform rect = textSch.transform as RectTransform;
        this.width = rect.rect.width;
        this.height = rect.rect.height;
    }
}

[Serializable]
public class BGData
{
    public string key;
    public ColorSerializer color;
    public BGData() { }
    public BGData(BgForSchematic bgForSch)
    {
        key = bgForSch.key;
        color.Fill(bgForSch.mainColor);
    }
}

[SerializeField]
public class SchematicJsonData {
    public string sceneName;
    public  List <LineData> listLineData;
    public List<PelData> listPelData;
    public List<TextData> textData;
    public BGData bgData;
    public SchematicJsonData(string name, List<LineData> listLine, List<PelData> listPel,List<TextData> textDat,BGData bgData)
    {
        this.sceneName = name;
        this.listLineData = listLine;
        this.listPelData = listPel;
        this.textData = textDat;
        this.bgData = bgData;
    }
}
[Serializable]
public class SceneLineData
{
    public List<SchematicJsonData> schematData;
    public SceneLineData() { }
    public SceneLineData(List<SchematicJsonData> list) { this.schematData = list; }

    public SceneLineData(Dictionary<string, SchematicJsonData> AllLineDict)
    {
        if (AllLineDict==null|| AllLineDict.Count <= 0) return;
        schematData = new List<SchematicJsonData>(AllLineDict.Count);
        foreach (var item in AllLineDict.Values)
        {
            SchematicJsonData jsonData = new SchematicJsonData(item.sceneName, item.listLineData, item.listPelData,item.textData,item.bgData);
            if (jsonData != null) schematData.Add(jsonData);
        }
    }
}
[Serializable]
public struct Vector3Serializer
{
    public float x;
    public float y;
    public float z;
    public void Fill(Vector3 v3)
    {
        x = v3.x; y = v3.y; z = v3.z;
    }    
}

[Serializable]
public struct ColorSerializer
{
    public float r;
    public float g;
    public float b;
    public float a;
    public void Fill(Color color)
    {
        r = color.r; g = color.g; b = color.b; a = color.a;
    }
}


