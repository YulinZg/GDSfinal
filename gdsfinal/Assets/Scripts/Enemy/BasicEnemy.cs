using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    private float ownSpeed = 5;
    private List<Coroutine> runTimeCoroutine = new List<Coroutine>();
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        mySprite = GetComponent<SpriteRenderer>();
        hp = 100;
        moveSpeed = ownSpeed;
        isburnning = false;
        isWater = false;
    }

    // Update is called once per frame
    void Update()
    {
        //TakeDamage(2);
    }

    public override void takeDamage(float damage, string damageType)
    {
        hp -= damage;
        if (damageType != "")
        {
            takenAttactk(damageType);
        }
        if (hp <= 0)
        {
            GetComponent<Rigidbody2D>().simulated = false;
            anim.Play("die");
            Debug.Log("died!!!!!!!!");
        }

    }

    public override void takenAttactk(string type)
    {
        switch (type)
        {
            case "fire":
                if (!isburnning)
                {
                    
                    burnning(3f);
                    runTimeCoroutine.Add(StartCoroutine(DoBlinks(Color.red, 30, 3f / 30)));
                    isburnning = true;
                }
                else
                {
                    foreach (Coroutine item in runTimeCoroutine)
                    {
                        StopCoroutine(item);
                        //runTimeCoroutine.Remove(item);
                    }
                    
                    burnning(3f);
                    runTimeCoroutine.Add(StartCoroutine(DoBlinks(Color.red, 30, 3f / 30)));
                }
                break;
            case "water":
                if (!isWater)
                {
                    runTimeCoroutine.Add(StartCoroutine(DoBlinks(Color.blue, 30, 3f / 30)));
                    moveSpeed = ownSpeed * 0.5f;
                    Debug.Log(moveSpeed);
                    isWater = true;
                }
                else
                {
                    foreach (Coroutine item in runTimeCoroutine)
                    {
                        StopCoroutine(item);
                    }
                    runTimeCoroutine.Add(StartCoroutine(DoBlinks(Color.blue, 30, 3f / 30)));
                    //burnning(3f);
                }
                Debug.Log("water");
                break;
            case "earth":
                Debug.Log("earth");
                break;
            case "lightning":
                Debug.Log("lightning");
                break;
            case "metal":
                Debug.Log("metal");
                break;
        }
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
        runTimeCoroutine.Clear();
        if (blinkColor == Color.blue)
            moveSpeed = ownSpeed;
        Debug.LogError(moveSpeed);
    }

    public override void burnning(float damage)
    {
        runTimeCoroutine.Add(StartCoroutine(continueDamage(damage, 6, 3f / 6, "fire")));
    }

    IEnumerator continueDamage(float damage, int damageTimes, float time, string damageType)
    {
        //Debug.Log("new burning");
        for (int i = 0; i < damageTimes; i++)
        {
            takeDamage(damage, "");
            Debug.Log(damage.ToString());
            yield return new WaitForSeconds(time);
        }
        if (damageType == "fire")
        {
            isburnning = false;
           
        }
    }
        
    public override void move()
    {
        //transform.position.x += moveSpeed * Time.deltaTime;
        throw new System.NotImplementedException();
    }
}
