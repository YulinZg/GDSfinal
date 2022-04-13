using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestUI : MonoBehaviour
{
    public GameObject panel;
    [SerializeField] private Image icon;
    [SerializeField] private Text title;
    [SerializeField] private Text description;

    public void Setup(Color color, string title, string description)
    {
        icon.color = color;
        this.title.text = title;
        this.description.text = description;
    }
}
