﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using SpringGUI;
using UnityEngine.UI;

public class RSliderPointUP : MonoBehaviour, IPointerUpHandler
{
    public ColorPicker colorPicker;
    public Slider slider;

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        colorPicker.OnRedSliderChanged(slider.value);
    }

}
