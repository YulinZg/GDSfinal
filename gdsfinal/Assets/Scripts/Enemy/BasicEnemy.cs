using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
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
        isAlive = true;
        currentSpeed = speed = moveSpeed;
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
}
