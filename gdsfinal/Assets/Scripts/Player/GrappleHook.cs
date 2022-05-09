using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    public enum HitType
    {
        none,
        wall,
        enemy,
        boss
    }

    [SerializeField] private LayerMask layer;
    [SerializeField] private GameObject hook;
    [SerializeField] private float shootSpeed;
    [SerializeField] private float grappleSpeed;

    public bool isInvincible = false;
    private LineRenderer line;
    private Vector2 target;
    private GameObject enemy;
    private PlayerController player;
    private Status status;
    private bool isShooting = false;
    private bool isGrappling = false;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        player = GetComponent<PlayerController>();
        status = GetComponent<Status>();
    }

    private void Update()
    {
        if (isGrappling)
        {
            line.SetPosition(0, transform.position);
        }
    }

    public void Grapple(Vector2 dir, float range, float resetSpeed, float damage)
    {
        if (!isShooting)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, range, layer);
            if (hit.collider)
            {
                target = hit.point;
                if (hit.collider.CompareTag("Enemy"))
                {
                    enemy = hit.collider.gameObject;
                    StartCoroutine(Grappling(dir, Vector2.Distance(transform.position, target), resetSpeed, HitType.enemy, damage));
                }
                else if (hit.collider.CompareTag("Boss"))
                    StartCoroutine(Grappling(dir, Vector2.Distance(transform.position, target), resetSpeed, HitType.boss, damage));
                else
                    StartCoroutine(Grappling(dir, Vector2.Distance(transform.position, target), resetSpeed, HitType.wall, 0));
            }
            else
            {
                target = (Vector2)transform.position + dir * range;
                StartCoroutine(Grappling(dir, Vector2.Distance(transform.position, target), resetSpeed, HitType.none, 0));
            }
        }
    }

    IEnumerator Grappling(Vector2 dir, float distance, float resetSpeed, HitType hitType, float damage)
    {
        if (isInvincible)
        {
            gameObject.layer = 8;
            player.col.layer = 8;
        }
        isShooting = true;
        line.enabled = true;
        line.positionCount = 2;
        player.Attack(dir, distance / shootSpeed + distance / grappleSpeed, true, resetSpeed, false, true);
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);
        GameObject hookInstance = Instantiate(hook, transform.position, Quaternion.identity);
        hookInstance.GetComponent<Bullet>().Setup(dir, 0, damage, status.GetCritProbability(), status.GetCritRate(), 10f, Bullet.BulletType.penetrable);
        if (hitType != HitType.enemy && hitType != HitType.boss)
            hookInstance.GetComponent<Collider2D>().enabled = false;
        float timer = 0;
        float duration = distance / shootSpeed;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            line.SetPosition(1, Vector2.Lerp(transform.position, target, timer / duration));
            if (hookInstance)
                hookInstance.transform.position = line.GetPosition(1);
            yield return null;
        }
        line.SetPosition(1, target);
        isGrappling = true;
        switch(hitType)
        {
            case HitType.none:
                timer = 0;
                float duration1 = distance / grappleSpeed;
                while (timer < duration1)
                {
                    timer += Time.deltaTime;
                    line.SetPosition(1, Vector2.Lerp(target, transform.position, timer / duration1));
                    if (hookInstance)
                        hookInstance.transform.position = line.GetPosition(1);
                    yield return null;
                }
                break;
            case HitType.wall:
            case HitType.boss:
                yield return StartCoroutine(player.Dashing(dir, distance, distance / grappleSpeed));
                break;
            case HitType.enemy:
                if (enemy)
                    enemy.transform.parent = hookInstance.transform;
                timer = 0;
                float duration2 = distance / grappleSpeed;
                while (timer < duration2)
                {
                    timer += Time.deltaTime;
                    line.SetPosition(1, Vector2.Lerp(target, transform.position, timer / duration2));
                    if (hookInstance)
                        hookInstance.transform.position = line.GetPosition(1);
                    yield return null;
                }
                if (enemy)
                    enemy.transform.parent = null;
                break;
        }
        Destroy(hookInstance);
        isShooting = false;
        isGrappling = false;
        line.enabled = false; 
        if (isInvincible)
        {
            gameObject.layer = 3;
            player.col.layer = 3;
        }
    }
}
