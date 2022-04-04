using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float hp;
    public float moveSpeed;
    public SpriteRenderer mySprite;
    public Animator anim;
    public bool isburnning;
    public bool isWater;
    public bool isEarth;
    public bool isLighting;
    //public Weapon playerWeapon;
    //public enum BeAttackedType { fire, water, earth, thunder, metal }
    public abstract void takeDamage(float damage, string damageType);
    public abstract void move();

    public abstract void takenAttactk(string type);

    public abstract void burnning(float damge);
}
