using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GSliderPointUp : RSliderPointUP {

    public override void OnPointerUp(PointerEventData eventData)
    {
        colorPicker.OnGreenSliderChanged(slider.value);
    }
}
