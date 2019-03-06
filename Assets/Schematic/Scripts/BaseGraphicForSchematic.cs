using UnityEngine;

public abstract class BaseGraphicForSchematic:MonoBehaviour {
    //预制体路径
    [HideInInspector]
    public string prefabPath = "";
    protected RectTransform rect;
    [HideInInspector]
    public RectTransform Rect
    {
        get
        {
            if(!rect) rect = transform as RectTransform;
            return rect;
        }
    }

    protected MoonFlash moonFlash;
    public MoonFlash MoonFlash
    {
        get
        {
            if(!moonFlash) moonFlash  = GetComponent<MoonFlash>();
            return moonFlash;
        }
    }

    public SchematicType schematicType;
    protected  virtual void Start() {
        rect = transform as RectTransform;
        moonFlash = GetComponent<MoonFlash>();
    }
    
    public Color mainColor ;//图片 线断都是image颜色。文本是文字颜色

    public virtual void DragTOInit(Vector3 worldPos,Camera camera) { }

    public virtual void SetMoonFlashRun()
    {
        MoonFlash.isOn = true;
    }
    public virtual void SetMoonFlashClose()
    {
        MoonFlash.isOn = false;
        MoonFlash.SetApphaReset();
    }

    public abstract void MyDestroy();

    public abstract void SetColor(Color color);

    public abstract void SetParent(bool bo);
}
