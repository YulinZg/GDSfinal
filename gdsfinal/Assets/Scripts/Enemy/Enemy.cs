using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float health;
    public float moveSpeed;
    public float burnResistance;
    public float decelerateResistance;
    public float maxStunValue;
    public float palsyResistance;
    public float repelResistance;

    public Transform player;
    public GameObject damageText;
    public SpriteRenderer sprite;
    public Rigidbody2D rigid;
    public Animator anim;

    public float effectSize; 
    public float effectOffsetY;
    public GameObject burnEffect;
    public GameObject decelerateEffect;
    public GameObject stunEffect;
    public GameObject palsyEffect;

    public bool isBurn = false;
    public bool isDecelerate = false;
    public bool isStun = false;
    public bool isPalsy = false;
    public bool isRepel = false;

    public abstract void Move();

    public abstract void TakeDamage(float damage);

    public abstract void Burn(float damage, float time, float interval);

    public abstract void Decelerate(float rate, float time);

    public abstract void Stun(float value, float time);

    public abstract void Palsy(float damage, float time, float interval);

    public abstract void Repel(float distance);
}
