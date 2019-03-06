using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ASliderPointUp : RSliderPointUP {

    public override void OnPointerUp(PointerEventData eventData)
    {
        colorPicker.OnAlphaSliderChanged(slider.value);
    }
}
