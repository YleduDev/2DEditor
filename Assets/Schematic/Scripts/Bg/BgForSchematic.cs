using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgForSchematic : BaseGraphicForSchematic
{
    public string key="";
    private Image image;
    protected override void Start()
    {
        image = GetComponent<Image>();
    }

    public override void MyDestroy()
    {
        
    }

    public override void SetColor(Color color)
    {
        image.color = color;
    }

    public override void SetParent(bool bo)
    {
        
    }
}
