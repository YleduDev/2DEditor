/****************************************************************************
 * 2019.3 LAPTOP-R0ONNKOC
 ****************************************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QFramework.TDE
{
    public partial class UILineSwitch : UIElement,IPointerEnterHandler, IPointerExitHandler,IDragHandler

    {
        public Texture2D hand;
        public Texture2D darg;
        private void Awake()
		{
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        }

		protected override void OnBeforeDestroy()
		{
		}
        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log("»®Ïß");
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            Cursor.SetCursor(hand, new Vector2(hand.width * .5f, hand.height * .5f), CursorMode.Auto);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Cursor.SetCursor(darg, new Vector2(darg.width * .5f, darg.height * .5f), CursorMode.Auto);
        }
    }
}