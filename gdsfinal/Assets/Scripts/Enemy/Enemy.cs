using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

    [Header("Fire")]
    public float burnningTime;
    public int burnningTimes;
    public float burnningDamage;
    public bool isburnning;
    public GameObject burnningEffect;

    [Header("Water")]
    public float waterTime;
    public bool isWater;
    public GameObject waterEffect;

    [Header("Earth")]
    public float dizzinessTime;
    public int dizzinessValue;
    public bool isEarth;
    public int increaseDizziness;
    public GameObject dizzinessEffect;

    [Header("Lightning")]
    public float lightningTime;
    public int lightningTimes;
    public float lightningDamage;
    public bool isLighting;
    public bool isParalysis;
    public Sprite paralysisSprit;
    public float paralysisTime;
    public GameObject lightingEffect;
    public GameObject bigLightingEffect;

    [Header("Metal")]
    public float backDis;

    [Header("Enemy Value")]
    public float hp;
    public float moveSpeed;
    public float resistance;

    [Header("Component")]
    public GameObject damageText;
    public SpriteRenderer mySprite;
    public Rigidbody2D rid;
    public Animator anim;
   
    //public Weapon playerWeapon;
    //public enum BeAttackedType { fire, water, earth, thunder, metal }
    public abstract void TakeDamage(float damage, string damageType);
    public abstract void Dizziness();
    public abstract void ExitDizziness();
    public abstract void Move();

    public abstract void TakenAttactk(string type);

    public abstract void Burnning(float damge);

    public abstract void GoBack(float dis);

    public abstract void Paralysis(float time);
}
