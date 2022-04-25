using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchFish : Enemy
{
    private float chasingRange;
    private bool canGiveAward = true;
    private void Awake()
    {
        isAlive = true;
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = PlayerController.instance.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("selfDisappear", 20f);
        currentSpeed = speed = moveSpeed;
        chasingRange = Random.Range(0.9f, 2f);
        foreach (GameObject point in GameObject.FindGameObjectsWithTag("Point"))
        {
            pathPointsPos.Add(point.transform.position);
        }
        GetNewTargetPoint();
    }

    private void selfDisappear()
    {
        Die();
        canGiveAward = false;
    }
    public override void Move()
    {
        rigid.velocity = speed * moveDir;
        Filp("normal");
    }

    public override void UpdateState()
    {
        if (desTraget == new Vector2(0, 0) || Vector2.Distance(desTraget, transform.position) < chasingRange)
        {
            GetNewTargetPoint();
        }
        moveDir = ((Vector2)desTraget - (Vector2)transform.position).normalized;
    }

    // Start is called before the first frame update
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

    private void OnDestroy()
    {
        GameManagement.instance.enemyCount--;
        Debug.LogError(GameManagement.instance.enemyCount);
        if (canGiveAward)
        {
            Debug.Log("µôÂäÂß¼­ÔÚÕâÀï");
        }
    }
}
