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
    [SerializeField] private Transform fireRotater;
    [SerializeField] private Transform waterRotater;
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

    [SerializeField] private float skillDamageF;
    [SerializeField] private float skillRangeF;
    [SerializeField] private float cooldownTimeF;
    [SerializeField] private float markTime;
    [SerializeField] private float explosionDamage;
    [SerializeField] private GameObject skillF;
    private float cooldownTimerF = 0;

    [Header("Water")]
    [SerializeField] private float damageW;
    [SerializeField] private float damageW1;
    [SerializeField] private float damageW2;
    [SerializeField] private float damageW3;
    [SerializeField] private float damageW4;
    [SerializeField] private float intervalW;
    [SerializeField] private float bulletSpeedW;
    [SerializeField] private float rangeW;
    [SerializeField] private float rangeW1;
    [SerializeField] private float rangeW2;
    [SerializeField] private float rangeW3;
    [SerializeField] private float moveSpeedW;
    [SerializeField] private float moveSpeedW1;
    [SerializeField] private float standTimeW;
    [SerializeField] private float standTimeW1;
    [SerializeField] private float chargeTime;
    [SerializeField] private float sprayTime;
    [SerializeField] private float sprayRotateSpeed;
    [SerializeField] private GameObject[] bulletsW;
    private float chargeTimer = 0;
    private bool isCharging = false;
    private bool charged1 = false;
    private bool charged2 = false;
    private bool isSpraying = false;
    private bool isPassiveAttacking = false;
    private bool spawnedEffect1 = false;
    private bool spawnedEffect2 = false;
    private bool spawned2Effect2 = false;

    [SerializeField] private float decelerateRate;
    [SerializeField] private float decelerateTime;

    [SerializeField] private float skillDamageW;
    [SerializeField] private float skillTimeW;
    [SerializeField] private float skillRangeW;
    [SerializeField] private float cooldownTimeW;
    [SerializeField] private GameObject skillW;
    private float cooldownTimerW = 0;

    [Header("Earth")]
    [SerializeField] private float damageE;
    [SerializeField] private float damageE1;
    [SerializeField] private float intervalE;
    [SerializeField] private float bulletSpeedE;
    [SerializeField] private float bulletSpeedE1;
    [SerializeField] private float rangeE;
    [SerializeField] private float moveSpeedE;
    [SerializeField] private float moveSpeedE1;
    [SerializeField] private int preheatTimes;
    [SerializeField] private float scatterAngle;
    [SerializeField] private int rockTimes;
    [SerializeField] private GameObject[] bulletsE;
    private int preheatBullet;
    private bool isShooting = false;
    private bool isAiming = false;
    private int shotTimes = 0;
    private int nextRockTimes;

    [SerializeField] private float stunValue;
    [SerializeField] private float stunValue1;
    [SerializeField] private float stunValueSkill;
    [SerializeField] private float stunTime;

    [SerializeField] private float skillDamageE;
    [SerializeField] private float skillTimeE;
    [SerializeField] private float skillRangeEMin;
    [SerializeField] private float skillRangeEMax;
    [SerializeField] private float cooldownTimeE;
    [SerializeField] private float dropInterval;
    [SerializeField] private GameObject[] skillE;
    private float cooldownTimerE = 0;
    private float dropTimer = 0;
    private bool isDroping = false;

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

    [SerializeField] private float skillDamageL;
    [SerializeField] private float skillTimeL;
    [SerializeField] private float skillRangeL;
    [SerializeField] private float cooldownTimeL;
    [SerializeField] private GameObject[] skillL;
    private float cooldownTimerL = 0;
    private float skillTimerL = 0;
    private bool hasMoved = false;
    private Transform backPos;

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
        if (cooldownTimerF != 0)
        {
            cooldownTimerF -= Time.deltaTime;
            if (cooldownTimerF < 0)
                cooldownTimerF = 0;
        }
        if (cooldownTimerW != 0)
        {
            cooldownTimerW -= Time.deltaTime;
            if (cooldownTimerW < 0)
                cooldownTimerW = 0;
        }
        if (cooldownTimerE != 0)
        {
            cooldownTimerE -= Time.deltaTime;
            if (cooldownTimerE < 0)
                cooldownTimerE = 0;
        }
        if (isDroping)
            Drop();
        if (cooldownTimerL != 0)
        {
            cooldownTimerL -= Time.deltaTime;
            if (cooldownTimerL < 0)
                cooldownTimerL = 0;
        }
        if (hasMoved)
        {
            skillTimerL -= Time.deltaTime;
            if (skillTimerL <= 0)
            {
                hasMoved = false;
                Destroy(backPos.gameObject);
                cooldownTimerL = cooldownTimeL;
            }
        }
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
                isPassiveAttacking = false;
                spawnedEffect1 = false;
                spawnedEffect2 = false;
                spawned2Effect2 = false;
                break;
            case Property.earth:
                player.SetSpeed(moveSpeedE);
                preheatBullet = preheatTimes;
                isShooting = false;
                isAiming = false;
                shotTimes = 0;
                nextRockTimes = Random.Range(Mathf.FloorToInt(rockTimes * 0.5f), Mathf.FloorToInt(rockTimes * 1.5f));
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
        if (bulletsInWorld.childCount > 0)
            foreach (Transform child in bulletsInWorld)
                Destroy(child.gameObject);
        if (waterRotater.childCount > 0)
            foreach (Transform child in waterRotater)
                Destroy(child.gameObject);
        if (effectInstance)
            Destroy(effectInstance);
        if (effectInstance1)
            Destroy(effectInstance1);
    }

    private Vector3 MouseDir()
    {
        return Direction(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position);
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

    private void SpawnEffect(int effectNo)
    {
        effectInstance1 = Instantiate(effects[effectNo], transform);
    }

    private void SpawnEffectInstance(int effectNo)
    {
        effectInstance = Instantiate(effects[effectNo], transform);
    }

    private float GetDamage(float damage)
    {
        return damage * status.GetAttack();
    }

    //Fire/////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Fire()
    {
        if (Input.GetMouseButtonDown(0) && !player.isSkilling)
        {
            if (shootTimer >= intervalF)
            {
                NormalAttackF(bulletNumber);
                shootTimer = 0;
            }
        }
        if (Input.GetMouseButtonDown(1) && !player.isSkilling)
        {
            NormalAttackF1();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!player.isAttacking && cooldownTimerF == 0)
            {
                SkillF();
                cooldownTimerF = cooldownTimeF;
            }
        }
    }

    private void NormalAttackF(int bulletNum)
    {
        if (fireRotater.childCount > 0)
            foreach (Transform child in fireRotater)
                Destroy(child.gameObject);
        Bullet bulletInstance;
        canLaunch = false;
        for (int i = 0; i < bulletNumber; i++)
        {
            bulletInstance = Instantiate(bulletsF[0], transform.position, Quaternion.identity).GetComponent<Bullet>();
            bulletInstance.transform.parent = fireRotater;
            bulletInstance.Setup(RotateVector(MouseDir(), (360f / bulletNum) * (i + 1)), bulletRotateRange / standTimeF, GetDamage(damageF), status.GetCritProbability(), status.GetCritRate(), standTimeF + bulletLifeTimeBase * (i + 1), Bullet.BulletType.normal);
            bulletInstance.SetFire(bulletRotateRange);
            SetBurn(bulletInstance);
        }
        Invoke(nameof(SetCanLaunch), standTimeF);
        player.Attack(MouseDir(), standTimeF, true, moveSpeedF, true, false);
        SpawnEffect(0);
    }

    private void SetCanLaunch()
    {
        canLaunch = true;
    }

    private void NormalAttackF1()
    {
        if (fireRotater.childCount > 0 && canLaunch)
        {
            Bullet bulletInstance;
            foreach (Transform child in fireRotater)
            {
                bulletInstance = Instantiate(bulletsF[1], child.position, Quaternion.identity).GetComponent<Bullet>();
                bulletInstance.Setup(Direction(Camera.main.ScreenToWorldPoint(Input.mousePosition), child.position), bulletSpeedF, GetDamage(damageF), status.GetCritProbability(), status.GetCritRate(), rangeF / bulletSpeedF, Bullet.BulletType.normal);
                SetBurn(bulletInstance);
                Destroy(child.gameObject);
            }
            player.Attack(MouseDir(), 0.2f, false, moveSpeedF, false, false);
        }
    }

    private void RotateBullet(float speed)
    {
        fireRotater.Rotate(0, 0, speed * Time.deltaTime);
    }

    private void SetBurn(Bullet bullet)
    {
        bullet.SetBurn(burnDamage, burnTime, burnInterval);
    }

    private void SkillF()
    {
        Bullet bulletInstance = Instantiate(skillF, transform).GetComponentInChildren<Bullet>();
        bulletInstance.Setup(Vector2.right, 0, GetDamage(skillDamageF), status.GetCritProbability(), status.GetCritRate(), 1f, Bullet.BulletType.penetrable);
        bulletInstance.SetFireSkill(markTime, GetDamage(explosionDamage));
        SetBurn(bulletInstance);
        bulletInstance.transform.parent.localScale = new Vector3(skillRangeF, skillRangeF, 1);
        player.Attack(MouseDir(), 1f, true, moveSpeedF, true, true);
    }

    //Fire/////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Water////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Water()
    {
        if (Input.GetMouseButtonDown(0) && !isSpraying && !player.isSkilling)
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
            //NormalAttackW1(rangeW3, rangeW3 * 0.25f, damageW3);
            PassiveAttackW(rangeW3, rangeW3 * 0.25f, sprayTime);
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

        if (isPassiveAttacking)
        {
            float angle = Vector3.SignedAngle(waterRotater.up, Camera.main.ScreenToWorldPoint(Input.mousePosition) - waterRotater.position, Vector3.forward);
            waterRotater.Rotate(new Vector3(0, 0, Mathf.Sign(angle) * sprayRotateSpeed * Time.deltaTime));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!player.isAttacking && cooldownTimerW == 0)
            {
                SkillW();
                cooldownTimerW = cooldownTimeW;
            }
        }
    }

    private void NormalAttackW()
    {
        isCharging = false;
        charged1 = false;
        charged2 = false;
        chargeTimer = 0;
        Bullet bulletInstance = Instantiate(bulletsW[0], ShootPos(shootOffset), Quaternion.identity).GetComponent<Bullet>();
        bulletInstance.Setup(MouseDir(), bulletSpeedW, GetDamage(damageW), status.GetCritProbability(), status.GetCritRate(), rangeW / bulletSpeedW, Bullet.BulletType.normal);
        SetDecelerate(bulletInstance);
        player.Attack(MouseDir(), standTimeW, true, moveSpeedW, false, false);
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
        Bullet bulletInstance = Instantiate(bulletsW[1], ShootPos(rangeY * 0.5f + shootOffset), Quaternion.identity).GetComponent<Bullet>();
        bulletInstance.Setup(MouseDir(), 0, GetDamage(damage), status.GetCritProbability(), status.GetCritRate(), standTimeW1, Bullet.BulletType.penetrable);
        SetDecelerate(bulletInstance);
        bulletInstance.transform.localScale = new Vector3(rangeY, rangeX, 1);
        bulletInstance.transform.parent = bulletsInWorld;
        player.Attack(MouseDir(), standTimeW1, true, moveSpeedW, false, false);
        spawnedEffect1 = false;
        spawnedEffect2 = false;
        spawned2Effect2 = false;
        Invoke(nameof(SetNotSpraying), standTimeW1);
        if (effectInstance)
            Destroy(effectInstance);
    }

    private void PassiveAttackW(float rangeY, float rangeX, float attackTime)
    {
        isSpraying = true;
        isPassiveAttacking = true;
        isCharging = false;
        charged1 = false;
        charged2 = false;
        chargeTimer = 0; 
        float angle = Mathf.Atan2(MouseDir().y, MouseDir().x) * Mathf.Rad2Deg;
        waterRotater.rotation = Quaternion.Euler(0, 0, angle - 90);
        Bullet bulletInstance = Instantiate(bulletsW[2], ShootPos(rangeY * 0.5f + shootOffset), Quaternion.identity).GetComponent<Bullet>();
        bulletInstance.Setup(MouseDir(), 0, GetDamage(damageW4), status.GetCritProbability(), status.GetCritRate(), attackTime, Bullet.BulletType.penetrable);
        SetDecelerate(bulletInstance);
        bulletInstance.transform.localScale = new Vector3(rangeY, rangeX, 1);
        bulletInstance.transform.parent = waterRotater;
        player.Attack(MouseDir(), attackTime, true, moveSpeedW, true, false);
        spawnedEffect1 = false;
        spawnedEffect2 = false;
        spawned2Effect2 = false;
        Invoke(nameof(SetNotSpraying), attackTime);
        if (effectInstance)
            Destroy(effectInstance);
    }

    private void SetNotSpraying()
    {
        isSpraying = false;
        isPassiveAttacking = false;
    }

    private void SetDecelerate(Bullet bullet)
    {
        bullet.SetDecelerate(decelerateRate, decelerateTime);
    }

    private void SkillW()
    {
        GetWeapon(property);
        Bullet bulletInstance = Instantiate(skillW, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity).GetComponentInChildren<Bullet>();
        bulletInstance.Setup(Vector2.right, 0, GetDamage(skillDamageW), status.GetCritProbability(), status.GetCritRate(), skillTimeW, Bullet.BulletType.penetrable);
        SetDecelerate(bulletInstance);
        bulletInstance.transform.parent.localScale = new Vector3(skillRangeW, skillRangeW, 1);
        player.Attack(MouseDir(), 0.5f, true, moveSpeedW, false, true);
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
        else if (Input.GetMouseButtonUp(1))
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
            player.Attack(MouseDir(), Time.deltaTime * 5, false, moveSpeedE, true, false);
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!player.isAttacking && cooldownTimerE == 0 && !isDroping)
                SkillE();
        }
    }

    private void NormalAttackE(Vector3 dir)
    {
        Bullet bulletInstance = Instantiate(bulletsE[0], ShootPos(shootOffset), Quaternion.identity).GetComponent<Bullet>();
        bulletInstance.Setup(dir, bulletSpeedE, GetDamage(damageE), status.GetCritProbability(), status.GetCritRate(), rangeE / bulletSpeedE, Bullet.BulletType.normal);
        SetStun(bulletInstance, stunValue);
        PassiveAttackE();
    }

    private void PassiveAttackE()
    {
        shotTimes++;
        if (shotTimes >= nextRockTimes)
        {
            Bullet bulletInstance = Instantiate(bulletsE[1], transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized, Quaternion.identity).GetComponent<Bullet>();
            bulletInstance.Setup(Direction(Camera.main.ScreenToWorldPoint(Input.mousePosition), bulletInstance.transform.position), bulletSpeedE1, GetDamage(damageE1), status.GetCritProbability(), status.GetCritRate(), rangeE / bulletSpeedE1, Bullet.BulletType.normal);
            SetStun(bulletInstance, stunValue1);
            nextRockTimes = Random.Range(Mathf.FloorToInt(rockTimes * 0.5f), Mathf.FloorToInt(rockTimes * 1.5f));
            shotTimes = 0;
        }
    }

    private void SetStun(Bullet bullet, float value)
    {
        bullet.SetStun(value, stunTime);
    }

    private void SkillE()
    {
        GetWeapon(property);
        Instantiate(skillE[0], transform);
        player.Attack(Vector2.right, 1f, true, moveSpeedE, true, true);
        Invoke(nameof(StartDrop), 1f);
    }

    private void StartDrop()
    {
        isDroping = true;
        dropTimer = 0;
        Invoke(nameof(StopDrop), skillTimeE);
    }

    private void StopDrop()
    {
        isDroping = false;
        cooldownTimerE = cooldownTimeE;
    }

    private void Drop()
    {
        if (dropTimer <= 0)
        {
            earthSkill earthSkill = Instantiate(skillE[1], transform.position + RotateVector(Vector3.right, Random.Range(0, 360f)) * Random.Range(skillRangeEMin, skillRangeEMax) + new Vector3(0, 16F + skillRangeEMax, 0), Quaternion.identity).GetComponent<earthSkill>();
            earthSkill.Setup(GetDamage(skillDamageE), status.GetCritProbability(), status.GetCritRate(), stunValueSkill, stunTime, 16f + skillRangeEMax);
            dropTimer = dropInterval;
        }
        dropTimer -= Time.deltaTime;
    }

    //Earth////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Lightning////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Lightning()
    {
        if (Input.GetMouseButtonDown(0) && !isLinking && !player.isSkilling)
        {
            if (shootTimer >= intervalL && lightningBalls.childCount < maxBulletNum)
            {
                NormalAttackL();
                shootTimer = 0;
            }
        }
        if (Input.GetMouseButtonDown(1) && !isLinking && !player.isSkilling)
        {
            if (shootTimer >= intervalL && lightningBalls.childCount > 0)
            {
                NormalAttackL1();
                shootTimer = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!player.isAttacking && cooldownTimerL == 0)
                SkillL();
        }
    }

    private void NormalAttackL()
    {
        Bullet bulletInstance = Instantiate(bulletsL[0], ShootPos(shootOffset), Quaternion.identity).GetComponent<Bullet>();
        bulletInstance.Setup(MouseDir(), 0, GetDamage(damageL), status.GetCritProbability(), status.GetCritRate(), stayTime, Bullet.BulletType.penetrable);
        SetPalsy(bulletInstance);
        bulletInstance.transform.parent = lightningBalls;
        player.Attack(MouseDir(), 0.2f, false, moveSpeedL, false, false);
    }

    private void NormalAttackL1()
    {
        isLinking = true;
        foreach (Transform child in lightningBalls)
        {
                Bullet bulletInstance = Instantiate(bulletsL[0], child.position, Quaternion.identity).GetComponent<Bullet>();
                bulletInstance.Setup(Direction(transform.position, child.position), 0, GetDamage(damageL), status.GetCritProbability(), status.GetCritRate(), standTimeL, Bullet.BulletType.penetrable);
                SetPalsy(bulletInstance);
                bulletInstance = Instantiate(bulletsL[1], 0.5f * Vector3.Distance(child.position, transform.position) * Direction(transform.position, child.position) + child.position, Quaternion.identity).GetComponent<Bullet>();
                bulletInstance.Setup(Direction(transform.position, child.position), 0, GetDamage(damageL1), status.GetCritProbability(), status.GetCritRate(), standTimeL, Bullet.BulletType.penetrable);
                SetPalsy(bulletInstance);
                bulletInstance.transform.localScale = new Vector3(Vector3.Distance(child.position, transform.position), 2, 1);
                bulletInstance.transform.parent = bulletsInWorld;
                Destroy(child.gameObject);
        }
        player.Attack(MouseDir(), standTimeL, true, moveSpeedL, true, false);
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

    private void SkillL()
    {
        if (!hasMoved)
        {
            backPos = Instantiate(skillL[0], transform.position, Quaternion.identity).transform;
            Dash(MouseDir(), skillRangeL);
            skillTimerL = skillTimeL;
            hasMoved = true;
        }
        else
        {
            NormalAttackL();
            Dash(Direction(backPos.position, transform.position), Vector3.Distance(backPos.position, transform.position));
            hasMoved = false;
            Destroy(backPos.gameObject);
            cooldownTimerL = cooldownTimeL;
        }
    }

    private void Dash(Vector3 dir, float distance)
    {
        player.Attack(dir, 0.1f, true, moveSpeedL, false, true);
        player.Dash(dir, distance, 0.1f);
        Bullet bulletInstance = Instantiate(skillL[1], transform).GetComponent<Bullet>();
        bulletInstance.Setup(dir, 0, GetDamage(skillDamageL), status.GetCritProbability(), status.GetCritRate(), 0.25f, Bullet.BulletType.penetrable);
        SetPalsy(bulletInstance);
        gameObject.layer = 8;
        foreach (Transform child in transform)
            if (child.name == "collider")
                child.gameObject.layer = 8;
        GetComponent<SpriteRenderer>().color = Color.clear;
        Invoke(nameof(EndDash), 0.1f);
    }

    private void EndDash()
    {
        gameObject.layer = 3;
        foreach (Transform child in transform)
            if (child.name == "collider")
                child.gameObject.layer = 3;
        GetComponent<SpriteRenderer>().color = Color.white;
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
        player.Attack(MouseDir(), time, true, moveSpeedM, true, false);
        isCombating = true;
        Bullet bulletInstance = Instantiate(bulletsM[bulletNo], transform.position, Quaternion.identity).GetComponent<Bullet>();
        bulletInstance.Setup(MouseDir(), 0, GetDamage(damage), status.GetCritProbability(), status.GetCritRate(), time, Bullet.BulletType.penetrable);
        SetRepel(bulletInstance, repelDistance);
        bulletInstance.transform.localScale = new Vector3(rangeM, rangeM, 1);
        bulletInstance.transform.parent = bulletsInWorld;
        yield return new WaitForSeconds(time);
        isCombating = false;
    }

    IEnumerator NormalAttackM1()
    {
        isCombating = true;
        Vector3 dir = MouseDir();
        player.Attack(dir, 0.5f, true, moveSpeedM, true, false);
        Bullet bulletInstance = Instantiate(bulletsM[3], ShootPos(shootOffset), Quaternion.identity).GetComponent<Bullet>();
        bulletInstance.Setup(
            dir, 0, GetDamage(damageM3), status.GetCritProbability(), status.GetCritRate(), 0.5f, Bullet.BulletType.penetrable);
        SetRepel(bulletInstance, repelDistance3);
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
        player.Attack(dir, 0.5f, true, moveSpeedM, true, false);
        Bullet bulletInstance = Instantiate(bulletsM[4], ShootPos(shootOffset), Quaternion.identity).GetComponent<Bullet>();
        bulletInstance.Setup(
            dir, 0, GetDamage(damageM4), status.GetCritProbability(), status.GetCritRate(), 0.5f, Bullet.BulletType.penetrable);
        SetRepel(bulletInstance, repelDistance4);
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
        player.Attack(dir, 1f, true, moveSpeedM, true, false);
        Bullet bulletInstance = Instantiate(bulletsM[5], ShootPos(shootOffset), Quaternion.identity).GetComponent<Bullet>();
        bulletInstance.Setup(
            dir, 0, GetDamage(damageM5), status.GetCritProbability(), status.GetCritRate(), 1f, Bullet.BulletType.penetrable);
        SetRepel(bulletInstance, repelDistance5);
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
}
