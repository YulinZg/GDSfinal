using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfBurstingEnemy : Enemy
{
    //private float chasingRange;
    public float chargeTime;
    private float timer = 0f;

    private LayerMask invincible;
    private enum EnemyState
    {
        Chase,
        Charge,
        Burst
    };
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
        chargeTime = 2f;
        isAlive = true;
        currentSpeed = speed = moveSpeed;
        invincible = 1 << LayerMask.NameToLayer("Invincible");
        //chasingRange = Random.Range(0.9f, 2f);
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

    public override void Move()
    {
        rigid.velocity = speed * moveDir;
        Filp("");
    }
    private void Chasing()
    {
        //if (Vector2.Distance(desTraget, transform.position) < chasingRange)
        //{
        //    desTraget = (Vector2)player.position + new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        //}
        moveDir = ((Vector2)player.position - (Vector2)transform.position).normalized;
    }
    public override void UpdateState()
    {
        switch (currentState)
        {
            case EnemyState.Chase:
                Chasing();
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("die"))
                {
                    currentState = EnemyState.Charge;
                    rigid.simulated = true;
                    canBeAttacked = false;
                    gameObject.layer = 8;
                    speed = 0;
                }
                if (Vector2.Distance((Vector2)player.position, (Vector2)transform.position) <= 1.2f)
                {
                    //Die();
                    currentState = EnemyState.Charge;
                    canBeAttacked = false;
                    gameObject.layer = 8;
                    anim.Play("die");
                    speed = 0;
                }
                break;
            case EnemyState.Charge:
                timer += Time.deltaTime;
                if (timer >= chargeTime )
                {
                    currentState = EnemyState.Burst;
                }   
                break;
            case EnemyState.Burst:
                CanBurst();
                isAlive = false;
                break;
            default:
                break;
        }
    }

    private void CanBurst()
    {
        speed = 0;
        anim.SetBool("canBursting", true);
    }
}
