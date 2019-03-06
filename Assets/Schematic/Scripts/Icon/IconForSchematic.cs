using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconForSchematic : BaseGraphicForSchematic
{
    public string spriteKey;

    private Image image;

    protected override void Start()
    {
        base.Start();
        schematicType = SchematicType.Icon;
}
    public override void MyDestroy()
    {
        SchematicControl.Instance.Delete(this.gameObject);
    }

    public override void SetColor(Color color)
    {
        if (!image) image = GetComponent<Image>();
        image.color = color;
    }

    public override void SetParent(bool bo)
    {
        transform.SetParent(SchematicControl.Instance.IcParent, bo);
    }
}
