using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipeEnemy : Enemy
{
    //private float chasingRange;
    //private float changeStateTimer;
    private float attackTimer;
    private float attackInterval;

    private float idleTimer;
    public float idleInterval;

    private bool isSensePlayer;
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
        if (!isStun && !isHurt)
        {
            UpdateState();
        }
    }

    private void FixedUpdate()
    {
        if (!isRepel)
        {
            Move();
        }
    }

    public override void UpdateState()
    {
        attackTimer += Time.deltaTime;
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
                else if(IsPlayerInSense())
                {
                    anim.SetBool("isIdle", true);
                    //isSensePlayer = true;
                    speed = 0;
                    if (attackTimer >= attackInterval)
                    {
                        currentState = EnemyState.Attack;
                        attackTimer = 0;
                    }
                }else
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
                else
                    Attack();
                Debug.Log("attack");
                break;
            case EnemyState.back:
                break;
            default:
                break;
        }
    }

    private void Idle()
    {
        idleTimer += Time.deltaTime;
        speed = 0;
        
    }

    private void Wander()
    {
        
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
}
