using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public void TakeDamage(float damage)
    {
        Debug.Log(damage.ToString());
    }
}
