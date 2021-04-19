using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    // Set the Slider's initial values
    public void SetMaxHealth(int max, int start)
    {
        // Max health value in Health Bar
        slider.maxValue = max;
        // Starting health value in Health Bar
        slider.value = start;
        // Set the gradient to the green part(full health)
        //fill.color = gradient.Evaluate(1f);

        // -------- ESTO LUEGO SERÁ SOLO VERDE (se borra esto y se descomenta la línea anterior)------------------- 
        // Set gradient value according to slider's value
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    // Every time we heal or get hurt we must call this function to adjust the slider
    public void SetHealth(int health)
    {
        // Set slider value to current hitPoints
        slider.value = health;
        // Set gradient value according to slider's value
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    
}
