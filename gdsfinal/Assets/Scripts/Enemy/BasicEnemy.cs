using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    private float chasingRange;
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
        player = PlayerController.instance.transform;
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        currentSpeed = speed = moveSpeed;
        chasingRange = Random.Range(0.9f, 2f);
        pathPoints = GameObject.FindGameObjectsWithTag("Point");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
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
                Wander();
                //Debug.LogError("wander");
                if (IsPlayerInSense() || beAttacked)
                {
                    chasingRange = Random.Range(0.9f, 2f);
                    desTraget = (Vector2)player.position + new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
                    currentState = EnemyState.Chase;
                }
                break;
            case EnemyState.Chase:
                Chasing();
                //Debug.LogError("chasing");
                //if (!IsPlayerInView() && !beAttacked)
                //{
                //    GetNewTargetPoint();
                //    currentState = EnemyState.Wander;
                //}
                if (Vector2.Distance((Vector2)player.position, (Vector2)transform.position) <= 0.9f)
                {
                    currentState = EnemyState.Attack;
                    isAttacking = true;
                    anim.SetBool("isAttacking", true);
                    speed = 0;
                }
                break;
            case EnemyState.Attack:
                Attack();
                if (Vector2.Distance((Vector2)player.position, (Vector2)transform.position) > 0.9f && !isPalsy && !isStun)
                {
                    chasingRange = Random.Range(0.9f, 2f);
                    currentState = EnemyState.Chase;
                    desTraget = (Vector2)player.position;
                    StopAttack();
                }
                if (isPalsy || isStun)
                {
                    isAttacking = false;
                    anim.SetBool("isAttacking", false);
                }
                //Debug.LogError("Attack");
                break;
            default:
                break;
        }
    }

    private void Chasing()
    {
        if (Vector2.Distance(desTraget, transform.position) < chasingRange)
        {
            desTraget = (Vector2)player.position + new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        }
        moveDir = ((Vector2)desTraget - (Vector2)transform.position).normalized;
    }
    private void Wander()
    {
        if (desTraget == new Vector2(0, 0) || Vector2.Distance(desTraget, transform.position) < chasingRange)
        {
            GetNewTargetPoint();
        }
        moveDir = ((Vector2)desTraget - (Vector2)transform.position).normalized;
    }

    private void Attack()
    {

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
