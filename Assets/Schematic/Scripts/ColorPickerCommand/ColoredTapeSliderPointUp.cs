using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using SpringGUI;
using UnityEngine.UI;

public class ColoredTapeSliderPointUp : MonoBehaviour,IPointerUpHandler{

    public ColorPicker colorPicker;
    public Slider slider;

    public void OnPointerUp(PointerEventData eventData)
    {
        colorPicker.verticalSliderChanged(slider.value); 
    }
    
	
}
