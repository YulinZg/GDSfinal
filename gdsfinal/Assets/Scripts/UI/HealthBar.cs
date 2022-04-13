using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public RectTransform rect;
    public Slider slider;
    public Image heart;
    public float widthUnit;

    public void SetMaxHealth(int health)
    {
        rect.sizeDelta = new Vector2(widthUnit * health, rect.sizeDelta.y);
    }

    public void SetHealth(float currentHealth, float maxHealth)
    {
        slider.value = currentHealth / maxHealth;
        if (currentHealth == 0)
            heart.color = Color.black;
    }
}
