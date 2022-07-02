using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    public Slider h_slider;
    public Slider s_slider;

    public void SetMaxHealth(int health)
    {
        h_slider.maxValue = health;
        h_slider.value = health;
    }

    public void SetHealth(int health)
    {
        h_slider.value = health;
    }
}
