using DevelopEngine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PelDargButtonEvent : DragButtonEvent {

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (!Input.GetMouseButton(0)||!go) return;            
        Sprite sprite = SchematicUIManager.Instance.getPicture.GetSprite(key);
        Image image = go.GetComponent<Image>();
        ImageAdaptive(image, sprite);
        IconForSchematic iconSch= go.GetComponent<IconForSchematic>();
        if (!iconSch) ConsoleM.LogError(pelPrefabPathPrefix + "目录下预制体木有挂载BaseGraphicForSchematic脚本");
        iconSch.spriteKey = key;
        iconSch.prefabPath = pelPrefabPathPrefix;
        iconSch.mainColor = image.color;
        go.transform.SetParent(SchematicControl.Instance.IcParent, false);
    }
  
    ///自适应image
    protected void ImageAdaptive(Image image, Sprite sprite)
    {
        image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sprite.rect.width);
        image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sprite.rect.height);
        image.sprite = sprite;
    }
    public void SelcetButtonEvent()
    {
        fileNane = EventSystem.current.currentSelectedGameObject.name;
    }
}
