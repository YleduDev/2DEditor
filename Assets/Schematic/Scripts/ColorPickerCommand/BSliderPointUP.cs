using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BSliderPointUP : RSliderPointUP {

    public override void OnPointerUp(PointerEventData eventData)
    {
        colorPicker.OnBlueSliderChanged(slider.value); 
    }
}
