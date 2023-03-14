using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ShowSliderValue : MonoBehaviour
{
    TextMeshProUGUI text;    
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void SetVal(Slider slider)
    {
        text.text = slider.value.ToString();
    }
}
