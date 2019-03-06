using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ToggleEvents : MonoBehaviour
{

    public void ShowEditor(Toggle toggle)
    {
            transform.Find(toggle.name + "Content").gameObject.SetActive(toggle.isOn);
    }
}
