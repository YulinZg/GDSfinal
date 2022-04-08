using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    private float speed;
    private float currentStunValue = 0;
    private float burnTimer = 0;
    private float decelerateTimer = 0;
    private float palsyTimer = 0;
    private Coroutine burnCoroutine;
    private Coroutine decelerateCoroutine;
    private Color currentColor;
    private float currentSpeed;
    private Vector3 moveDir;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = speed = moveSpeed;
        currentColor = sprite.color;
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = ((Vector2)player.position - (Vector2)transform.position).normalized;
    }

    private void FixedUpdate()
    {
        if (!isRepel)
        {
            Move();
        }
    }

    public override void Move()
    {
        rigid.velocity = speed * moveDir;
    }

    public override void TakeDamage(float d)
    {
        float damage = Mathf.Floor(d);
        health -= damage;
        Damage damageUI = Instantiate(damageText, transform.position + new Vector3(Random.Range(-0.5f, 1.0f), Random.Range(0.5f, 1f), 0), Quaternion.identity).GetComponent<Damage>();
        damageUI.ShowUIDamage(damage);
        if (health <= 0)
        {
            health = 0;
            rigid.simulated = false;
            anim.Play("die");
        }
    }

    public override void Burn(float damage, float time, float interval)
    {
        burnTimer = 0;
        if (!isBurn)
            burnCoroutine = StartCoroutine(Burnning(damage * (1 - burnResistance), time * (1 - burnResistance), interval));
        if (isDecelerate)
        {
            isDecelerate = false;
            currentSpeed = speed = moveSpeed;
            StopCoroutine(decelerateCoroutine);
            currentColor = sprite.color = Color.white;
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
                TakeDamage(damage);
                StartCoroutine(DoBlinks(Color.red, 5, 0.1f));
                timer = 0;
            }
            yield return null;
        }
        isBurn = false;
        Destroy(effectInstance);
    }

    public override void Decelerate(float rate, float time)
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
        currentColor = sprite.color = Color.blue;
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
        currentColor = sprite.color = Color.white;
        Destroy(effectInstance);
    }

    public override void Stun(float value, float time)
    {
        if (!isStun)
            currentStunValue += value;
        if (currentStunValue >=  maxStunValue)
        {
            currentStunValue = 0;
            isStun = true;
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

    public override void Palsy(float damage, float time, float interval)
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
                TakeDamage(d);
                StartCoroutine(DoBlinks(Color.yellow, 10, 0.1f));
                StartCoroutine(StopMove(1f));
                timer = 0;
            }
            yield return null;
        }
        isPalsy = false;
        Destroy(effectInstance);
    }

    public override void Repel(float distance)
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
        speed = 0;
        yield return new WaitForSeconds(time);
        yield return null;
        if (!isStun)
            speed = currentSpeed;
    }

    IEnumerator DoBlinks(Color color, int blinkNum, float interval)
    {
        for (int i = 0; i < blinkNum - 1; i++)
        {
            if (i % 2 == 0)
                sprite.color = color;
            else
                sprite.color = currentColor;
            yield return new WaitForSeconds(interval);
        }
        yield return new WaitForSeconds(interval);
        sprite.color = currentColor;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
