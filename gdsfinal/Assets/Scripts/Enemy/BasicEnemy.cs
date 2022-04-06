using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    private float ownSpeed = 5;
    private int ownDizzinessValue = 0;
    private List<Coroutine> fireCoroutine = new List<Coroutine>();
    private List<Coroutine> waterCoroutine = new List<Coroutine>();
    private List<Coroutine> earthCoroutine = new List<Coroutine>();
    private List<Coroutine> lightningCoroutine = new List<Coroutine>();

    private bool canGoBack;
    private Vector3 startPos;
    private Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        canGoBack = false;
        anim = GetComponent<Animator>();
        mySprite = GetComponent<SpriteRenderer>();
        //hp = 100000;
        rid = GetComponent<Rigidbody2D>();
        //dizzinessValue = 100;
        moveSpeed = ownSpeed;
        isburnning = false;
        isWater = false;
        isEarth = false;
        isLighting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canGoBack)
        {
            if (Vector3.Distance(startPos, destination) > 0.01f)
            {
                transform.position += (destination - transform.position) * backDis * Time.deltaTime;
            }
            else
            {
                transform.position = destination;
                canGoBack = false;
            }
        }
        //TakeDamage(2);
    }

    public override void TakeDamage(float damage, string damageType)
    {
        hp -= damage;
        damageUI damageNumber = Instantiate(damageText, transform.position + new Vector3 (Random.Range(-1, 1.5f), 0 , 0), Quaternion.identity).GetComponent<damageUI>();
        damageNumber.showUIDamage(Mathf.FloorToInt(damage));
        int rand = Random.Range(0, 100);
        if (damageType != "" && rand >= resistance)
        {
            TakenAttactk(damageType);
        }
        if (hp <= 0)
        {
            rid.simulated = false;
            hp = 0;
            anim.Play("die");
        }
        //Debug.LogError(hp);

    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public override void TakenAttactk(string type)
    {
        switch (type)
        {
            case "fire":
                if (!isburnning)
                {
                    if (!isWater)
                    {
                        Burnning(burnningDamage);
                        fireCoroutine.Add(StartCoroutine(DoBlinks(Color.red, 30, burnningTime / 30)));
                        isburnning = true;
                        burnningEffect.SetActive(true);
                    }
                }
                else
                {
                    foreach (Coroutine item in fireCoroutine)
                    {
                        StopCoroutine(item);
                    }

                    Burnning(burnningDamage);
                    fireCoroutine.Add(StartCoroutine(DoBlinks(Color.red, 30, burnningTime / 30)));
                }
                break;
            case "water":
                if (!isWater)
                {
                    if (isburnning)
                    {
                        foreach (Coroutine item in fireCoroutine)
                        {
                            StopCoroutine(item);
                        }
                        fireCoroutine.Clear();
                        isburnning = false;
                        burnningEffect.SetActive(false);
                    }
                    waterCoroutine.Add(StartCoroutine(DoBlinks(Color.blue, 30, waterTime / 30)));
                    moveSpeed = ownSpeed * 0.5f;
                    //Debug.Log(moveSpeed);
                    isWater = true;
                    waterEffect.SetActive(true);
                }
                else
                {
                    foreach (Coroutine item in waterCoroutine)
                    {
                        StopCoroutine(item);
                    }
                    waterCoroutine.Add(StartCoroutine(DoBlinks(Color.blue, 30, waterTime / 30)));
                }
                break;
            case "earth":
                if (!isEarth)
                {
                    IncreaseDizzinessValue(increaseDizziness);
                }
                break;
            case "lightning":
                if (!isLighting)
                {
                    isLighting = true;
                    lightingEffect.SetActive(true);
                    StartCoroutine(ContinueDamage(lightningDamage, lightningTimes, lightningTime / lightningTimes, "lightning"));
                    Debug.Log("lightning");
                }
                else
                {
                    foreach (Coroutine item in lightningCoroutine)
                    {
                        StopCoroutine(item);
                    }
                    lightningCoroutine.Add(StartCoroutine(ContinueDamage(lightningDamage, lightningTimes, lightningTime / lightningTimes, "lightning")));
                }
                break;
            case "metal":
                GoBack(backDis);
                //Debug.Log("metal");
                break;
        }
    }

    private void IncreaseDizzinessValue(int value)
    {
        ownDizzinessValue += value;
        if (ownDizzinessValue >= dizzinessValue)
        {
            Dizziness();
        }
    }
    public override void Dizziness()
    {
        isEarth = true;
        earthCoroutine.Add(StartCoroutine(DoBlinks(new Color(1, 1, 1, 0), 30, dizzinessTime / 30)));
        moveSpeed = 0;
        ownDizzinessValue = 0;
        Debug.Log("earth");
    }

    public override void ExitDizziness()
    {
        foreach (Coroutine item in earthCoroutine)
        {
            StopCoroutine(item);
        }
        earthCoroutine.Clear();
        isEarth = false;
        moveSpeed = ownSpeed;
        ownDizzinessValue = 0;
        mySprite.color = Color.white;
        Debug.Log("no earth");
        // throw new System.NotImplementedException();
    }
    IEnumerator DoBlinks(Color blinkColor, int numBlinks, float time)
    {
        //isBlink = true;
        for (int i = 0; i < numBlinks; i++)
        {
            if (i % 2 == 0)
                mySprite.color = blinkColor;
            else
                mySprite.color = Color.white;
            yield return new WaitForSeconds(time);
        }
        mySprite.color = Color.white;

        if (blinkColor == Color.blue)
        {
            waterCoroutine.Clear();
            moveSpeed = ownSpeed;
            isWater = false;
            waterEffect.SetActive(false);
        }
        else if (blinkColor == Color.red)
        {
            isburnning = false;
            fireCoroutine.Clear();
            burnningEffect.SetActive(false);
        }
        else if (blinkColor == new Color(1, 1, 1, 0))
        {
            ExitDizziness();
        }

        //Debug.LogError(moveSpeed);
    }

    public override void Burnning(float damage)
    {
        fireCoroutine.Add(StartCoroutine(ContinueDamage(damage, burnningTimes, burnningTime / burnningTimes, "fire")));
    }

    IEnumerator ContinueDamage(float damage, int damageTimes, float time, string damageType)
    {
        for (int i = 0; i < damageTimes; i++)
        {
            if (isWater && damageType == "lightning")
            {
                damage += 10;
            }
            TakeDamage(damage, "");
            if(damageType == "lightning")
            {
                Paralysis(paralysisTime);
            }
            yield return new WaitForSeconds(time);
        }
        if (damageType == "lightning")
        {
            isLighting = false;
            lightingEffect.SetActive(false);
            lightningCoroutine.Clear();
        }
    }

    public override void GoBack(float dis)
    {
        startPos = transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        destination = transform.position + (mousePos - transform.position).normalized * dis;
        destination.z = 0;
        canGoBack = true;
    }
    public override void Move()
    {
        throw new System.NotImplementedException();
    }

    public override void Paralysis(float time)
    {
        if (isWater)
            bigLightingEffect.SetActive(true);
        isParalysis = true;
        moveSpeed = 0;
        mySprite.color = Color.yellow;
        anim.enabled = false;
        mySprite.sprite = paralysisSprit;
        Invoke(nameof(CancelParalysis), time); 
    }

    private void CancelParalysis()
    {
        moveSpeed = ownSpeed;
        isParalysis = false;
        anim.enabled = true;
        mySprite.color = Color.white;
        bigLightingEffect.SetActive(false);
    }
}
