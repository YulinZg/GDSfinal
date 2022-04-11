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
        Dead,
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
        rigid.velocity = speed * moveDir;
    }

    public override void UpdateState()
    {
        switch (currentState)
        {
            case EnemyState.Wander:
                Wander();
                Debug.LogError("wander");
                if (IsPlayerInView())
                {
                    desTraget = player;
                    currentState = EnemyState.Chase;
                }
                break;
            case EnemyState.Chase:
                Chasing();
                Debug.LogError("chasing");
                if (!IsPlayerInView())
                {
                    GetNewTargetPoint();
                    currentState = EnemyState.Wander;
                }
                if ((player.position - transform.position).magnitude < 1f)
                {
                    currentState = EnemyState.Attack;
                }
                break;
            case EnemyState.Attack:
                speed = 0;
                Debug.LogError("Attack");
                break;
            case EnemyState.Dead:
                break;
            default:
                break;
        }
    }

    private void Chasing()
    {
        moveDir = ((Vector2)desTraget.position + new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)) - (Vector2)transform.position).normalized;
    }
    private void Wander()
    {
        if (!desTraget || (desTraget.position - transform.position).magnitude < 0.1f)
        {
            GetNewTargetPoint();
        }
        moveDir = ((Vector2)desTraget.position - (Vector2)transform.position).normalized;
    }
}