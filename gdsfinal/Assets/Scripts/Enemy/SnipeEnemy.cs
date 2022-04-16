using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipeEnemy : Enemy
{
    
    [Header("Own")]
    public GameObject child;
    public int numberOfChildren;
    private float attackTimer;
    private float attackInterval;
    private enum ChasingTarget
    {
        none,
        up,
        down,
        right,
        left
    }
    private ChasingTarget chasingTarget;
    private Vector2 chasingOffset;
    private enum EnemyState
    {
        Chase,
        Attack,
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
    // Start is called before the first frame update
    void Start()
    {
        attackInterval = GetLengthByName("attack");
        chasingTarget = (ChasingTarget)Random.Range(0, 5);
        switch (chasingTarget)
        {
            case ChasingTarget.none:
                chasingOffset = Vector2.zero;
                break;
            case ChasingTarget.up:
                chasingOffset = Vector2.up * 0.5f;
                break;
            case ChasingTarget.down:
                chasingOffset = Vector2.down * 0.5f;
                break;
            case ChasingTarget.right:
                chasingOffset = Vector2.right * 0.5f;
                break;
            case ChasingTarget.left:
                chasingOffset = Vector2.left * 0.5f;
                break;
            default:
                break;
        }
        
        currentSpeed = speed = moveSpeed;
        //chasingRange = Random.Range(0.9f, 2f);
        pathPoints = GameObject.FindGameObjectsWithTag("Point");
    }

    public override void UpdateState()
    {
        attackTimer += Time.deltaTime;
        switch (currentState)
        {
            case EnemyState.Chase:
                if (Vector2.Distance((Vector2)player.position, (Vector2)transform.position) <= 0.9f)
                {
                    speed = 0;
                    //anim.SetBool("isIdle", true);
                    if (attackTimer >= attackInterval)
                    {
                        currentState = EnemyState.Attack;
                        attackTimer = 0;
                        //anim.SetBool("isIdle", false);
                    }
                }
                else
                    Chasing();
                break;
            case EnemyState.Attack:
                if (Vector2.Distance((Vector2)player.position, (Vector2)transform.position) > 0.9f)
                {
                    //chasingRange = Random.Range(0.9f, 2f);
                    currentState = EnemyState.Chase;
                    desTraget = (Vector2)player.position;
                    //changeStateTimer = 0f;
                    StopAttack();
                }
                else
                    Attack();
                break;
            default:
                break;
        }
    }

    private void Attack()
    {
        speed = 0;
        isAttacking = true;
        anim.SetBool("isAttacking", true);
        moveDir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        //需要让怪物面向玩家
    }
    private void Chasing()
    {
        speed = currentSpeed;
        moveDir = ((Vector2)player.position + chasingOffset - (Vector2)transform.position).normalized;
    }
    public void SpawnChildren()
    {
        for (int i = 0; i < numberOfChildren; i++)
        {
            Instantiate(child, transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0), Quaternion.identity);
        }
    }
    private void StopAttack()
    {
        speed = currentSpeed;
        isAttacking = false;
        anim.SetBool("isAttacking", false);
    }
}
