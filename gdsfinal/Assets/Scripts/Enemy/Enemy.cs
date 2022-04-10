using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public bool isAlive;
    public float health;
    public float moveSpeed;
    public float burnResistance;
    public float decelerateResistance;
    public float maxStunValue;
    public float palsyResistance;
    public float repelResistance;
    
    public Transform player;
    public GameObject damageText;
    public SpriteRenderer sprite;
    public Rigidbody2D rigid;
    public Animator anim;

    public float effectSize; 
    public float effectOffsetY;
    public float damageTextOffsetXLeftLimt;
    public float damageTextOffsetXRightLimt;
    public float damageTextOffsetYBottomLimt;
    public float damageTextOffsetYUpLimt;
    public GameObject burnEffect;
    public GameObject decelerateEffect;
    public GameObject stunEffect;
    public GameObject palsyEffect;

    public bool isBurn = false;
    public bool isDecelerate = false;
    public bool isStun = false;
    public bool isPalsy = false;
    public bool isRepel = false;
    public bool isHurt = false;

    protected float currentSpeed;
    protected float speed;
    protected Vector3 moveDir;

    private float burnTimer = 0;
    private float currentStunValue = 0;
    private float decelerateTimer = 0;
    private float palsyTimer = 0;
    private float stopTimer = 0;
    private Coroutine burnCoroutine;
    private Coroutine decelerateCoroutine;
    
    public abstract void Move();

    public void TakeDamage(float d, float blinkTime, Color blinkColor, bool hurtStop)
    {
        float damage = Mathf.Floor(d);
        health -= damage;
        Damage damageUI = Instantiate(damageText, transform.position + new Vector3(Random.Range(damageTextOffsetXLeftLimt, damageTextOffsetXRightLimt), Random.Range(damageTextOffsetYBottomLimt, damageTextOffsetYUpLimt), 0), Quaternion.identity).GetComponent<Damage>();
        damageUI.ShowUIDamage(damage);
        if (health <= 0)
        {
            health = 0;
            isAlive = false;
            rigid.simulated = false;
            anim.Play("die");
        }
        if (blinkTime != 0)
            StartCoroutine(DoBlinks(blinkColor, (int)(blinkTime / 0.1f), 0.1f));
        if (hurtStop)
        {
            stopTimer = 0;
            if (!isHurt)
            {
                isHurt = true;
                anim.SetBool("isHurt", true);
                StartCoroutine(StopMove(blinkTime));
            }
        }
    }

    public void Burn(float damage, float time, float interval)
    {
        burnTimer = 0;
        if (!isBurn)
            burnCoroutine = StartCoroutine(Burnning(damage * (1 - burnResistance), time * (1 - burnResistance), interval));
        if (isDecelerate)
        {
            isDecelerate = false;
            currentSpeed = speed = moveSpeed;
            StopCoroutine(decelerateCoroutine);
            if (transform.childCount > 0)
                foreach (Transform child in transform)
                {
                    if (child.name == "waterEffect(Clone)")
                        Destroy(child.gameObject);
                }
        }
    }

    IEnumerator Burnning(float damage, float time, float interval)
    {
        float timer = 0;
        isBurn = true;
        GameObject effectInstance = Instantiate(burnEffect, transform);
        effectInstance.transform.localPosition += Vector3.up * effectOffsetY;
        effectInstance.transform.localScale = new Vector3(effectSize, effectSize, 1);
        while (burnTimer <= time)
        {
            burnTimer += Time.deltaTime;
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                TakeDamage(damage, 0.5f, Color.red, false);
                timer = 0;
            }
            yield return null;
        }
        isBurn = false;
        Destroy(effectInstance);
    }

    public void Decelerate(float rate, float time)
    {
        decelerateTimer = 0;
        if (!isDecelerate)
            decelerateCoroutine = StartCoroutine(Decelerating(rate, time * (1 - decelerateResistance)));
        if (isBurn)
        {
            isBurn = false;
            StopCoroutine(burnCoroutine);
            if (transform.childCount > 0)
                foreach (Transform child in transform)
                {
                    if (child.name == "burnEffect(Clone)")
                        Destroy(child.gameObject);
                }
        }
    }

    IEnumerator Decelerating(float rate, float time)
    {
        isDecelerate = true;
        currentSpeed = speed *= 1 - rate;
        GameObject effectInstance = Instantiate(decelerateEffect, transform);
        effectInstance.transform.localPosition += Vector3.up * effectOffsetY;
        effectInstance.transform.localScale = new Vector3(effectSize, effectSize, 1);
        while (decelerateTimer <= time)
        {
            decelerateTimer += Time.deltaTime;
            yield return null;
        }
        isDecelerate = false;
        currentSpeed = speed = moveSpeed;
        Destroy(effectInstance);
    }

    public void Stun(float value, float time)
    {
        if (!isStun)
            currentStunValue += value;
        if (currentStunValue >= maxStunValue)
        {
            currentStunValue = 0;
            isStun = true;
            stopTimer = 0;
            StartCoroutine(StopMove(time));
            StartCoroutine(DoBlinks(Color.gray, (int)(time / 0.1f), 0.1f));
            GameObject effectInstance = Instantiate(stunEffect, transform);
            effectInstance.transform.localPosition += Vector3.up * effectOffsetY;
            effectInstance.transform.localScale = new Vector3(effectSize, effectSize, 1);
            Destroy(effectInstance, time);
            Invoke(nameof(SetNotStun), time);
        }
    }

    private void SetNotStun()
    {
        isStun = false;
        speed = currentSpeed;
    }

    public void Palsy(float damage, float time, float interval)
    {
        palsyTimer = 0;
        if (!isPalsy)
            StartCoroutine(Palsying(damage * (1 - palsyResistance), time * (1 - palsyResistance), interval));
    }

    IEnumerator Palsying(float damage, float time, float interval)
    {
        float timer = 0;
        isPalsy = true;
        GameObject effectInstance = Instantiate(palsyEffect, transform);
        effectInstance.transform.localPosition += Vector3.up * effectOffsetY;
        effectInstance.transform.localScale = new Vector3(effectSize, effectSize, 1);
        while (palsyTimer <= time)
        {
            palsyTimer += Time.deltaTime;
            timer += Time.deltaTime;
            if (isDecelerate)
                effectInstance.transform.localScale = new Vector3(effectSize * 1.5f, effectSize * 1.5f, 1);
            else
                effectInstance.transform.localScale = new Vector3(effectSize, effectSize, 1);
            if (timer >= interval)
            {
                float d = damage;
                if (isDecelerate)
                    d *= 2;
                TakeDamage(d, 0.5f, Color.yellow, true);
                timer = 0;
            }
            yield return null;
        }
        isPalsy = false;
        Destroy(effectInstance);
    }

    public void Repel(float distance)
    {
        Vector3 dir = ((Vector2)transform.position - (Vector2)player.position).normalized;
        StartCoroutine(Repeling(dir, distance * (1 - repelResistance), 0.2f));
    }

    IEnumerator Repeling(Vector3 dir, float distance, float time)
    {
        isPalsy = true;
        float timer = 0;
        speed = 0;
        Vector3 start = transform.position;
        while (timer < time - Time.fixedDeltaTime)
        {
            rigid.MovePosition(Vector3.Lerp(start, start + dir * distance, timer / time));
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        isPalsy = false;
        if (!isStun)
            speed = currentSpeed;
    }

    IEnumerator StopMove(float time)
    {
        bool set = false;
        speed = 0;
        while (stopTimer <= time)
        {
            stopTimer += Time.deltaTime;
            yield return null;
            if (!set)
            {
                set = true;
                anim.SetBool("isHurt", false);
            }
        }
        isHurt = false;
        if (!isStun)
        {
            speed = currentSpeed;
        }
    }

    IEnumerator DoBlinks(Color color, int blinkNum, float interval)
    {
        for (int i = 0; i < blinkNum - 1; i++)
        {
            if (i % 2 == 0)
                sprite.color = color;
            else
                sprite.color = Color.white;
            yield return new WaitForSeconds(interval);
        }
        sprite.color = Color.white;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
