using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public Image skillingFill;
    public Image cooldownFill;

    public void SetSkillingFill(float percent)
    {
        skillingFill.fillAmount = percent;
    }

    public void SetCooldownFill(float percent)
    {
        cooldownFill.fillAmount = 1 - percent;
    }
}
