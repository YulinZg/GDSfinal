using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipeEnemy : Enemy
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

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }
}
