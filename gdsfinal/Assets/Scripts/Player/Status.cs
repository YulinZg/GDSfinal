using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField] private int health = 10;
    [SerializeField] private float healthUnit;
    [SerializeField] private int attack = 10;
    [SerializeField] private int critProbability = 10;
    [SerializeField] private float critProbabilityUnit;
    [SerializeField] private int critRate = 10;
    [SerializeField] private float critRateUnit;
    [SerializeField] private int defense = 10;
    [SerializeField] private float defenseUnit;
    [SerializeField] private int moveSpeed = 10;
    [SerializeField] private float moveSpeedUnit;

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private StatusUI statusUI;
    [SerializeField] private GameObject damageText;
    [SerializeField] private float damageUIOffsetXMin;
    [SerializeField] private float damageUIOffsetXMax;
    [SerializeField] private float damageUIOffsetYMin;
    [SerializeField] private float damageUIOffsetYMax;

    [SerializeField] private GameObject[] bloodEffects;
    [SerializeField] private AudioClip hurtClip;

    private float maxHp;
    private float currentHp;
    private PlayerController player;
    private AudioSource audioSource;

    [Header("Scroll")]
    [SerializeField] private float breathAmount;
    [SerializeField] private float breathAddition;
    private bool assassinScroll = false;

    [SerializeField] private float perfectStrengthen;
    [SerializeField] private float perfectStrengthenAddition;
    private bool perfectScroll = false;

    [SerializeField] private float violentBloodLine;
    [SerializeField] private float violentBloodLineAddition;
    [SerializeField] private float violentStrengthen;
    [SerializeField] private float violentStrengthenAddition;
    private bool violentScroll = false;

    [SerializeField] private float gutsBloodLine;
    [SerializeField] private float gutsBloodLineAddition;
    private bool gutsScroll = false;

    [SerializeField] private float quickTime;
    [SerializeField] private float quickTimeAddition;
    private bool quickHandsScroll = false;
    private bool isQuick = false;
    private float quickTimer = 0;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp = health * healthUnit;
        healthBar.SetMaxHealth(health);
    }

    public void TakeDamage(float damage, float stopTime)
    {
        float hp = currentHp;
        damage -= defense * defenseUnit;
        float d = Mathf.Floor(damage);
        if (d < 1f)
            d = 1f;
        currentHp -= d;
        DamageUI damageUI = Instantiate(damageText, transform.position + new Vector3(Random.Range(damageUIOffsetXMin, damageUIOffsetXMax), Random.Range(damageUIOffsetYMin, damageUIOffsetYMax), 0), Quaternion.identity).GetComponent<DamageUI>();
        damageUI.ShowUIDamage(d, Color.red);
        Instantiate(bloodEffects[Random.Range(0, bloodEffects.Length)], transform);
        if (stopTime > 0)
            player.Hurt(stopTime);
        if (currentHp <= 0)
        {
            if (gutsScroll && hp / maxHp > gutsBloodLine && hp != 1)
                currentHp = 1;
            else
                currentHp = 0;
        }
        healthBar.SetHealth(currentHp, maxHp);
        if (currentHp == 0)
        {
            DieEffect();
            player.Die();
        }
        audioSource.PlayOneShot(hurtClip);
    }

    private void DieEffect()
    {
        for (int i = 0; i < bloodEffects.Length; i++)
            Instantiate(bloodEffects[i], transform);
    }

    public void RestoreHp(float amount)
    {
        float h = Mathf.Floor(amount);
        currentHp += h;
        if (currentHp > maxHp)
            currentHp = maxHp;
        healthBar.SetHealth(currentHp, maxHp);
        DamageUI damageUI = Instantiate(damageText, transform.position + new Vector3(Random.Range(damageUIOffsetXMin, damageUIOffsetXMax), Random.Range(damageUIOffsetYMin, damageUIOffsetYMax), 0), Quaternion.identity).GetComponent<DamageUI>();
        damageUI.ShowUIDamage(h, Color.green);
    }

    public void AddHealth(int amount)
    {
        health += amount;
        if (health > 100)
            health = 100;
        maxHp = health * healthUnit;
        currentHp += amount * healthUnit;
        if (currentHp > maxHp)
            currentHp = maxHp;
        healthBar.SetMaxHealth(health);
        healthBar.SetHealth(currentHp, maxHp);
        statusUI.SetHealth(health);
    }

    public void AddAttack(int amount)
    {
        attack += amount;
        if (attack > 100)
            attack = 100;
        statusUI.SetAttack(attack);
    }

    public void AddCritProbability(int amount)
    {
        critProbability += amount;
        if (critProbability > 100)
            critProbability = 100;
        statusUI.SetCritProbability(critProbability);
    }

    public void AddCritRate(int amount)
    {
        critRate += amount;
        if (critRate > 100)
            critRate = 100;
        statusUI.SetCritRate(critRate);
    }

    public void AddDefense(int amount)
    {
        defense += amount;
        if (defense > 100)
            defense = 100;
        statusUI.SetDefense(defense);
    }

    public void AddMoveSpeed(int amount)
    {
        moveSpeed += amount;
        if (moveSpeed > 100)
            moveSpeed = 100;
        statusUI.SetMoveSpeed(moveSpeed);
    }

    public float GetMaxHP()
    {
        return maxHp;
    }

    public int GetAttack()
    {
        return attack;
    }

    public float GetCritProbability()
    {
        float cp = critProbability * critProbabilityUnit;
        if (perfectScroll && currentHp == maxHp)
        {
            cp += perfectStrengthen;
            if (cp > 1)
                cp = 1;
        }
        return cp;
    }

    public float GetCritRate()
    {
        float cr = 1.2f + (critRate - 10) * critRateUnit;
        if (violentScroll && currentHp / maxHp < violentBloodLine)
        {
            cr += violentStrengthen;
        }
        return cr;
    }

    public float GetSpeed()
    {
        float s = 1 + (moveSpeed - 10) * moveSpeedUnit;
        if (isQuick)
            s += 1f;
        return s;
    }

    public void AssassinScroll()
    {
        if (assassinScroll)
        {
            breathAmount += breathAddition;
        }
        else
            assassinScroll = true;
    }

    public void AssassinBreath()
    {
        if (assassinScroll)
        {
            RestoreHp(breathAmount);
        }
    }

    public void PerfectScroll()
    {
        if (perfectScroll)
        {
            perfectStrengthen += perfectStrengthenAddition;
        }
        else
            perfectScroll = true;
    }

    public void ViolentScroll()
    {
        if (violentScroll)
        {
            violentBloodLine += violentBloodLineAddition;
            if (violentBloodLine > 0.8f)
                violentBloodLine = 0.8f;
            violentStrengthen += violentStrengthenAddition;
        }
        else
            violentScroll = true;
    }

    public void BerserkerScroll()
    {
        AddAttack(Mathf.FloorToInt(attack * 0.5f));
        health -= Mathf.FloorToInt(health * 0.5f);
        maxHp = health * healthUnit;
        currentHp -= Mathf.Floor(currentHp * 0.5f);
        healthBar.SetMaxHealth(health);
        healthBar.SetHealth(currentHp, maxHp);
        statusUI.SetHealth(health);
    }

    public void GutsScroll()
    {
        if (gutsScroll)
        {
            gutsBloodLine -= gutsBloodLineAddition;
            if (gutsBloodLine < 0.05f)
                gutsBloodLine = 0.05f;
        }
        else
            gutsScroll = true;
    }

    public void QuickHandsScroll()
    {
        if (quickHandsScroll)
        {
            quickTime += quickTimeAddition;
        }
        else
            quickHandsScroll = true;
    }

    public void QuickSwitch()
    {
        if (quickHandsScroll)
        {
            quickTimer = 0;
            if (!isQuick)
                StartCoroutine(Quicking());
        }
    }

    IEnumerator Quicking()
    {
        isQuick = true;
        while (quickTimer < quickTime)
        {
            quickTimer += Time.deltaTime;
            yield return null; 
        }
        isQuick = false;
    }
}
