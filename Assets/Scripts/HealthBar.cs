using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    
    public void setMaxHP(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void setHP(int health)
    {
        slider.value = health;
    }
}