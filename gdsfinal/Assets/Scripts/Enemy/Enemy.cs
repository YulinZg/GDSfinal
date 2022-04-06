using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Earth")]
    public float dizzinessTime;
    public int dizzinessValue;
    public bool isEarth;
    public int increaseDizziness;
    public GameObject dizzinessEffect;
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
    public abstract void takeDamage(float damage, string damageType);
    public abstract void dizziness();
    public abstract void exitDizziness();
    public abstract void move();

    public abstract void takenAttactk(string type);

    public abstract void burnning(float damge);

    public abstract void goBack(float dis);

    public abstract void paralysis(float time);
}
