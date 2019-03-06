using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextForSchematic : BaseGraphicForSchematic {
    [HideInInspector]
    public string text;

    public InputField inputField;
    public Text textMeshProUGUI; 
    public override void MyDestroy()
    {
        SchematicControl.Instance.Delete(this.gameObject);
    }
    protected override void Start()
    {
        Init(); 
    }
    public void Init()
    {
        schematicType = SchematicType.Text;
        inputField = GetComponent<InputField>();
        if (inputField) inputField.onValueChanged.AddListener(TextChange);
        textMeshProUGUI = transform.GetChild(0).Find("Text").GetComponent<Text>();
    }
    public void TextChange(string text)
    {
        this.text = text;
    }

    public override void SetColor(Color color)
    {
        if (!textMeshProUGUI) Init();
        textMeshProUGUI.color = color;
    }
    public void InitMainColor()
    {
        if (!textMeshProUGUI) Init();
        mainColor= textMeshProUGUI.color;
    }

    public override void SetParent(bool bo)
    {
        transform.SetParent(SchematicControl.Instance.TextParent, bo);
    }
}
