using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;
    public GameObject healthBarUI; // Reference to the parent UI GameObject

    public void SetMaxHP(int hp)
    {
        slider.maxValue = hp;
        slider.value = hp;
    }

    public void SetHP(int hp)
    {
        slider.value = hp;
    }

    public void Show()
    {
        healthBarUI.SetActive(true);
    }

    public void Hide()
    {
        healthBarUI.SetActive(false);
    }
}
