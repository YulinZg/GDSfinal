using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] private GameObject arrow;
    [SerializeField] private WeaponUI weaponUI;

    private Camera cam;
    private Animator anim;
    private Rigidbody2D rigid;
    private Vector3 moveDir;
    private Vector3 mousePos;
    private Vector3 mouseDir;
    private Weapon weapon;
    private Status status;

    private float speed;
    private bool canInput = true;
    private bool canRotate = true;
    private bool isFacingRight = true;
    private bool isAttacking = false;
    private float attackTimer;
    public bool isHurting = false;
    private float hurtTimer;

    public bool canSuckBlood = false;
    public float suckBloodAmount = 5;

    private List<Weapon.Property> weapons = new List<Weapon.Property>
    {
        Weapon.Property.fire,
        Weapon.Property.water,
        Weapon.Property.earth,
        Weapon.Property.lightning,
        Weapon.Property.metal
    };
    private Weapon.Property weapon1;
    private Weapon.Property weapon2;
    private Weapon.Property currentWeapon;


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
        GetWeapons();
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
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftControl)) && canInput && !isAttacking)
            SwitchWeapon();
        if (Input.GetKeyDown(KeyCode.R) && canInput && !isAttacking)
            ChangeWeapon();
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

    private void GetWeapons()
    {
        int i = Random.Range(0, weapons.Count);
        weapon1 = weapons[i];
        weaponUI.SetWeapon1Icon(SpriteNo(weapon1));
        weapons.RemoveAt(i);
        i = Random.Range(0, weapons.Count);
        weapon2 = weapons[i];
        weaponUI.SetWeapon2Icon(SpriteNo(weapon2));
        weapons.RemoveAt(i);
        currentWeapon = weapon1;
        weapon.GetWeapon(currentWeapon);
    }

    private int SpriteNo(Weapon.Property property)
    {
        int n = 0;
        switch (property)
        {
            case Weapon.Property.fire:
                n = 0;
                break;
            case Weapon.Property.water:
                n = 1;
                break;
            case Weapon.Property.earth:
                n = 2;
                break;
            case Weapon.Property.lightning:
                n = 3;
                break;
            case Weapon.Property.metal:
                n = 4;
                break;
        }
        return n;
    }

    private void SwitchWeapon()
    {
        if (currentWeapon == weapon1)
            currentWeapon = weapon2;
        else
            currentWeapon = weapon1;
        weapon.GetWeapon(currentWeapon);
        weaponUI.SwitchWeapon();
    }

    public void ChangeWeapon()
    {
        int i = Random.Range(0, weapons.Count);
        Weapon.Property temp = currentWeapon;
        if (currentWeapon == weapon1)
        {
            currentWeapon = weapon1 = weapons[i];
            weaponUI.SetWeapon1Icon(SpriteNo(weapon1));
        }
        else
        {
            currentWeapon = weapon2 = weapons[i];
            weaponUI.SetWeapon2Icon(SpriteNo(weapon2));
        }
        weapons.RemoveAt(i);
        weapons.Add(temp);
        weapon.GetWeapon(currentWeapon);
    }

    public void SuckBlood()
    {
        if (canSuckBlood)
        {
            status.RestoreHp(suckBloodAmount);
        }
    }
}
