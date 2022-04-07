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
        lightning,
        metal
    }

    public Property property;
    public float shootOffset;
    private PlayerController player;
    private float shootTimer = 10;
    private bool isAttacking = false;
    private bool isReloading = false;
    [SerializeField] private GameObject bulletRotater;
    [SerializeField] private GameObject bulletsInWorld;

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
    [SerializeField] private float standTimeF;
    [SerializeField] private int bulletNumber;
    [SerializeField] private float bulletRotateRange;
    [SerializeField] private float bulletLifeTimeBase;
    [SerializeField] private GameObject[] bulletsF;
    private int currentMaganizeF;
    private bool canLaunch = true;

    [Header("Water")]
    [SerializeField] private float damageW;
    [SerializeField] private float damageW1;
    [SerializeField] private float damageW2;
    [SerializeField] private float damageW3;
    [SerializeField] private float critProbabilityW;
    [SerializeField] private float critRateW;
    [SerializeField] private int maganizeW;
    [SerializeField] private float intervalW;
    [SerializeField] private float reloadTimeW;
    [SerializeField] private float bulletSpeedW;
    [SerializeField] private float rangeW;
    [SerializeField] private float rangeW1;
    [SerializeField] private float rangeW2;
    [SerializeField] private float rangeW3;
    [SerializeField] private float moveSpeedW;
    [SerializeField] private float moveSpeedW1;
    [SerializeField] private float chargeTime;
    [SerializeField] private float standTimeW;
    [SerializeField] private float standTimeW1;
    [SerializeField] private GameObject[] bulletsW;
    private int currentMaganizeW;
    private float chargeTimer = 0;
    private bool isCharging = false;
    private bool charged1 = false;
    private bool charged2 = false;


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
    [SerializeField] private float moveSpeedE1;
    [SerializeField] private int preheatTimes;
    [SerializeField] private float scatterAngle;
    [SerializeField] private GameObject[] bulletsE;
    private int currentMaganizeE;
    private int preheatBullet;
    private bool isShooting = false;
    private bool isAiming = false;

    [Header("Lightning")]
    [SerializeField] private float damageL;
    [SerializeField] private float damageL1;
    [SerializeField] private float critProbabilityL;
    [SerializeField] private float critRateL;
    [SerializeField] private int maganizeL;
    [SerializeField] private float intervalL;
    [SerializeField] private float reloadTimeL;
    [SerializeField] private float moveSpeedL;
    [SerializeField] private float standTimeL;
    [SerializeField] private int maxBulletNum;
    [SerializeField] private float stayTime;
    [SerializeField] private GameObject[] bulletsL;
    private int currentMaganizeL;

    [Header("Metal")]
    [SerializeField] private float damageM0;
    [SerializeField] private float damageM1;
    [SerializeField] private float damageM2;
    [SerializeField] private float damageM3;
    [SerializeField] private float damageM4;
    [SerializeField] private float damageM5;
    [SerializeField] private float critProbabilityM;
    [SerializeField] private float critRateM;
    [SerializeField] private float intervalM;
    [SerializeField] private float rangeM;
    [SerializeField] private float moveSpeedM;
    [SerializeField] private GameObject[] bulletsM;
    private bool isCombating = false;
    private int comboCount = 0;
    private float comboTimer = 0;
    private bool canCombo = false;
    private bool leftDown = false;
    private bool rightDown = false;


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
        currentMaganizeL = maganizeL;
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        if (!isAttacking && !isReloading && (Input.GetKeyDown(KeyCode.R)
            || currentMaganizeF == 0 || currentMaganizeW == 0 || currentMaganizeE == 0 || currentMaganizeL == 0))
            StartCoroutine(Reload());
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
            case Property.lightning:
                Lightning();
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
                player.SetSpeed(moveSpeedF);
                canLaunch = true;
                break;
            case Property.water:
                player.SetSpeed(moveSpeedW);
                chargeTimer = 0;
                isCharging = false;
                charged1 = false;
                charged2 = false;
                break;
            case Property.earth:
                player.SetSpeed(moveSpeedE);
                preheatBullet = preheatTimes;
                isShooting = false;
                isAiming = false;
                break;
            case Property.lightning:
                player.SetSpeed(moveSpeedL);
                break;
            case Property.metal:
                player.SetSpeed(moveSpeedM);
                isCombating = false;
                comboCount = 0;
                comboTimer = 0;
                canCombo = false;
                leftDown = false;
                rightDown = false;
                break;
        }
        if (bulletRotater.transform.childCount > 0)
            foreach (Transform child in bulletRotater.transform)
                Destroy(child.gameObject);
        if (bulletsInWorld.transform.childCount > 0)
            foreach (Transform child in bulletsInWorld.transform)
                Destroy(child.gameObject);
    }

    private Vector3 MouseDir()
    {
        return ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized;
    }

    private Vector3 Direction(Vector2 endPos, Vector2 startPos)
    {
        return (endPos - startPos).normalized;
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
        return transform.position + MouseDir() * distance;
    }

    IEnumerator StandAttack(float time, float speed, bool canRotate)
    {
        yield return null;
        yield return null;
        isAttacking = true;
        player.canInput = false;
        player.canRotate = canRotate;
        player.isAttacking = true;
        player.SetSpeed(0);
        yield return new WaitForSeconds(time - Time.deltaTime * 3);
        player.SetSpeed(speed);
        player.isAttacking = false;
        player.canRotate = true;
        player.canInput = true;
        isAttacking = false;
    }

    IEnumerator Reload()
    {
        float reloadTime = 0;
        player.canInput = false;
        isReloading = true;
        switch (property)
        {
            case Property.fire:
                currentMaganizeF = 0;
                reloadTime = reloadTimeF;
                break;
            case Property.water:
                currentMaganizeW = 0;
                reloadTime = reloadTimeW;
                break;
            case Property.earth:
                currentMaganizeE = 0;
                reloadTime = reloadTimeE;
                break;
            case Property.lightning:
                currentMaganizeL = 0;
                reloadTime = reloadTimeL;
                break;
        }
        yield return new WaitForSeconds(reloadTime);
        switch (property)
        {
            case Property.fire:
                currentMaganizeF = maganizeF;
                break;
            case Property.water:
                currentMaganizeW = maganizeW;
                break;
            case Property.earth:
                currentMaganizeE = maganizeE;
                break;
            case Property.lightning:
                currentMaganizeL = maganizeL;
                break;
        }
        isReloading = false;
        player.canInput = true;
    }

    public bool GetIsReloading()
    {
        return isReloading;
    }

    void RotateBullet(float speed)
    {
        bulletRotater.transform.Rotate(0, 0, speed * Time.deltaTime);
    }

    void SetPlayerAttackingFalse()
    {
        player.isAttacking = false;
    }

    //Fire/////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Fire()
    {
        if (Input.GetMouseButtonDown(0) && currentMaganizeF != 0 && !isAttacking)
        {
            if (shootTimer >= intervalF)
            {
                NormalAttackF(bulletNumber);
                shootTimer = 0;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            NormalAttackF1();
        }
        RotateBullet(bulletSpeedF * 5);
    }

    void NormalAttackF(int bulletNum)
    {
        currentMaganizeF--;
        if (bulletRotater.transform.childCount > 0)
            foreach (Transform child in bulletRotater.transform)
                Destroy(child.gameObject);
        GameObject bulletInstance;
        canLaunch = false;
        for (int i = 0; i < bulletNumber; i++)
        {
            bulletInstance = Instantiate(bulletsF[0], transform.position, Quaternion.identity);
            bulletInstance.transform.parent = bulletRotater.transform;
            bulletInstance.GetComponent<Bullet>().Setup(
                RotateVector(MouseDir(), (360f / bulletNum) * (i + 1)), bulletSpeedF * 0.5f, damageF, critProbabilityF, critRateF, bulletLifeTimeBase * (i + 1), Bullet.BulletType.normal);
            bulletInstance.GetComponent<Bullet>().SetFire(bulletRotateRange);
        }
        float invokeTime = bulletRotateRange / (bulletSpeedF * 0.5f);
        Invoke(nameof(SetCanLaunch), invokeTime);
        player.isAttacking = true;
        Invoke(nameof(SetPlayerAttackingFalse), invokeTime);
    }

    void SetCanLaunch()
    {
        canLaunch = true;
    }

    void NormalAttackF1()
    {
        if (bulletRotater.transform.childCount > 0 && canLaunch)
        {
            GameObject bulletInstance;
            foreach (Transform child in bulletRotater.transform)
            {
                bulletInstance = Instantiate(bulletsF[1], child.position, Quaternion.identity);
                bulletInstance.GetComponent<Bullet>().Setup(
                    Direction(Camera.main.ScreenToWorldPoint(Input.mousePosition), child.position), bulletSpeedF, damageF, critProbabilityF, critRateF, rangeF / bulletSpeedF, Bullet.BulletType.normal);
                Destroy(child.gameObject);
            }
            StartCoroutine(StandAttack(standTimeF, moveSpeedF, false));
        }
    }

    //Fire/////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Water////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Water()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentMaganizeW != 0 && !isAttacking && shootTimer >= intervalW && !isCharging)
            {
                isCharging = true;
                shootTimer = 0;
            }
            if (chargeTimer >= chargeTime * 2 && chargeTimer <= chargeTime * 2 + 0.5f && isCharging)
            {
                charged2 = true;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (chargeTimer >= chargeTime && chargeTimer <= chargeTime + 0.5f && isCharging)
            {
                charged1 = true;
            }
        }
        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            if (!charged1)
            {
                if (chargeTimer < chargeTime)
                {
                    NormalAttackW();
                }
                else
                {
                    NormalAttackW1(rangeW1, rangeW1 * 0.25f, damageW1);
                }
            }
            if (charged2)
            {
                if (chargeTimer < chargeTime * 3)
                {
                    NormalAttackW1(rangeW2, rangeW2 * 0.25f, damageW2);
                }
            }
        }
        if (Input.GetMouseButtonUp(1) && isCharging)
        {
            if (charged1 && !charged2)
            {
                if (chargeTimer < chargeTime * 2)
                {
                    NormalAttackW1(rangeW1, rangeW1 * 0.25f, damageW1);
                }
                else
                {
                    NormalAttackW1(rangeW2, rangeW2 * 0.25f, damageW2);
                }
            }
        }

        if (chargeTimer > chargeTime + 0.5 && !charged1 && isCharging)
        {
            NormalAttackW1(rangeW1, rangeW1 * 0.25f, damageW1);
        }
        else if (chargeTimer > chargeTime * 2 + 0.5 && charged1 && !charged2 && isCharging)
        {
            NormalAttackW1(rangeW2, rangeW2 * 0.25f, damageW2);
        }
        else if (chargeTimer >= chargeTime * 3 && charged1 && charged2 && isCharging)
        {
            NormalAttackW1(rangeW3, rangeW3 * 0.25f, damageW3);
        }
        
        if (isCharging)
        {
            chargeTimer += Time.deltaTime;
            if (chargeTimer > 0.2)
                player.SetSpeed(moveSpeedW1);
        }
    }

    void NormalAttackW()
    {
        isCharging = false;
        charged1 = false;
        charged2 = false;
        chargeTimer = 0;
        currentMaganizeW--;
        chargeTimer = 0;
        GameObject bulletInstance;
        bulletInstance = Instantiate(bulletsW[0], ShootPos(shootOffset), Quaternion.identity);
        bulletInstance.GetComponent<Bullet>().Setup(
            MouseDir(), bulletSpeedW, damageW, critProbabilityW, critRateW, rangeW / bulletSpeedW, Bullet.BulletType.normal);
        StartCoroutine(StandAttack(standTimeW, moveSpeedW, false));
    }

    void NormalAttackW1(float rangeY, float rangeX, float damage)
    {
        isCharging = false;
        charged1 = false;
        charged2 = false;
        chargeTimer = 0;
        currentMaganizeW--;
        chargeTimer = 0;
        GameObject bulletInstance;
        bulletInstance = Instantiate(bulletsW[1], ShootPos(rangeY * 0.5f + shootOffset), Quaternion.identity);
        bulletInstance.GetComponent<Bullet>().Setup(
            MouseDir(), 0, damage, critProbabilityW, critRateW, standTimeW1, Bullet.BulletType.penetrable);
        bulletInstance.transform.localScale = new Vector3(rangeY, rangeX, 1);
        StartCoroutine(StandAttack(standTimeW1, moveSpeedW, false));
    }

    //Water////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Earth////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Earth()
    {
        if (Input.GetMouseButtonDown(1) && currentMaganizeE != 0)
        {
            isAiming = true;
            player.SetSpeed(moveSpeedE1);
        }
        if (Input.GetMouseButtonUp(1) || currentMaganizeE == 0)
        {
            isAiming = false;
            player.SetSpeed(moveSpeedE);
            if (!isShooting)
                preheatBullet = preheatTimes;
        }
        if (Input.GetMouseButtonDown(0) && currentMaganizeE != 0)
        {
            isShooting = true;
            player.isAttacking = true;
        }
        if (Input.GetMouseButtonUp(0) || currentMaganizeE == 0)
        {
            isShooting = false;
            player.isAttacking = false;
            if (!isAiming || currentMaganizeE == 0)
                preheatBullet = preheatTimes;
        }
        if (isShooting || isAiming)
        {
            if (shootTimer >= intervalE * (preheatBullet + 1))
            {
                if (preheatBullet != 0)
                    preheatBullet--;
                if (isShooting)
                {
                    if (isAiming)
                    {
                        NormalAttackE(MouseDir());
                    }
                    else
                    {
                        NormalAttackE(RotateVector(MouseDir(), Random.Range(-scatterAngle, scatterAngle)));
                    }
                }
                shootTimer = 0;
            }
        }
    }

    void NormalAttackE(Vector3 dir)
    {
        currentMaganizeE--;
        GameObject bulletInstance = Instantiate(bulletsE[0], ShootPos(shootOffset), Quaternion.identity);
        bulletInstance.GetComponent<Bullet>().Setup(
            dir, bulletSpeedE, damageE, critProbabilityE, critRateE, rangeE / bulletSpeedE, Bullet.BulletType.normal);
    }

    //Earth////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Lightning////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Lightning()
    {
        if (Input.GetMouseButtonDown(0) && currentMaganizeL != 0 && !isAttacking)
        {
            if (shootTimer >= intervalL)
            {
                NormalAttackL();
                shootTimer = 0;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            NormalAttackL1();
        }
    }

    void NormalAttackL()
    {
        if (bulletsInWorld.transform.childCount < maxBulletNum)
        {
            currentMaganizeL--;
            GameObject bulletInstance = Instantiate(bulletsL[0], ShootPos(shootOffset), Quaternion.identity);
            bulletInstance.GetComponent<Bullet>().Setup(
                MouseDir(), 0, damageL, critProbabilityL, critRateL, stayTime, Bullet.BulletType.penetrable);
            bulletInstance.transform.parent = bulletsInWorld.transform;
            player.isAttacking = true;
            Invoke(nameof(SetPlayerAttackingFalse), 0.2f);
        }
    }

    void NormalAttackL1()
    {
        if (bulletsInWorld.transform.childCount > 0)
        {
            foreach(Transform child in bulletsInWorld.transform)
            {
                GameObject bulletInstance = Instantiate(bulletsL[0], child.position, Quaternion.identity);
                bulletInstance.GetComponent<Bullet>().Setup(
                    Direction(transform.position, child.position), 0, damageL, critProbabilityL, critRateL, standTimeL, Bullet.BulletType.penetrable);
                bulletInstance = Instantiate(bulletsL[1], 0.5f * Vector3.Distance(child.position, transform.position) * Direction(transform.position, child.position) + child.position, Quaternion.identity);
                bulletInstance.GetComponent<Bullet>().Setup(
                    Direction(transform.position, child.position), 0, damageL1, critProbabilityL, critRateL, standTimeL, Bullet.BulletType.penetrable);
                bulletInstance.transform.localScale = new Vector3(Vector3.Distance(child.position, transform.position), 2, 1);
                Destroy(child.gameObject);
            }
            StartCoroutine(StandAttack(standTimeL, moveSpeedL, false));
        }
    }

    //Lightning////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Metal////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Metal()
    {
        if (canCombo)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer > intervalM * 2)
            {
                canCombo = false;
                comboTimer = 0;
                leftDown = false;
                rightDown = false;
                comboCount = 0;
            }
            if (Input.GetMouseButtonDown(0))
            {
                leftDown = true;
                rightDown = false;
                canCombo = false;
                comboTimer = 0;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                leftDown = false;
                rightDown = true;
                canCombo = false;
                comboTimer = 0;
            }
        }
        else if (!leftDown && !rightDown)
        {
            if (Input.GetMouseButtonDown(0))
            {
                NormalAttackM00();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                NormalAttackM10();
            }
        }
        if (!isCombating && (leftDown || rightDown))
            Combo();
    }

    void NormalAttackM00()
    {
        if (!isCombating)
        {
            StartCoroutine(NormalAttackM0(0.35f, 0, damageM0));
            StartCoroutine(SetCombo(0.35f));
            comboCount++;
        }
    }

    void NormalAttackM01()
    {
        if (!isCombating)
        {
            StartCoroutine(NormalAttackM0(0.4f, 1, damageM1));
            StartCoroutine(SetCombo(0.4f));
            comboCount++;
        }
    }

    void NormalAttackM02()
    {
        if (!isCombating)
        {
            StartCoroutine(NormalAttackM0(1f, 2, damageM2));
            StartCoroutine(SetCombo(1f));
            comboCount = 0;
        }
    }

    void NormalAttackM10()
    {
        if (!isCombating)
        {
            StartCoroutine(NormalAttackM1());
            StartCoroutine(SetCombo(0.5f));
            comboCount++;
        }
    }

    void NormalAttackM11()
    {
        if (!isCombating)
        {
            StartCoroutine(NormalAttackM2());
            StartCoroutine(SetCombo(0.5f));
            comboCount++;
        }
    }

    void NormalAttackM12()
    {
        if (!isCombating)
        {
            StartCoroutine(NormalAttackM3());
            StartCoroutine(SetCombo(1f));
            comboCount = 0;
        }
    }

    void Combo()
    {
        player.SetSpeed(0);
        switch (comboCount)
        {
            case 0:
                if (leftDown)
                    NormalAttackM00();
                else if(rightDown)
                    NormalAttackM10();
                break;
            case 1:
                if (leftDown)
                    NormalAttackM01();
                else if (rightDown)
                    NormalAttackM11();
                break;
            case 2:
                if (leftDown)
                    NormalAttackM02();
                else if (rightDown)
                    NormalAttackM12();
                break;
        }
        leftDown = false;
        rightDown = false;
    }

    IEnumerator SetCombo(float time)
    {
        leftDown = false;
        rightDown = false;
        canCombo = false;
        yield return new WaitForSeconds(time - intervalM);
        canCombo = true;
    }

    IEnumerator NormalAttackM0(float time, int bulletNo, float damage)
    {
        StartCoroutine(StandAttack(time, moveSpeedM, true));
        isCombating = true;
        GameObject bulletInstance = Instantiate(bulletsM[bulletNo], transform.position, Quaternion.identity);
        bulletInstance.GetComponent<Bullet>().Setup(
            MouseDir(), 0, damage, critProbabilityM, critRateM, time, Bullet.BulletType.penetrable);
        bulletInstance.transform.localScale = new Vector3(rangeM, rangeM, 1);
        bulletInstance.transform.parent = transform;
        yield return new WaitForSeconds(time);
        isCombating = false;
    }

    IEnumerator NormalAttackM1()
    {
        StartCoroutine(StandAttack(0.25f, 0, true));
        isCombating = true;
        Vector3 dir = MouseDir();
        GameObject bulletInstance = Instantiate(bulletsM[3], ShootPos(shootOffset), Quaternion.identity);
        bulletInstance.GetComponent<Bullet>().Setup(
            dir, 0, damageM3, critProbabilityM, critRateM, 0.5f, Bullet.BulletType.penetrable);
        bulletInstance.transform.localScale = new Vector3(rangeM, rangeM, 1);
        yield return new WaitForSeconds(0.25f);
        player.Dash(dir, 2.0f * rangeM, 0.25f, MoveSpeedM(), true, true);
        yield return new WaitForSeconds(0.25f);
        isCombating = false;
    }

    IEnumerator NormalAttackM2()
    {
        isCombating = true;
        Vector3 dir = MouseDir();
        GameObject bulletInstance = Instantiate(bulletsM[4], ShootPos(shootOffset), Quaternion.identity);
        bulletInstance.GetComponent<Bullet>().Setup(
            dir, 0, damageM4, critProbabilityM, critRateM, 0.5f, Bullet.BulletType.penetrable);
        bulletInstance.transform.localScale = new Vector3(rangeM, rangeM, 1);
        player.Dash(dir, 1.0f * rangeM, 0.17f, 0, false, true);
        yield return new WaitForSeconds(0.17f);
        StartCoroutine(StandAttack(0.16f, 0, true));
        yield return new WaitForSeconds(0.16f);
        player.Dash(dir, 1.0f * rangeM, 0.17f, MoveSpeedM(), true, true);
        yield return new WaitForSeconds(0.17f);
        isCombating = false;
    }

    IEnumerator NormalAttackM3()
    {
        StartCoroutine(StandAttack(0.62f, 0, true));
        isCombating = true;
        Vector3 dir = MouseDir();
        GameObject bulletInstance = Instantiate(bulletsM[5], ShootPos(shootOffset), Quaternion.identity);
        bulletInstance.GetComponent<Bullet>().Setup(
            dir, 0, damageM5, critProbabilityM, critRateM, 1f, Bullet.BulletType.penetrable);
        bulletInstance.transform.localScale = new Vector3(rangeM, rangeM, 1);
        yield return new WaitForSeconds(0.62f);
        player.Dash(dir, 3.5f * rangeM, 0.23f, 0, false, true);
        yield return new WaitForSeconds(0.23f);
        StartCoroutine(StandAttack(0.15f, moveSpeedM, true));
        yield return new WaitForSeconds(0.15f);
        isCombating = false;
    }

    float MoveSpeedM()
    {
        if (leftDown || rightDown)
            return 0;
        else
            return moveSpeedM;
    }

    //Metal////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
