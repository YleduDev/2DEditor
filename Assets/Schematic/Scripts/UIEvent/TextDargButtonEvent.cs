using DevelopEngine;
using UnityEngine.EventSystems;

public class TextDargButtonEvent : DragButtonEvent
{
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (!go) return;
        go.transform.SetParent(SchematicControl.Instance.TextParent, false);
        TextForSchematic textSch = go.GetComponent<TextForSchematic>();
        if (!textSch) ConsoleM.LogError(pelPrefabPathPrefix + "目录下预制体木有挂载BaseGraphicForSchematic脚本");
        textSch.prefabPath = pelPrefabPathPrefix;
        textSch.InitMainColor();
    }

}
