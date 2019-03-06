using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoonFlash : MonoBehaviour
{
    private CanvasGroup moonCanvasGroup;
    public float flashSpeed = 2f;//光晕闪动速度
    [HideInInspector]
    public bool isOn = false;
    private bool isOver = false;
    private float maxAlpha = 1f;//显示的最高alpha值
    private float minAlpha = 0.05f;//显示的最低alpha值
    void Start()
    {
        Init();
    }
   void Init()
    {
        moonCanvasGroup = GetComponent<CanvasGroup>();
    }
    void Update()
    {
        if (!isOn) return;
        if (!moonCanvasGroup) Init();
        if (moonCanvasGroup.alpha < maxAlpha && isOver)
        {
            moonCanvasGroup.alpha += flashSpeed * Time.deltaTime;
        }
        else
        {
            isOver = false;
            moonCanvasGroup.alpha -= flashSpeed * Time.deltaTime;
            if (moonCanvasGroup.alpha < minAlpha)
            {
                isOver = true;
            }

        }

    }

    public void SetApphaReset()
    {
        if (!moonCanvasGroup) Init();
        moonCanvasGroup.alpha = 1;
    }
}
