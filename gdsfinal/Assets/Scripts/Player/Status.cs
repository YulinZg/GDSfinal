using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private float healthUnit;
    [SerializeField] private int attack;
    [SerializeField] private int critProbability;
    [SerializeField] private float critProbabilityUnit;
    [SerializeField] private int critRate;
    [SerializeField] private float critRateUnit;
    [SerializeField] private int defense;
    [SerializeField] private float defenseUnit;
    [SerializeField] private int moveSpeed;
    [SerializeField] private float moveSpeedUnit;

    [SerializeField] private GameObject damageText;
    [SerializeField] private float damageUIOffsetXMin;
    [SerializeField] private float damageUIOffsetXMax;
    [SerializeField] private float damageUIOffsetYMin;
    [SerializeField] private float damageUIOffsetYMax;

    private float maxHp;
    private float currentHp;
    private PlayerController player;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp = health * healthUnit;
    }

    public void TakeDamage(float damage)
    {
        damage -= defense * defenseUnit;
        float d = Mathf.Floor(damage);
        if (d < 0)
            d = 0;
        currentHp -= d;
        Damage damageUI = Instantiate(damageText, transform.position + new Vector3(Random.Range(damageUIOffsetXMin, damageUIOffsetXMax), Random.Range(damageUIOffsetYMin, damageUIOffsetYMax), 0), Quaternion.identity).GetComponent<Damage>();
        damageUI.ShowUIDamage(d, Color.red);
        if (currentHp < 0)
            currentHp = 0;
        if (currentHp == 0)
            player.Die();
    }

    public void AddHealth(int amount)
    {
        health += amount;
        maxHp = health * healthUnit;
        currentHp += amount * healthUnit;
    }

    public void AddAttack(int amount)
    {
        attack += amount;
    }

    public void AddCritProbability(int amount)
    {
        critProbability += amount;
    }

    public void AddCritRate(int amount)
    {
        critRate += amount;
    }

    public void AddDefense(int amount)
    {
        defense += amount;
    }

    public void AddMoveSpeed(int amount)
    {
        moveSpeed += amount;
    }

    public int GetAttack()
    {
        return attack;
    }

    public float GetCritProbability()
    {
        return critProbability * critProbabilityUnit;
    }

    public float GetCritRate()
    {
        return 1f + critRate * critRateUnit;
    }

    public float GetSpeed()
    {
        return moveSpeed * moveSpeedUnit;
    }
}
