using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("debuff")]
    public float dizzinessTime;
    public float burnningTime;
    public float waterTime;
    public float backDis;
    public int dizzinessValue;
    public bool isburnning;
    public bool isWater;
    public bool isEarth;
    public bool isLighting;
    public GameObject lightingEffect;
    public GameObject burnningEffect;
    public GameObject waterEffect;
    public GameObject dizzinessEffect;

    public float hp;
    public float moveSpeed;

    [Header("component")]
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
}
