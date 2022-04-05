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

    private bool canGoBack;
    private Vector3 startPos;
    private Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        canGoBack = false;
        anim = GetComponent<Animator>();
        mySprite = GetComponent<SpriteRenderer>();
        hp = 100000;
        rid = GetComponent<Rigidbody2D>();
        dizzinessValue = 100;
        moveSpeed = ownSpeed;
        isburnning = false;
        isWater = false;
        isEarth = false;
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

    public override void takeDamage(float damage, string damageType)
    {
        hp -= damage;
        damageUI damageNumber = Instantiate(damageText, transform.position + new Vector3 (Random.Range(-1, 1.5f), 0 , 0), Quaternion.identity).GetComponent<damageUI>();
        damageNumber.showUIDamage(Mathf.FloorToInt(damage));
        if (damageType != "")
        {
            takenAttactk(damageType);
        }
        if (hp <= 0)
        {
            rid.simulated = false;
            anim.Play("die");
        }

    }

    public override void takenAttactk(string type)
    {
        switch (type)
        {
            case "fire":
                if (!isburnning)
                {
                    if (!isWater)
                    {
                        burnning(3f);
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

                    burnning(3f);
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
                    increaseDizzinessValue(2);
                }
                break;
            case "lightning":
                Debug.Log("lightning");
                break;
            case "metal":
                goBack(backDis);
                Debug.Log("metal");
                break;
        }
    }

    private void increaseDizzinessValue(int value)
    {
        ownDizzinessValue += value;
        if (ownDizzinessValue >= dizzinessValue)
        {
            dizziness();
        }
    }
    public override void dizziness()
    {
        isEarth = true;
        earthCoroutine.Add(StartCoroutine(DoBlinks(new Color(1, 1, 1, 0), 30, dizzinessTime / 30)));
        moveSpeed = 0;
        ownDizzinessValue = 0;
        Debug.Log("earth");
    }

    public override void exitDizziness()
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
            exitDizziness();
        }

        //Debug.LogError(moveSpeed);
    }

    public override void burnning(float damage)
    {
        fireCoroutine.Add(StartCoroutine(continueDamage(damage, 6, burnningTime / 6, "fire")));
    }

    IEnumerator continueDamage(float damage, int damageTimes, float time, string damageType)
    {
        for (int i = 0; i < damageTimes; i++)
        {
            takeDamage(damage, "");
            yield return new WaitForSeconds(time);
        }
    }

    public override void goBack(float dis)
    {

        startPos = transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        destination = transform.position + (mousePos - transform.position).normalized * dis;
        destination.z = 0;
        canGoBack = true;
        //Vector3 dir = (mousePos - transform.position).normalized;
        //Vector3 knockbackPosition = (transform.position + (mousePos - transform.position).normalized * dis); // calculation is missing here! Calculate the new position by the knockback direction 
        //transform.position = Vector2.MoveTowards(transform.position, destination, 5 * Time.deltaTime);
        //rid.MovePosition();
    }
    public override void move()
    {
        //transform.position.x += moveSpeed * Time.deltaTime;
        throw new System.NotImplementedException();
    }
}
