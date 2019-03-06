using UnityEngine.EventSystems;

public class LinkLineBackgrund : LinkLine,IPointerClickHandler {


    public void OnPointerClick(PointerEventData eventData)
    {
        //判断有木有点击到辅助图元
        if (SchematicControl.Instance.ImageGizmo.selectedAxis == Axis.None)
        {
           SchematicControl.Instance.SetTargetOb(null);
        }
        
    }
}
