using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [SerializeField] private GameObject arrow;

    private float speed;
    private bool canInput = true;
    private bool canRotate = true;
    private bool isFacingRight = true;
    private bool isAttacking = false;
    private float attackTimer;
    public bool isHurting = false;
    private float hurtTimer;
    private Camera cam;
    private Animator anim;
    private Rigidbody2D rigid;
    private Vector3 moveDir;
    private Vector3 mousePos;
    private Vector3 mouseDir;
    private Weapon weapon;
    private Status status;

    private void Awake()
    {
        instance = this;
        cam = Camera.main;
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        weapon = GetComponent<Weapon>();
        status = GetComponent<Status>();
    }

    // Start is called before the first frame update
    void Start()
    {
        weapon.GetWeapon(Weapon.Property.fire);
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = 0;
        float moveY = 0;
        if (Input.GetKey(KeyCode.W))
        {
            moveY = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.W))
                moveY = 0;
            else
                moveY = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.D))
                moveX = 0;
            else
                moveX = -1f;
        }
        moveDir = new Vector3(moveX, moveY).normalized;
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void LateUpdate()
    {
        if (canInput && !isAttacking)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                weapon.GetWeapon(Weapon.Property.fire);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                weapon.GetWeapon(Weapon.Property.water);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                weapon.GetWeapon(Weapon.Property.earth);
            if (Input.GetKeyDown(KeyCode.Alpha4))
                weapon.GetWeapon(Weapon.Property.lightning);
            if (Input.GetKeyDown(KeyCode.Alpha5))
                weapon.GetWeapon(Weapon.Property.metal);
        }
    }

    private void FixedUpdate()
    {
        mouseDir = ((Vector2)mousePos - (Vector2)transform.position).normalized;
        Move();
        RotateArrow();
    }

    private void Move()
    {
        rigid.velocity = speed * moveDir;
        if (rigid.velocity.x > 0)
        {
            isFacingRight = true;
            anim.SetBool("isMovingRight", isFacingRight);
            anim.SetBool("isMovingLeft", !isFacingRight);
        }
        else if (rigid.velocity.x < 0)
        {
            isFacingRight = false;
            anim.SetBool("isMovingRight", isFacingRight);
            anim.SetBool("isMovingLeft", !isFacingRight);
        }
        else
        {
            if (rigid.velocity.y == 0)
            {
                anim.SetBool("isMovingRight", false);
                anim.SetBool("isMovingLeft", false);
            }
            else
            {
                anim.SetBool("isMovingRight", isFacingRight);
                anim.SetBool("isMovingLeft", !isFacingRight);
            }
        }
    }

    private void RotateArrow()
    {
        if (canRotate)
        {
            float angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void Die()
    {
        //Debug.LogError("Die");
    }

    public void SetSpeed(float s)
    {
        speed = s * status.GetSpeed();
    }

    public void Attack(Vector3 dir, float time, bool ifStop, float resetSpeed, bool canRotate)
    {
        attackTimer = 0;
        if (!isAttacking)
            StartCoroutine(Attacking(dir, time, ifStop, resetSpeed, canRotate));
    }

    IEnumerator Attacking(Vector3 dir, float time, bool ifStop, float resetSpeed, bool canRotate)
    {
        isAttacking = true;
        yield return null;
        canInput = false;
        this.canRotate = canRotate;
        if (ifStop)
            SetSpeed(0);
        if (dir.x >= 0)
        {
            anim.SetBool("isRightAttacking", true);
            anim.SetBool("isLeftAttacking", false);
        }
        else
        {
            anim.SetBool("isRightAttacking", false);
            anim.SetBool("isLeftAttacking", true);
        }
        while (attackTimer <= time - Time.deltaTime * 2)
        {
            attackTimer += Time.deltaTime;
            if (!ifStop)
            {
                if (mouseDir.x >= 0)
                {
                    anim.SetBool("isRightAttacking", true);
                    anim.SetBool("isLeftAttacking", false);
                }
                else
                {
                    anim.SetBool("isRightAttacking", false);
                    anim.SetBool("isLeftAttacking", true);
                }
            }
            yield return null;
        }
        if (ifStop)
            SetSpeed(resetSpeed);
        this.canRotate = true;
        canInput = true;
        isAttacking = false;
        anim.SetBool("isRightAttacking", false);
        anim.SetBool("isLeftAttacking", false);
    }

    public void Dash(Vector3 dir, float distance, float time)
    {
        StartCoroutine(Dashing(dir, distance, time));
    }

    IEnumerator Dashing(Vector3 dir, float distance, float time)
    {
        float timer = 0;
        Vector3 start = transform.position;
        while (timer < time - Time.fixedDeltaTime)
        {
            rigid.MovePosition(Vector3.Lerp(start, start + dir * distance, timer/ time));
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    public void Hurt(float time)
    {
        hurtTimer = 0;
        anim.SetBool("isHurt", true);
        if (isFacingRight)
            anim.Play("takeDamage_right", 0, 0);
        else
            anim.Play("takeDamage_left", 0, 0);
        if (!isHurting)
        {
            StopAllCoroutines();
            StartCoroutine(Hurting(time));
        }
    }

    IEnumerator Hurting(float time)
    {
        isHurting = true;
        weapon.StartHurt();
        speed = 0;
        while (hurtTimer < time)
        {
            hurtTimer += Time.deltaTime;
            yield return null;
        }
        isHurting = false;
        weapon.FinishHurt();
        canRotate = true;
        canInput = true;
        isAttacking = false;
        anim.SetBool("isRightAttacking", false);
        anim.SetBool("isLeftAttacking", false);
        anim.SetBool("isHurt", false);
    }
}
