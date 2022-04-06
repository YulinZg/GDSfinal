using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    private void Awake()
    {
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
        if (canInput)
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
            animator.SetBool("isMovingRight", true);
            animator.SetBool("isMovingLeft", false);
        }
        else if (rigid.velocity.x < 0)
        {
            animator.SetBool("isMovingRight", false);
            animator.SetBool("isMovingLeft", true);
        }
        else
        {
            animator.SetBool("isMovingRight", false);
            animator.SetBool("isMovingLeft", false);
        }
    }

    void RotateArrow()
    {
        if (canRotate)
        {
            mouseDir = (mousePos - transform.position).normalized;
            float angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }
}
