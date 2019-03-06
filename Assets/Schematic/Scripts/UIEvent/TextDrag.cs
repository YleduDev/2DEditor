using UnityEngine;
using UnityEngine.UI;

public class TextDrag : PelDrag {
    
    public override void Init()
    {
        tf = transform;
        rtf = transform as RectTransform;
        baseGraphic = GetComponent<BaseGraphicForSchematic>();
    }
}
