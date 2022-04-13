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
    [SerializeField] private float shootOffset;
    [SerializeField] private GameObject bulletRotater;
    [SerializeField] private Transform bulletsInWorld;
    [SerializeField] private Transform lightningBalls;
    [SerializeField] private GameObject[] effects;
    private GameObject effectInstance;
    private GameObject effectInstance1;
    private PlayerController player;
    private Status status;
    private float shootTimer = 10;

    [Header("Fire")]
    [SerializeField] private float damageF;
    [SerializeField] private float intervalF;
    [SerializeField] private float bulletSpeedF;
    [SerializeField] private float rangeF;
    [SerializeField] private float moveSpeedF;
    [SerializeField] private float standTimeF;
    [SerializeField] private int bulletNumber;
    [SerializeField] private float bulletRotateRange;
    [SerializeField] private float bulletLifeTimeBase;
    [SerializeField] private GameObject[] bulletsF;
    private bool canLaunch = true;

    [SerializeField] private float burnDamage;
    [SerializeField] private float burnTime;
    [SerializeField] private float burnInterval;

    [Header("Water")]
    [SerializeField] private float damageW;
    [SerializeField] private float damageW1;
    [SerializeField] private float damageW2;
    [SerializeField] private float damageW3;
    [SerializeField] private float intervalW;
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
    private float chargeTimer = 0;
    private bool isCharging = false;
    private bool charged1 = false;
    private bool charged2 = false;
    private bool isSpraying = false;
    private bool spawnedEffect1 = false;
    private bool spawnedEffect2 = false;
    private bool spawned2Effect2 = false;

    [SerializeField] private float decelerateRate;
    [SerializeField] private float decelerateTime;


    [Header("Earth")]
    [SerializeField] private float damageE;
    [SerializeField] private float intervalE;
    [SerializeField] private float bulletSpeedE;
    [SerializeField] private float rangeE;
    [SerializeField] private float moveSpeedE;
    [SerializeField] private float moveSpeedE1;
    [SerializeField] private int preheatTimes;
    [SerializeField] private float scatterAngle;
    [SerializeField] private GameObject[] bulletsE;
    private int preheatBullet;
    private bool isShooting = false;
    private bool isAiming = false;

    [SerializeField] private float stunValue;
    [SerializeField] private float stunTime;

    [Header("Lightning")]
    [SerializeField] private float damageL;
    [SerializeField] private float damageL1;
    [SerializeField] private float intervalL;
    [SerializeField] private float moveSpeedL;
    [SerializeField] private float standTimeL;
    [SerializeField] private int maxBulletNum;
    [SerializeField] private float stayTime;
    [SerializeField] private GameObject[] bulletsL;
    private bool isLinking = false;

    [SerializeField] private float palsyDamage;
    [SerializeField] private float palsyTime;
    [SerializeField] private float palsyInterval;

    [Header("Metal")]
    [SerializeField] private float damageM0;
    [SerializeField] private float damageM1;
    [SerializeField] private float damageM2;
    [SerializeField] private float damageM3;
    [SerializeField] private float damageM4;
    [SerializeField] private float damageM5;
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
    private int lastClick = 0;

    [SerializeField] private float repelDistance0;
    [SerializeField] private float repelDistance1;
    [SerializeField] private float repelDistance2;
    [SerializeField] private float repelDistance3;
    [SerializeField] private float repelDistance4;
    [SerializeField] private float repelDistance5;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        status = GetComponent<Status>();
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        if (!player.isHurting)
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
        RotateBullet(bulletSpeedF * 10);
    }

    public void GetWeapon(Property type)
    {
        shootTimer = 0;
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
                isSpraying = false;
                break;
            case Property.earth:
                player.SetSpeed(moveSpeedE);
                preheatBullet = preheatTimes;
                isShooting = false;
                isAiming = false;
                break;
            case Property.lightning:
                player.SetSpeed(moveSpeedL);
                isLinking = false;
                break;
            case Property.metal:
                player.SetSpeed(moveSpeedM);
                isCombating = false;
                comboCount = 0;
                comboTimer = 0;
                canCombo = false;
                leftDown = false;
                rightDown = false;
                lastClick = 0;
                break;
        }
        if (bulletsInWorld.transform.childCount > 0)
            foreach (Transform child in bulletsInWorld.transform)
                Destroy(child.gameObject);
        if (effectInstance)
            Destroy(effectInstance);
        if (effectInstance1)
            Destroy(effectInstance1);
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

    private void RotateBullet(float speed)
    {
        bulletRotater.transform.Rotate(0, 0, speed * Time.deltaTime);
    }

    private void SpawnEffect(int effectNo)
    {
        effectInstance1 = Instantiate(effects[effectNo], transform);
    }

    private void SpawnEffectInstance(int effectNo)
    {
        effectInstance = Instantiate(effects[effectNo], transform);
    }

    //Fire/////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Fire()
    {
        if (Input.GetMouseButtonDown(0))
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
    }

    private void NormalAttackF(int bulletNum)
    {
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
                RotateVector(MouseDir(), (360f / bulletNum) * (i + 1)), bulletRotateRange / standTimeF, damageF * status.GetAttack(), status.GetCritProbability(), status.GetCritRate(), standTimeF + bulletLifeTimeBase * (i + 1), Bullet.BulletType.normal);
            bulletInstance.GetComponent<Bullet>().SetFire(bulletRotateRange);
            SetBurn(bulletInstance.GetComponent<Bullet>());
        }
        Invoke(nameof(SetCanLaunch), standTimeF);
        player.Attack(MouseDir(), standTimeF, true, moveSpeedF, true);
        SpawnEffect(0);
    }

    private void SetCanLaunch()
    {
        canLaunch = true;
    }

    private void NormalAttackF1()
    {
        if (bulletRotater.transform.childCount > 0 && canLaunch)
        {
            GameObject bulletInstance;
            foreach (Transform child in bulletRotater.transform)
            {
                bulletInstance = Instantiate(bulletsF[1], child.position, Quaternion.identity);
                bulletInstance.GetComponent<Bullet>().Setup(
                    Direction(Camera.main.ScreenToWorldPoint(Input.mousePosition), child.position), bulletSpeedF, damageF * status.GetAttack(), status.GetCritProbability(), status.GetCritRate(), rangeF / bulletSpeedF, Bullet.BulletType.normal);
                SetBurn(bulletInstance.GetComponent<Bullet>());
                Destroy(child.gameObject);
            }
            player.Attack(MouseDir(), 0.2f, false, moveSpeedF, false);
        }
    }

    private void SetBurn(Bullet bullet)
    {
        bullet.SetBurn(burnDamage, burnTime, burnInterval);
    }

    //Fire/////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Water////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Water()
    {
        if (Input.GetMouseButtonDown(0) && !isSpraying)
        {
            if (shootTimer >= intervalW && !isCharging)
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
            SpawnEffect(2);
        }

        if (chargeTimer >= chargeTime && !spawnedEffect2)
        {
            SpawnEffect(2);
            spawnedEffect2 = true;
        }
        if (chargeTimer >= chargeTime * 2 && !spawned2Effect2)
        {
            SpawnEffect(2);
            spawned2Effect2 = true;
        }

        if (isCharging)
        {
            chargeTimer += Time.deltaTime;
            if (chargeTimer > 0.2 && !spawnedEffect1)
            {
                player.SetSpeed(moveSpeedW1);
                SpawnEffectInstance(1);
                spawnedEffect1 = true;
            }
        }
    }

    private void NormalAttackW()
    {
        isCharging = false;
        charged1 = false;
        charged2 = false;
        chargeTimer = 0;
        chargeTimer = 0;
        GameObject bulletInstance;
        bulletInstance = Instantiate(bulletsW[0], ShootPos(shootOffset), Quaternion.identity);
        bulletInstance.GetComponent<Bullet>().Setup(
            MouseDir(), bulletSpeedW, damageW * status.GetAttack(), status.GetCritProbability(), status.GetCritRate(), rangeW / bulletSpeedW, Bullet.BulletType.normal);
        SetDecelerate(bulletInstance.GetComponent<Bullet>());
        player.Attack(MouseDir(), standTimeW, true, moveSpeedW, false);
        spawnedEffect1 = false;
        spawnedEffect2 = false;
        spawned2Effect2 = false;
        if (effectInstance)
            Destroy(effectInstance);
    }

    private void NormalAttackW1(float rangeY, float rangeX, float damage)
    {
        isSpraying = true;
        isCharging = false;
        charged1 = false;
        charged2 = false;
        chargeTimer = 0;
        chargeTimer = 0;
        GameObject bulletInstance;
        bulletInstance = Instantiate(bulletsW[1], ShootPos(rangeY * 0.5f + shootOffset), Quaternion.identity);
        bulletInstance.GetComponent<Bullet>().Setup(
            MouseDir(), 0, damage * status.GetAttack(), status.GetCritProbability(), status.GetCritRate(), standTimeW1, Bullet.BulletType.penetrable);
        SetDecelerate(bulletInstance.GetComponent<Bullet>());
        bulletInstance.transform.localScale = new Vector3(rangeY, rangeX, 1);
        bulletInstance.transform.parent = bulletsInWorld;
        player.Attack(MouseDir(), standTimeW1, true, moveSpeedW, false);
        spawnedEffect1 = false;
        spawnedEffect2 = false;
        spawned2Effect2 = false;
        Invoke(nameof(SetNotSpraying), standTimeW1);
        if (effectInstance)
            Destroy(effectInstance);
    }

    private void SetNotSpraying()
    {
        isSpraying = false;
    }

    private void SetDecelerate(Bullet bullet)
    {
        bullet.SetDecelerate(decelerateRate, decelerateTime);
    }

    //Water////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Earth////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Earth()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isAiming = true;
            player.SetSpeed(moveSpeedE1);
            SpawnEffectInstance(3);
        }
        if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            player.SetSpeed(moveSpeedE);
            if (!isShooting)
                preheatBullet = preheatTimes;
            if (effectInstance)
                Destroy(effectInstance);
        }
        if (Input.GetMouseButton(0))
        {
            isShooting = true;
            player.Attack(MouseDir(), Time.deltaTime * 5, false, moveSpeedE, true);
        }
        else
        {
            isShooting = false;
            if (!isAiming)
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
                        NormalAttackE(RotateVector(MouseDir(), Random.Range(-scatterAngle * 0.1f, scatterAngle * 0.1f)));
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

    private void NormalAttackE(Vector3 dir)
    {
        GameObject bulletInstance = Instantiate(bulletsE[0], ShootPos(shootOffset), Quaternion.identity);
        bulletInstance.GetComponent<Bullet>().Setup(
            dir, bulletSpeedE, damageE * status.GetAttack(), status.GetCritProbability(), status.GetCritRate(), rangeE / bulletSpeedE, Bullet.BulletType.normal);
        SetStun(bulletInstance.GetComponent<Bullet>());
    }

    private void SetStun(Bullet bullet)
    {
        bullet.SetStun(stunValue, stunTime);
    }

    //Earth////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Lightning////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Lightning()
    {
        if (Input.GetMouseButtonDown(0) && !isLinking)
        {
            if (shootTimer >= intervalL && lightningBalls.childCount < maxBulletNum)
            {
                NormalAttackL();
                shootTimer = 0;
            }
        }
        if (Input.GetMouseButtonDown(1) && !isLinking)
        {
            if (shootTimer >= intervalL && lightningBalls.childCount > 0)
            {
                NormalAttackL1();
                shootTimer = 0;
            }
        }
    }

    private void NormalAttackL()
    {
        GameObject bulletInstance = Instantiate(bulletsL[0], ShootPos(shootOffset), Quaternion.identity);
        bulletInstance.GetComponent<Bullet>().Setup(
            MouseDir(), 0, damageL * status.GetAttack(), status.GetCritProbability(), status.GetCritRate(), stayTime, Bullet.BulletType.penetrable);
        SetPalsy(bulletInstance.GetComponent<Bullet>());
        bulletInstance.transform.parent = lightningBalls;
        player.Attack(MouseDir(), 0.2f, false, moveSpeedL, false);
    }

    private void NormalAttackL1()
    {
        isLinking = true;
        foreach (Transform child in lightningBalls)
        {
                GameObject bulletInstance = Instantiate(bulletsL[0], child.position, Quaternion.identity);
                bulletInstance.GetComponent<Bullet>().Setup(
                    Direction(transform.position, child.position), 0, damageL * status.GetAttack(), status.GetCritProbability(), status.GetCritRate(), standTimeL, Bullet.BulletType.penetrable);
                SetPalsy(bulletInstance.GetComponent<Bullet>());
                bulletInstance = Instantiate(bulletsL[1], 0.5f * Vector3.Distance(child.position, transform.position) * Direction(transform.position, child.position) + child.position, Quaternion.identity);
                bulletInstance.GetComponent<Bullet>().Setup(
                    Direction(transform.position, child.position), 0, damageL1 * status.GetAttack(), status.GetCritProbability(), status.GetCritRate(), standTimeL, Bullet.BulletType.penetrable);
                SetPalsy(bulletInstance.GetComponent<Bullet>());
                bulletInstance.transform.localScale = new Vector3(Vector3.Distance(child.position, transform.position), 2, 1);
                bulletInstance.transform.parent = bulletsInWorld;
                Destroy(child.gameObject);
        }
        player.Attack(MouseDir(), standTimeL, true, moveSpeedL, true);
        Invoke(nameof(SetNotLinking), standTimeL);
        SpawnEffect(4);
    }

    private void SetNotLinking()
    {
        isLinking = false;
    }

    private void SetPalsy(Bullet bullet)
    {
        bullet.SetPalsy(palsyDamage, palsyTime, palsyInterval);
    }

    //Lightning////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Metal////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Metal()
    {
        if (canCombo)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer > intervalM * 2)
            {
                canCombo = false;
                comboTimer = 0;
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

    private void NormalAttackM00()
    {
        if (!isCombating)
        {
            StartCoroutine(NormalAttackM0(0.35f, 0, damageM0, repelDistance0));
            StartCoroutine(SetCombo(0.35f));
            comboCount++;
        }
    }

    private void NormalAttackM01()
    {
        if (!isCombating)
        {
            StartCoroutine(NormalAttackM0(0.4f, 1, damageM1, repelDistance1));
            StartCoroutine(SetCombo(0.4f));
            comboCount++;
        }
    }

    private void NormalAttackM02()
    {
        if (!isCombating)
        {
            StartCoroutine(NormalAttackM0(1f, 2, damageM2, repelDistance2));
            StartCoroutine(SetCombo(1f));
            comboCount = 0;
        }
    }

    private void NormalAttackM10()
    {
        if (!isCombating)
        {
            StartCoroutine(NormalAttackM1());
            StartCoroutine(SetCombo(0.5f));
            comboCount++;
        }
    }

    private void NormalAttackM11()
    {
        if (!isCombating)
        {
            StartCoroutine(NormalAttackM2());
            StartCoroutine(SetCombo(0.5f));
            comboCount++;
        }
    }

    private void NormalAttackM12()
    {
        if (!isCombating)
        {
            StartCoroutine(NormalAttackM3());
            StartCoroutine(SetCombo(1f));
            comboCount = 0;
        }
    }

    private void Combo()
    {
        player.SetSpeed(0);
        switch (comboCount)
        {
            case 0:
                if (leftDown)
                    NormalAttackM00();
                else if (rightDown)
                    NormalAttackM10();
                break;
            case 1:
                if (leftDown)
                {
                    lastClick = 1;
                    NormalAttackM01();
                }
                else if (rightDown)
                {
                    lastClick = 2;
                    NormalAttackM11();
                }
                break;
            case 2:
                if (leftDown && lastClick == 2)
                    NormalAttackM02();
                else if (rightDown && lastClick == 1)
                    NormalAttackM12();
                lastClick = 0;
                comboCount = 0;
                player.SetSpeed(moveSpeedM);
                break;
        }
        leftDown = false;
        rightDown = false;
    }

    IEnumerator SetCombo(float attackTime)
    {
        leftDown = false;
        rightDown = false;
        canCombo = false;
        yield return new WaitForSeconds(attackTime - intervalM);
        canCombo = true;
    }

    IEnumerator NormalAttackM0(float time, int bulletNo, float damage, float repelDistance)
    {
        player.Attack(MouseDir(), time, true, moveSpeedM, true);
        isCombating = true;
        GameObject bulletInstance = Instantiate(bulletsM[bulletNo], transform.position, Quaternion.identity);
        bulletInstance.GetComponent<Bullet>().Setup(
            MouseDir(), 0, damage * status.GetAttack(), status.GetCritProbability(), status.GetCritRate(), time, Bullet.BulletType.penetrable);
        SetRepel(bulletInstance.GetComponent<Bullet>(), repelDistance);
        bulletInstance.transform.localScale = new Vector3(rangeM, rangeM, 1);
        bulletInstance.transform.parent = bulletsInWorld;
        yield return new WaitForSeconds(time);
        isCombating = false;
    }

    IEnumerator NormalAttackM1()
    {
        isCombating = true;
        Vector3 dir = MouseDir();
        player.Attack(dir, 0.5f, true, moveSpeedM, true);
        GameObject bulletInstance = Instantiate(bulletsM[3], ShootPos(shootOffset), Quaternion.identity);
        bulletInstance.GetComponent<Bullet>().Setup(
            dir, 0, damageM3 * status.GetAttack(), status.GetCritProbability(), status.GetCritRate(), 0.5f, Bullet.BulletType.penetrable);
        SetRepel(bulletInstance.GetComponent<Bullet>(), repelDistance3);
        bulletInstance.transform.localScale = new Vector3(rangeM, rangeM, 1);
        bulletInstance.transform.parent = bulletsInWorld;
        yield return new WaitForSeconds(0.25f);
        player.Dash(dir, 2.0f * rangeM, 0.25f);
        yield return new WaitForSeconds(0.25f);
        isCombating = false;
    }

    IEnumerator NormalAttackM2()
    {
        isCombating = true;
        Vector3 dir = MouseDir();
        player.Attack(dir, 0.5f, true, moveSpeedM, true);
        GameObject bulletInstance = Instantiate(bulletsM[4], ShootPos(shootOffset), Quaternion.identity);
        bulletInstance.GetComponent<Bullet>().Setup(
            dir, 0, damageM4 * status.GetAttack(), status.GetCritProbability(), status.GetCritRate(), 0.5f, Bullet.BulletType.penetrable);
        SetRepel(bulletInstance.GetComponent<Bullet>(), repelDistance4);
        bulletInstance.transform.localScale = new Vector3(rangeM, rangeM, 1);
        bulletInstance.transform.parent = bulletsInWorld;
        player.Dash(dir, 1.0f * rangeM, 0.17f);
        yield return new WaitForSeconds(0.33f);
        player.Dash(dir, 1.0f * rangeM, 0.17f);
        yield return new WaitForSeconds(0.17f);
        isCombating = false;
    }

    IEnumerator NormalAttackM3()
    {
        isCombating = true;
        Vector3 dir = MouseDir();
        player.Attack(dir, 1f, true, moveSpeedM, true);
        GameObject bulletInstance = Instantiate(bulletsM[5], ShootPos(shootOffset), Quaternion.identity);
        bulletInstance.GetComponent<Bullet>().Setup(
            dir, 0, damageM5 * status.GetAttack(), status.GetCritProbability(), status.GetCritRate(), 1f, Bullet.BulletType.penetrable);
        SetRepel(bulletInstance.GetComponent<Bullet>(), repelDistance5);
        bulletInstance.transform.localScale = new Vector3(rangeM, rangeM, 1);
        bulletInstance.transform.parent = bulletsInWorld;
        yield return new WaitForSeconds(0.62f);
        player.Dash(dir, 3.5f * rangeM, 0.23f);
        yield return new WaitForSeconds(0.38f);
        isCombating = false;
    }

    private void SetRepel(Bullet bullet, float distance)
    {
        bullet.SetRepel(distance);
    }

    //Metal////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void StartHurt()
    {
        StopAllCoroutines();
        GetWeapon(property);
    }

    public void FinishHurt()
    {
        GetWeapon(property);
    }
}
