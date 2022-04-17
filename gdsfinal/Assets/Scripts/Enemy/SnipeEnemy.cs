using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipeEnemy : Enemy
{
    private float attackTimer;
    private float attackInterval;

    private float idleTimer;
    public float idleInterval;

    public bool isShooting = false;
    public SpriteRenderer shadow;

    private bool isDisappearing;
    private float disappearCoolDownTimer;
    public float disappearCoolDown;
    //public EnemyRotate rotateObject;
    private enum EnemyState
    {
        idle,
        Wander,
        Attack,
        back
    };

    private EnemyState currentState;
    public override void Move()
    {
        rigid.velocity = speed * moveDir;
        Filp("normal");
    }
    private void Awake()
    {
        isAlive = true;
        player = PlayerController.instance.transform;
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //disappearCoolDownTimer = disappearCoolDown;
        attackInterval = GetLengthByName("attack");
        //Debug.Log(desTraget);
        anim.SetBool("isIdle", true);
        currentState = EnemyState.idle;
        attackInterval = GetLengthByName("attack");
        currentSpeed = speed = moveSpeed;
        //chasingRange = Random.Range(0.9f, 2f);
        pathPoints = GameObject.FindGameObjectsWithTag("Point");
        GetNewTargetPoint();
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (!isStun && !isHurt && !isShooting)
        {
            UpdateState();
        }
    }

    private void FixedUpdate()
    {
        if (!isRepel && !isShooting)
        {
            Move();
        }
    }
    public override void UpdateState()
    {
        attackTimer += Time.deltaTime;
        disappearCoolDownTimer += Time.deltaTime;
        switch (currentState)
        {
            case EnemyState.idle:
                if (IsPlayerInSense())
                {
                    anim.SetBool("isIdle", true);
                    //isSensePlayer = true;
                    speed = 0;
                    if (attackTimer >= attackInterval)
                    {
                        currentState = EnemyState.Attack;
                        attackTimer = 0;
                    }
                }
                else if (idleTimer >= idleInterval)
                {
                    speed = currentSpeed;
                    anim.SetBool("isIdle", false);
                    currentState = EnemyState.Wander;
                    idleTimer = 0;
                }
                //else if (Vector2.Distance(player.position, transform.position) < 3f)
                //{
                //    currentState = EnemyState.back;
                //    StartCoroutine(Disappear(1.5f));
                //}
                else
                    Idle();
                break;
            case EnemyState.Wander:
                if (Vector2.Distance(desTraget, transform.position) < 0.3f)
                {
                    currentState = EnemyState.idle;
                    anim.SetBool("isIdle", true);
                    GetNewTargetPoint();
                }
                else if (IsPlayerInSense())
                {
                    anim.SetBool("isIdle", true);
                    //isSensePlayer = true;
                    speed = 0;
                    if (attackTimer >= attackInterval)
                    {
                        currentState = EnemyState.Attack;
                        attackTimer = 0;
                    }
                }
                //else if (Vector2.Distance(player.position, transform.position) < 3f)
                //{
                //    currentState = EnemyState.back;
                //    StartCoroutine(Disappear(1.5f));
                //}
                else
                    Wander();
                break;
            case EnemyState.Attack:
                if (!IsPlayerInSense())
                {
                    StopAttack();
                    GetNewTargetPoint();
                    anim.SetBool("isIdle", false);
                    currentState = EnemyState.Wander;
                }
                else if (Vector2.Distance(player.position, transform.position) < 3f)
                {
                    if (disappearCoolDownTimer >= disappearCoolDown)
                    {
                        currentState = EnemyState.back;
                        StartCoroutine(Disappear(1.5f));
                        anim.SetBool("isIdle", true);
                        StopAttack();
                        speed = 0;
                        disappearCoolDownTimer = 0;
                    }
                    //goToTheFarthestPoint();
                }
                else
                    Attack();
                break;
            case EnemyState.back:
                if (IsPlayerInSense() && !isDisappearing)
                {
                    anim.SetBool("isIdle", true);
                    //isSensePlayer = true;
                    speed = 0;
                    if (attackTimer >= attackInterval)
                    {
                        currentState = EnemyState.Attack;
                        attackTimer = 0;
                    }
                }
                break;
            default:
                break;
        }
    }
    private void goToTheFarthestPoint()
    {
        List<GameObject> temp = getSomeFartherPoints();
        transform.position = temp[Random.Range(0, temp.Count)].transform.position;
        temp.Clear();
    }

    IEnumerator Disappear(float duration)
    {
        isDisappearing = true;
        rigid.simulated = false;
        for (int i = 254; i >= 0; i--)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, (float)i / 255f);
            //Debug.LogError(sprite.color);
            //if (i <= 60)
            //{
            shadow.color = new Color(shadow.color.r, shadow.color.g, shadow.color.b, (float)i * 0.235f / 255f);
            //}
            yield return new WaitForSeconds(duration / 255f);
        }
        goToTheFarthestPoint();
        StartCoroutine(Appear(duration));
        //shadow.SetActive(false);
    }

    IEnumerator Appear(float duration)
    {
        for (int i = 1; i <= 255; i++)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, (float)i / 255f);
            //Debug.LogError(sprite.color);
            //if (i <= 60)
            //{
            shadow.color = new Color(shadow.color.r, shadow.color.g, shadow.color.b, (float)i * 0.235f / 255f);
            //}
            yield return new WaitForSeconds(duration / 255f);
        }
        isDisappearing = false;
        rigid.simulated = true;
    }
    private List<GameObject> getSomeFartherPoints()
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject point in pathPoints)
        {
            if (Vector2.Distance(player.position, point.transform.position) >= 5f && Vector2.Distance(player.position, point.transform.position) <= 10f)
            {
                temp.Add(point);
            }
        }
        return temp;
    }
    private void Wander()
    {
        anim.SetBool("isIdle", false);
        speed = currentSpeed;
        moveDir = ((Vector2)desTraget - (Vector2)transform.position).normalized;
        //Debug.Log(moveDir);
    }

    private void Attack()
    {
        speed = 0;
        isAttacking = true;
        anim.SetBool("isAttacking", true);
        moveDir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        //需要让怪物面向玩家
    }

    private void StopAttack()
    {
        speed = currentSpeed;
        isAttacking = false;
        anim.SetBool("isAttacking", false);
    }
    private void Idle()
    {
        idleTimer += Time.deltaTime;
        speed = 0;
    }
    public void StartDisappear(float duration)
    {
        StartCoroutine(DieDisappear(duration));
    }
    IEnumerator DieDisappear(float duration)
    {
        for (int i = 254; i >= 0; i--)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, (float)i / 255f);
            shadow.color = new Color(shadow.color.r, shadow.color.g, shadow.color.b, (float)i * 0.235f / 255f);
            yield return new WaitForSeconds(duration / 255f);
        }
    }
}
