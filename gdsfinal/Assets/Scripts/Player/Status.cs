using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float attack;
    [SerializeField] private float critProbability;
    [SerializeField] private float critRate;
    [SerializeField] private float defense;
    [SerializeField] private float moveSpeed;

    [SerializeField] private GameObject damageText;
    [SerializeField] private float damageUIOffsetXMin;
    [SerializeField] private float damageUIOffsetXMax;
    [SerializeField] private float damageUIOffsetYMin;
    [SerializeField] private float damageUIOffsetYMax;

    private float currentHealth;
    private PlayerController player;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
    }

    public void TakeDamage(float damage)
    {
        damage *= 1 - defense;
        float d = Mathf.Floor(damage);
        currentHealth -= d;
        Damage damageUI = Instantiate(damageText, transform.position + new Vector3(Random.Range(damageUIOffsetXMin, damageUIOffsetXMax), Random.Range(damageUIOffsetYMin, damageUIOffsetYMin), 0), Quaternion.identity).GetComponent<Damage>();
        damageUI.ShowUIDamage(d, Color.red);
        if (currentHealth < 0)
            currentHealth = 0;
        if (currentHealth == 0)
            player.Die();
    }

    public float GetAttack()
    {
        return attack;
    }

    public float GetCritProbability()
    {
        return critProbability;
    }

    public float GetCritRate()
    {
        return critRate;
    }

    public float GetSpeed()
    {
        return moveSpeed;
    }
}
