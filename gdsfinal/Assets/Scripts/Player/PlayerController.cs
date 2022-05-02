using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] private GameObject arrow;
    [SerializeField] private WeaponUI weaponUI;
    [SerializeField] private StatusUI statusUI;

    private Camera cam;
    private Animator anim;
    private Rigidbody2D rigid;
    private Vector3 moveDir;
    private Vector3 lastDir = Vector3.down;
    private Vector3 mousePos;
    private Vector3 mouseDir;
    private Weapon weapon;
    private Status status;

    public bool canInput;
    private float speed;
    private bool canChangeWeapon = true;
    private bool canRotate = true;
    private bool isFacingRight = true;
    public bool isAttacking = false;
    public bool isSkilling = false;
    private float attackTimer;
    public bool canAvoid = true;
    public bool isAvoiding = false;
    private bool layerBack = false;
    public float avoidSpeed;
    public bool isHurting = false;
    private float hurtTimer;

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
        if (moveX != 0 || moveY != 0)
            lastDir = moveDir;
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.Space) && !isAvoiding && canAvoid)
        {
            Avoid();
        }
    }

    private void LateUpdate()
    {
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Q)) && canChangeWeapon && !isAttacking)
            SwitchWeapon();
        if (Input.GetKeyDown(KeyCode.R) && canChangeWeapon && !isAttacking)
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
        if (!isAvoiding)
            rigid.velocity = speed * status.GetSpeed() * moveDir;
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
        if (canRotate && canInput)
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
        speed = s;
    }

    public void Attack(Vector3 dir, float time, bool ifStop, float resetSpeed, bool canRotate, bool isSkill)
    {
        attackTimer = 0;
        if (!isAttacking)
            StartCoroutine(Attacking(dir, time, ifStop, resetSpeed, canRotate, isSkill));
    }

    IEnumerator Attacking(Vector3 dir, float time, bool ifStop, float resetSpeed, bool canRotate, bool isSkill)
    {
        isAttacking = true;
        isSkilling = isSkill;
        canAvoid = !ifStop;
        yield return null;
        canChangeWeapon = false;
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
        canChangeWeapon = true;
        isAttacking = false;
        isSkilling = false;
        canAvoid = true;
        anim.SetBool("isRightAttacking", false);
        anim.SetBool("isLeftAttacking", false);
    }

    public void Dash(Vector3 dir, float distance, float time)
    {
        StartCoroutine(Dashing(dir, distance, time));
    }

    public IEnumerator Dashing(Vector3 dir, float distance, float time)
    {
        float timer = 0;
        Vector3 start = transform.position;
        while (timer < time)
        {
            rigid.MovePosition(Vector3.Lerp(start, start + dir * distance, timer/ time));
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    private void Avoid()
    {
        isAvoiding = true;
        weapon.ResetWeapon();
        StartCoroutine(Avoiding(lastDir, avoidSpeed * status.GetSpeed(), 0.5f));
    }

    private IEnumerator Avoiding(Vector3 dir, float sp, float time)
    {
        float timer = 0;
        gameObject.layer = 8;
        foreach (Transform child in transform)
            if (child.name == "collider")
                child.gameObject.layer = 8;
        layerBack = false;
        while (timer < time)
        {
            sp -= sp * 5 * Time.fixedDeltaTime;
            rigid.velocity = dir * sp;
            if (timer >= time * 0.5f && !layerBack)
            {
                gameObject.layer = 3;
                foreach (Transform child in transform)
                    if (child.name == "collider")
                        child.gameObject.layer = 3;
                layerBack = true;
            }
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        isAvoiding = false;
    }

    public void Hurt(float time)
    {
        if (isSkilling) return;
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
        weapon.ResetWeapon();
        speed = 0;
        canRotate = false;
        canChangeWeapon = false;
        isAttacking = false;
        while (hurtTimer < time)
        {
            hurtTimer += Time.deltaTime;
            yield return null;
        }
        isHurting = false;
        weapon.GetWeapon(currentWeapon);
        canRotate = true;
        canChangeWeapon = true;
        anim.SetBool("isRightAttacking", false);
        anim.SetBool("isLeftAttacking", false);
        anim.SetBool("isHurt", false);
    }

    private void GetWeapons()
    {
        int i = Random.Range(0, weapons.Count);
        weapon1 = weapons[i];
        weaponUI.SetWeapon1Icon(WeaponNo(weapon1));
        statusUI.SetWeapon1(WeaponNo(weapon1));
        weapons.RemoveAt(i);
        i = Random.Range(0, weapons.Count);
        weapon2 = weapons[i];
        weaponUI.SetWeapon2Icon(WeaponNo(weapon2));
        statusUI.SetWeapon2(WeaponNo(weapon2));
        weapons.RemoveAt(i);
        currentWeapon = weapon1;
        weapon.GetWeapon(currentWeapon);
    }

    private int WeaponNo(Weapon.Property property)
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
        status.QuickSwitch();
    }

    public void ChangeWeapon()
    {
        int i = Random.Range(0, weapons.Count);
        Weapon.Property temp = currentWeapon;
        if (currentWeapon == weapon1)
        {
            currentWeapon = weapon1 = weapons[i];
            weaponUI.SetWeapon1Icon(WeaponNo(weapon1));
            statusUI.SetWeapon1(WeaponNo(weapon1));
        }
        else
        {
            currentWeapon = weapon2 = weapons[i];
            weaponUI.SetWeapon2Icon(WeaponNo(weapon2));
            statusUI.SetWeapon2(WeaponNo(weapon2));
        }
        weapons.RemoveAt(i);
        weapons.Add(temp);
        weapon.GetWeapon(currentWeapon);
    }

    public void AssassinBreath()
    {
        status.AssassinBreath();
    }

    public void SetCannotInput()
    {
        canInput = false;
        weapon.ResetWeapon();
        speed = 0;
        anim.SetBool("isMovingRight", false);
        anim.SetBool("isMovingLeft", false);
        anim.SetBool("isRightAttacking", false);
        anim.SetBool("isLeftAttacking", false);
    }

    public void SetCanInput()
    {
        canInput = true;
        weapon.ResetWeapon();
    }
}
