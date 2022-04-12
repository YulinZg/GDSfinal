using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    public enum EnemyState
    {
        Wander,
        Chase,
        Attack,
    };

    public EnemyState currentState;
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
        
        pathPoints = GameObject.FindGameObjectsWithTag("Point");
    }

    // Update is called once per frame
    void Update()
    {
        //IsLookAtWall();
        UpdateState();
        //IsPlayerInView();
        //moveDir = ((Vector2)player.position - (Vector2)transform.position).normalized;
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
        //Debug.DrawRay((Vector2)transform.position + senseOffset, new Vector2(moveDir.x, 0).normalized * rayDis);
        //Debug.Log(1111);
        //IsNearOtherEnemy();
        rigid.velocity = speed * moveDir;
        Filp();
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
                if (Vector2.Distance((Vector2)player.position, (Vector2)transform.position) <= 0.8f)
                {
                    currentState = EnemyState.Attack;
                }
                break;
            case EnemyState.Attack:              
                Attack();
                if (Vector2.Distance((Vector2)player.position, (Vector2)transform.position) > 0.8f)
                {
                    currentState = EnemyState.Chase;
                    desTraget = (Vector2)player.position;
                    StopAttack();
                }
                //Debug.LogError("Attack");
                break;
            default:
                break;
        }
    }

    private void Chasing()
    {
        if (Vector2.Distance(desTraget, transform.position) < 1f)
        {
            desTraget = (Vector2)player.position + new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        }
        moveDir = ((Vector2)desTraget - (Vector2)transform.position).normalized;
    }
    private void Wander()
    {
        if (desTraget == new Vector2(0,0) || Vector2.Distance(desTraget, transform.position) < 1f)
        {
            GetNewTargetPoint();
        }
        moveDir = ((Vector2)desTraget - (Vector2)transform.position).normalized;
    }

    private void Attack()
    {
        speed = 0;
        isAttacking = true;
        moveDir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        //需要让怪物面向玩家
        anim.SetBool("isAttacking", true);
    }

    private void StopAttack()
    {
        speed = currentSpeed;
        isAttacking = false;
        anim.SetBool("isAttacking", false);
    }
}
