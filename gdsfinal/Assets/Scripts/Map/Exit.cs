using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public PlayerController player;

    private void OnEnable()
    {
        Invoke(nameof(E), 0.5f);
    }

    private void E()
    {
        player.canInput = true;
        gameObject.SetActive(false);
    }
}
