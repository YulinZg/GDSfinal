using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [SerializeField] private GameObject arrow;
    public bool canInput = true;
    public bool canRotate = true;
    public bool isAttacking = false;

    private float speed;
    private Vector3 moveDir;
    private Rigidbody2D rigid;
    private Camera cam;
    private Vector3 mousePos;
    private Vector3 mouseDir;
    private Weapon weapon;
    private Animator animator;
    private bool isFacingRight = true;

    private void Awake()
    {
        instance = this;
        rigid = GetComponent<Rigidbody2D>();
        weapon = GetComponent<Weapon>();
        cam = Camera.main;
        animator = GetComponent<Animator>();
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

    void Move()
    {
        rigid.velocity = speed * moveDir;
        if (isAttacking && mouseDir.x >= 0)
        {
            animator.SetBool("isRightAttacking", true);
            animator.SetBool("isLeftAttacking", false);
        }
        else if (isAttacking && mouseDir.x < 0)
        {
            animator.SetBool("isRightAttacking", false);
            animator.SetBool("isLeftAttacking", true);
        }
        else if (!isAttacking)
        {
            animator.SetBool("isRightAttacking", false);
            animator.SetBool("isLeftAttacking", false);
        }
        if (rigid.velocity.x > 0)
        {
            isFacingRight = true;
            animator.SetBool("isMovingRight", isFacingRight);
            animator.SetBool("isMovingLeft", !isFacingRight);
        }
        else if (rigid.velocity.x < 0)
        {
            isFacingRight = false;
            animator.SetBool("isMovingRight", isFacingRight);
            animator.SetBool("isMovingLeft", !isFacingRight);
        }
        else
        {
            if (rigid.velocity.y == 0)
            {
                animator.SetBool("isMovingRight", false);
                animator.SetBool("isMovingLeft", false);
            }
            else
            {
                animator.SetBool("isMovingRight", isFacingRight);
                animator.SetBool("isMovingLeft", !isFacingRight);
            }
        }
    }

    void RotateArrow()
    {
        if (canRotate)
        {
            float angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }

    IEnumerator DoDash(Vector3 dir, float distance, float time, float resetSpeed, bool setBack, bool rotate)
    {
        float timer = 0;
        speed = 0;
        canInput = false;
        canRotate = rotate;
        isAttacking = true;
        Vector3 start = transform.position;
        while (timer < time - Time.fixedDeltaTime)
        {
            rigid.MovePosition(Vector3.Lerp(start, start + dir * distance, timer/ time));
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        isAttacking = !setBack;
        speed = resetSpeed;
        canInput = setBack;
        canRotate = setBack;
    }

    public void Dash(Vector3 dir, float distance, float time, float resetSpeed, bool setBack, bool rotate)
    {
        StartCoroutine(DoDash(dir, distance, time, resetSpeed, setBack, rotate));
    }
}
