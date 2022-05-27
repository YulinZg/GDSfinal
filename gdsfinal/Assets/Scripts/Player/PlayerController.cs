using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] private GameObject arrow;
    [SerializeField] private WeaponUI weaponUI;
    [SerializeField] private StatusUI statusUI;
    [SerializeField] private bool canTryWeapon = false;
    [SerializeField] private GameObject dodgeEffect;
    [SerializeField] private Sprite dieSprite;

    private Camera cam;
    private Animator anim;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
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
    private Coroutine avoidCoroutine;
    public GameObject col;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        weapon = GetComponent<Weapon>();
        status = GetComponent<Status>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetWeapons();
        if (PlayerPrefs.HasKey("DM"))
            DevelopMode();
    }

    // Update is called once per frame
    void Update()
    {
        if (canInput)
        {
            float moveX = 0;
            float moveY = 0;
            if (Input.GetKey(KeyCode.W))
            {
                moveY = 1f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                moveY = -1f;
                if (Input.GetKey(KeyCode.W))
                    moveY = 0;
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveX = 1f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                moveX = -1f;
                if (Input.GetKey(KeyCode.D))
                    moveX = 0;
            }
            moveDir = new Vector3(moveX, moveY).normalized;
            if (moveX != 0 || moveY != 0)
                lastDir = moveDir;
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetKeyDown(KeyCode.Space) && !isAvoiding && canAvoid && !isHurting)
            {
                Avoid();
            }
        }
    }

    private void LateUpdate()
    {
        if (canInput)
        {
            if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Q)) && canChangeWeapon && !isAttacking)
                SwitchWeapon();
            if (Input.GetKeyDown(KeyCode.R) && canChangeWeapon && !isAttacking && canTryWeapon)
                ChangeWeapon();
        }
    }

    private void FixedUpdate()
    {
        if (canInput)
        {
            mouseDir = ((Vector2)mousePos - (Vector2)transform.position).normalized;
            Move();
            RotateArrow();
        }
        else
            rigid.velocity = Vector2.zero;
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
        if (canRotate)
        {
            float angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void Die()
    {
        StopAllCoroutines();
        SetCannotInput();
        if (isFacingRight)
            anim.Play("die_right");
        else
            anim.Play("die_left");
        arrow.SetActive(false);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<LineRenderer>().enabled = false;
        rigid.bodyType = RigidbodyType2D.Static;
        weapon.enabled = false;
        status.enabled = false;
        enabled = false;
        Invoke(nameof(GameOver), 1f);
        audioSource.Stop();
    }

    private void GameOver()
    {
        UIManager.instance.ShowGameOver();
        BGMController.instance.Stop();
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
        audioSource.Play();
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
        audioSource.Stop();
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
        avoidCoroutine = StartCoroutine(Avoiding(lastDir, avoidSpeed * status.GetSpeed(), 0.5f));
    }

    private IEnumerator Avoiding(Vector3 dir, float sp, float time)
    {
        float timer = 0;
        gameObject.layer = 8;
        col.layer = 8;
        layerBack = false;
        spriteRenderer.color = new Color(1, 1, 1, 0.25f);
        while (timer < time)
        {
            sp -= sp * 5f * Time.fixedDeltaTime;
            rigid.velocity = dir * sp;
            if (sp < 3f && !layerBack)
            {
                gameObject.layer = 3;
                col.layer = 3;
                layerBack = true;
                spriteRenderer.color = Color.white;
            }
            else
            {
                GameObject effectInstance = Instantiate(dodgeEffect, transform.position, Quaternion.identity);
                effectInstance.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
            }
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        isAvoiding = false;
    }

    public void Hurt(float time)
    {
        if (isSkilling) return;
        if (isAvoiding)
        {
            StopCoroutine(avoidCoroutine);
            gameObject.layer = 3;
            col.layer = 3;
            isAvoiding = false;
        }
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
        canAvoid = true;
        audioSource.Stop();
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
        weapon.ResetWeapon();
    }

    public void AssassinBreath()
    {
        status.AssassinBreath();
    }

    public void SetCannotInput()
    {
        canInput = false;
        weapon.ResetWeapon();
        anim.SetBool("isMovingRight", false);
        anim.SetBool("isMovingLeft", false);
        anim.SetBool("isRightAttacking", false);
        anim.SetBool("isLeftAttacking", false);
    }

    public void SetCanInput()
    {
        canInput = true;
    }

    public void DevelopMode()
    {
        spriteRenderer.sprite = dieSprite;
        anim.enabled = false;
        canTryWeapon = true;
        status.DM = true;
        status.AddAttack(100);
        status.AddCritProbability(100);
        status.AddCritRate(100);
        status.AddMoveSpeed(100);
    }
}
