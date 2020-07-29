using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeBar : MonoBehaviour
{
    public Slider grenadeSlider;
    

    public void setMaxSeconds(float seconds)
    {
        grenadeSlider.maxValue = seconds;
        grenadeSlider.value = seconds;
    }

    public void setSeconds(float seconds)
    {
        grenadeSlider.value = seconds;
    }

    

}
