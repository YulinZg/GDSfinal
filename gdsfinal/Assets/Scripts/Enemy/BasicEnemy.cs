using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    private float chasingRange;
    //private float changeStateTimer;
    //private float attackTimer;
    private float attackInterval;
    private float stuckWallTimer;
    private float stuckWallInterval;
    private Vector2 temp;
    public EnemyAttack enemyAttack;
    private enum EnemyState
    {
        Wander,
        Chase,
        Attack,
    };

    public override void Move()
    {
        rigid.velocity = speed * moveDir;
        Filp("normal");
    }

    private EnemyState currentState;
    private void Awake()
    {
        stuckWallInterval = 3f;
        isAlive = true;
        player = PlayerController.instance.transform;
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        GameManagement.instance.enemyCount--;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        attackInterval = GetLengthByName("attack");
        currentSpeed = speed = moveSpeed;
        chasingRange = Random.Range(0.8f, 1.5f);
        foreach (GameObject point in GameObject.FindGameObjectsWithTag("Point"))
        {
            pathPointsPos.Add(point.transform.position);
        }
        GetNewTargetPoint();
        enemyAttack.attack = attack;
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
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
        
        switch (currentState)
        {
            case EnemyState.Wander:
                if (IsPlayerInSense() || beAttacked)
                {
                    chasingRange = Random.Range(0.9f, 2f);
                    desTraget = (Vector2)player.position + new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
                    currentState = EnemyState.Chase;
                }
                else
                    Wander();
                break;
            case EnemyState.Chase:
                if (Vector2.Distance((Vector2)player.position, (Vector2)transform.position) <= 0.8f )
                {
                    speed = 0;
                    anim.SetBool("isIdle", true);
                    if (attackTimer >= attackInterval)
                    {
                        currentState = EnemyState.Attack;
                        //attackTimer = 0;
                        anim.SetBool("isIdle", false);
                    }
                }
                else
                    Chasing();
                break;
            case EnemyState.Attack:
                moveDir = ((Vector2)player.position - (Vector2)transform.position).normalized;
                if (Vector2.Distance((Vector2)player.position, (Vector2)transform.position) > 0.9f && attackTimer >= attackInterval - 0.75f)
                {
                    chasingRange = Random.Range(0.9f, 2f);
                    currentState = EnemyState.Chase;
                    desTraget = (Vector2)player.position;
                    attackTimer = 0;
                    StopAttack();
                }
                else if (attackTimer >= attackInterval)
                {
                    Attack();
                    attackTimer = 0;
                }
                break;
            default:
                break;
        }
    }

    private void Chasing()
    {
        anim.SetBool("isIdle", false);
        speed = currentSpeed;
        stuckWallTimer += Time.deltaTime;
        if (Vector2.Distance(desTraget, transform.position) < chasingRange)
        {
            temp = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            desTraget = (Vector2)player.position + temp;
        }
        else if (stuckWallTimer > stuckWallInterval)
        {
            stuckWallTimer = 0;
            temp = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            desTraget = (Vector2)player.position + temp;
        }
        else if (Vector2.Distance(desTraget, transform.position) > Vector2.Distance((Vector2)player.position + temp, transform.position))
        {
            temp = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            desTraget = (Vector2)player.position + temp;
        }
        moveDir = ((Vector2)desTraget - (Vector2)transform.position).normalized;
    }
    private void Wander()
    {
        if ( Vector2.Distance(desTraget, transform.position) < chasingRange)
        {
            GetNewTargetPoint();
        }
        moveDir = ((Vector2)desTraget - (Vector2)transform.position).normalized;
    }

    private void Attack()
    {
        speed = 0;
        isAttacking = true;
        anim.SetBool("isAttacking", true);
        
        //需要让怪物面向玩家
    }

    private void StopAttack()
    {
        speed = currentSpeed;
        isAttacking = false;
        anim.SetBool("isAttacking", false);
    }
}
