using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public bool canInput = true;

    private Vector3 moveDir;
    private Rigidbody2D rigid;
    private Weapon weapon;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        weapon = GetComponent<Weapon>();
    }

    // Start is called before the first frame update
    void Start()
    {
        weapon.GetWeapon(Weapon.Property.fire);
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = 0;
        float moveY = 0;
        if (Input.GetKey(KeyCode.W))
        {
            moveY = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1f;
        }
        moveDir = new Vector3(moveX, moveY).normalized;

        if (canInput)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                weapon.GetWeapon(Weapon.Property.fire);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                weapon.GetWeapon(Weapon.Property.water);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                weapon.GetWeapon(Weapon.Property.earth);
            if (Input.GetKeyDown(KeyCode.Alpha4))
                weapon.GetWeapon(Weapon.Property.thunder);
            if (Input.GetKeyDown(KeyCode.Alpha5))
                weapon.GetWeapon(Weapon.Property.metal);
        }
    }

    private void FixedUpdate()
    {
        rigid.velocity = moveDir * speed;
        if (canInput)
        {
            Rotate();
        }
    }

    void Rotate()
    {
        transform.right = (Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        //Vector3 dir = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
