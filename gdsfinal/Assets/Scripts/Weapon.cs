using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Property
    {
        none,
        fire,
        water,
        earth,
        thunder,
        metal
    }

    public Property property;
    public float shootOffset;
    private PlayerController player;
    private float shootTimer = 10;
    private bool isReloading = false;

    [Header("Fire")]
    [SerializeField] private float damageF;
    [SerializeField] private float critProbabilityF;
    [SerializeField] private float critRateF;
    [SerializeField] private int maganizeF;
    [SerializeField] private float intervalF;
    [SerializeField] private float reloadTimeF;
    [SerializeField] private float bulletSpeedF;
    [SerializeField] private float rangeF;
    [SerializeField] private float moveSpeedF;
    [SerializeField] private int sideBulletNumber;
    [SerializeField] private GameObject[] bulletsF;
    private int currentMaganizeF;

    [Header("Water")]
    [SerializeField] private float damage0W;
    [SerializeField] private float damage1W;
    [SerializeField] private float critProbabilityW;
    [SerializeField] private float critRateW;
    [SerializeField] private int maganizeW;
    [SerializeField] private float intervalW;
    [SerializeField] private float reloadTimeW;
    [SerializeField] private float bulletSpeedW;
    [SerializeField] private float range0W;
    [SerializeField] private float range1W;
    [SerializeField] private float moveSpeed0W;
    [SerializeField] private float moveSpeed1W;
    [SerializeField] private float chargeTime;
    [SerializeField] private float attackTime1;
    [SerializeField] private GameObject[] bulletsW;
    private int currentMaganizeW;
    private float chargeTimer = 0;
    private bool isCharge = false;
    private bool isChargeAttacking = false;

    [Header("Earth")]
    [SerializeField] private float damageE;
    [SerializeField] private float critProbabilityE;
    [SerializeField] private float critRateE;
    [SerializeField] private int maganizeE;
    [SerializeField] private float intervalE;
    [SerializeField] private float reloadTimeE;
    [SerializeField] private float bulletSpeedE;
    [SerializeField] private float rangeE;
    [SerializeField] private float moveSpeedE;
    [SerializeField] private int preheatTimes;
    [SerializeField] private GameObject[] bulletsE;
    private int currentMaganizeE;
    private int preheatBullet;
    private bool isShooting = false;

    [Header("Thunder")]
    [SerializeField] private float damageT;
    [SerializeField] private float critProbabilityT;
    [SerializeField] private float critRateT;
    [SerializeField] private int maganizeT;
    [SerializeField] private float intervalT;
    [SerializeField] private float reloadTimeT;
    [SerializeField] private float bulletSpeedT;
    [SerializeField] private float rangeT;
    [SerializeField] private float moveSpeedT;
    [SerializeField] private float explosionRange;
    [SerializeField] private GameObject[] bulletsT;
    private int currentMaganizeT;

    [Header("Metal")]
    [SerializeField] private float damageM;
    [SerializeField] private float critProbabilityM;
    [SerializeField] private float critRateM;
    [SerializeField] private float intervalM;
    [SerializeField] private float rangeM;
    [SerializeField] private float moveSpeedM;
    [SerializeField] private float attackTime;
    [SerializeField] private GameObject[] bulletsM;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentMaganizeF = maganizeF;
        currentMaganizeW = maganizeW;
        currentMaganizeE = maganizeE;
        currentMaganizeT = maganizeT;
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        switch (property)
        {
            case Property.fire:
                Fire();
                break;
            case Property.water:
                Water();
                break;
            case Property.earth:
                Earth();
                break;
            case Property.thunder:
                Thunder();
                break;
            case Property.metal:
                Metal();
                break;
        }
    }

    public void GetWeapon(Property type)
    {
        property = type;
        switch (property)
        {
            case Property.fire:
                player.speed = moveSpeedF;
                break;
            case Property.water:
                player.speed = moveSpeed0W;
                chargeTimer = 0;
                isCharge = false;
                isChargeAttacking = false;
                break;
            case Property.earth:
                player.speed = moveSpeedE;
                preheatBullet = preheatTimes;
                isShooting = false;
                break;
            case Property.thunder:
                player.speed = moveSpeedT;
                break;
            case Property.metal:
                player.speed = moveSpeedM;
                break;
        }
    }

    private Vector3 ShootDir()
    {
        return ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized;
    }

    private Vector3 RotateVector(Vector3 vector, float degree)
    {
        return new Vector3(
            vector.x * Mathf.Cos(degree * Mathf.Deg2Rad) - vector.y * Mathf.Sin(degree * Mathf.Deg2Rad), 
            vector.x * Mathf.Sin(degree * Mathf.Deg2Rad) + vector.y * Mathf.Cos(degree * Mathf.Deg2Rad),
            0);
    }

    private Vector3 ShootPos(float distance)
    {
        return transform.position + ShootDir() * distance;
    }

    IEnumerator Reload(int amount, float reloadTime)
    {
        isReloading = true;
        player.canInput = false;
        switch (property)
        {
            case Property.fire:
                currentMaganizeF = 0;
                break;
            case Property.water:
                currentMaganizeW = 0;
                break;
            case Property.earth:
                currentMaganizeE = 0;
                break;
            case Property.thunder:
                currentMaganizeT = 0;
                break;
        }
        yield return new WaitForSeconds(reloadTime);
        switch (property)
        {
            case Property.fire:
                currentMaganizeF = amount;
                break;
            case Property.water:
                currentMaganizeW = amount;
                break;
            case Property.earth:
                currentMaganizeE = amount;
                break;
            case Property.thunder:
                currentMaganizeT = amount;
                break;
        }
        player.canInput = true;
        isReloading = false;
    }

    //Fire/////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Fire()
    {
        if (Input.GetMouseButtonDown(0) && currentMaganizeF != 0)
        {
            if (shootTimer >= intervalF)
            {
                currentMaganizeF--;
                NormalAttackF(sideBulletNumber);
                shootTimer = 0;
            }
        }
        if ((currentMaganizeF == 0 || Input.GetKeyDown(KeyCode.R)) && !isReloading)
            StartCoroutine(Reload(maganizeF, reloadTimeF));
    }

    void NormalAttackF(int bulletNum)
    {
        GameObject bulletInstance = Instantiate(bulletsF[0], ShootPos(shootOffset), Quaternion.identity);
        bulletInstance.GetComponent<Bullet>().SetNormal(ShootDir(), bulletSpeedF, damageF, critProbabilityF, critRateF, rangeF / bulletSpeedF);
        for (int i = 1; i < bulletNum + 1; i++)
        {
            bulletInstance = Instantiate(bulletsF[0], ShootPos(shootOffset), Quaternion.identity);
            bulletInstance.GetComponent<Bullet>().SetNormal(RotateVector(ShootDir(), 10 * i), bulletSpeedF, damageF, critProbabilityF, critRateF, rangeF / bulletSpeedF);
            bulletInstance = Instantiate(bulletsF[0], ShootPos(shootOffset), Quaternion.identity);
            bulletInstance.GetComponent<Bullet>().SetNormal(RotateVector(ShootDir(), -10 * i), bulletSpeedF, damageF, critProbabilityF, critRateF, rangeF / bulletSpeedF);
        }
    }

    //Fire/////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Water////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Water()
    {
        if (Input.GetMouseButtonDown(0) && !isChargeAttacking && currentMaganizeW != 0)
        {
            if (shootTimer >= intervalW)
            {
                isCharge = true;
                shootTimer = 0;
            }
        }
        if (((Input.GetMouseButtonUp(0) && isCharge) || chargeTimer > chargeTime + 1f) && !isChargeAttacking)
        {
            isCharge = false;
            player.speed = moveSpeed0W;
            NormalAttackW(chargeTimer);
            chargeTimer = 0;
        }
        if (isCharge)
        {
            chargeTimer += Time.deltaTime;
            if (chargeTimer > 0.2)
                player.speed = moveSpeed1W;
        }
        if ((currentMaganizeW == 0 || Input.GetKeyDown(KeyCode.R)) && !isReloading)
            StartCoroutine(Reload(maganizeW, reloadTimeW));
    }

    void NormalAttackW(float time)
    {
        GameObject bulletInstance;
        if (time < chargeTime)
        {
            currentMaganizeW--;
            bulletInstance = Instantiate(bulletsW[0], ShootPos(shootOffset), Quaternion.identity);
            bulletInstance.GetComponent<Bullet>().SetNormal(ShootDir(), bulletSpeedW, damage0W, critProbabilityW, critRateW, range0W / bulletSpeedW);
        }
        else
        {
            bulletInstance = Instantiate(bulletsW[1], ShootPos(range1W * 0.5f + shootOffset), Quaternion.identity);
            bulletInstance.GetComponent<Bullet>().SetStay(ShootDir(), damage1W, critProbabilityW, critRateW, attackTime1);
            bulletInstance.transform.localScale = new Vector3(range1W, bulletInstance.transform.localScale.y, 1);
            StartCoroutine(ChargeAttacking(attackTime1));
        }
    }

    IEnumerator ChargeAttacking(float time)
    {
        isChargeAttacking = true;
        player.canInput = false;
        player.speed = 0;
        yield return new WaitForSeconds(time);
        currentMaganizeW--;
        player.speed = moveSpeed0W;
        player.canInput = true;
        isChargeAttacking = false;
    }

    //Water////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Earth////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Earth()
    {
        if (Input.GetMouseButtonDown(0) && currentMaganizeE != 0)
        {
            isShooting = true;
        }
        if (Input.GetMouseButtonUp(0) || currentMaganizeE == 0)
        {
            isShooting = false;
            preheatBullet = preheatTimes;
        }
        if (isShooting)
        {
            if (shootTimer >= intervalE * (preheatBullet + 1))
            {
                if (preheatBullet != 0)
                    preheatBullet--;
                currentMaganizeE--;
                NormalAttackE();
                shootTimer = 0;
            }
        }
        if ((currentMaganizeE == 0 || Input.GetKeyDown(KeyCode.R)) && !isReloading)
            StartCoroutine(Reload(maganizeE, reloadTimeE));
    }

    void NormalAttackE()
    {
        GameObject bulletInstance = Instantiate(bulletsE[0], ShootPos(shootOffset), Quaternion.identity);
        bulletInstance.GetComponent<Bullet>().SetNormal(ShootDir(), bulletSpeedE, damageE, critProbabilityE, critRateE, rangeE / bulletSpeedE);
    }

    //Earth////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Thunder//////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Thunder()
    {
        if (Input.GetMouseButtonDown(0) && currentMaganizeT != 0)
        {
            if (shootTimer >= intervalT)
            {
                currentMaganizeT--;
                NormalAttackT();
                shootTimer = 0;
            }
        }
        if ((currentMaganizeT == 0 || Input.GetKeyDown(KeyCode.R)) && !isReloading)
            StartCoroutine(Reload(maganizeT, reloadTimeT));
    }

    void NormalAttackT()
    {
        GameObject bulletInstance = Instantiate(bulletsT[0], ShootPos(shootOffset), Quaternion.identity);
        bulletInstance.GetComponent<Bullet>().SetExplode(ShootDir(), bulletSpeedT, damageT, critProbabilityT, critRateT, rangeT / bulletSpeedT, bulletsT[1], explosionRange);
    }

    //Thunder//////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Metal////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Metal()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (shootTimer >= intervalM)
            {
                NormalAttackM();
                shootTimer = 0;
            }
        }
    }

    void NormalAttackM()
    {
        GameObject bulletInstance = Instantiate(bulletsM[0], ShootPos(shootOffset), Quaternion.identity);
        bulletInstance.GetComponent<Bullet>().SetStay(ShootDir(), damageM, critProbabilityM, critRateM, attackTime);
        bulletInstance.transform.localScale = new Vector3(rangeM, rangeM, 1);
        bulletInstance.transform.parent = transform;
    }

    //Metal////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
