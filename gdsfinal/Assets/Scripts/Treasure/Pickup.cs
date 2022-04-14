using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private PickupEffect pickupEffect;
    [SerializeField] private bool randomRotate;
    public string title;
    public string description;
    private Rigidbody2D rigid;
    private BoxCollider2D col;
    private bool canFlow = false;
    private float timer = 0;
    private int dir = -1;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (randomRotate)
            transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
        rigid.AddForce(Random.Range(4f, 6f) * RotateVector(Vector3.up, Random.Range(15f, 30f) * (Random.Range(2, 4) % 3 - 1)), ForceMode2D.Impulse);
        Invoke(nameof(StopFalling), Random.Range(0.5f, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        if (canFlow)
        {
            timer += Time.deltaTime;
            if (timer >= 0.5f)
            {
                dir *= -1;
                timer = 0;
            }
            transform.position += 0.5f * dir * Time.deltaTime * Vector3.up;
        }
    }

    private Vector3 RotateVector(Vector3 vector, float degree)
    {
        return new Vector3(
            vector.x * Mathf.Cos(degree * Mathf.Deg2Rad) - vector.y * Mathf.Sin(degree * Mathf.Deg2Rad),
            vector.x * Mathf.Sin(degree * Mathf.Deg2Rad) + vector.y * Mathf.Cos(degree * Mathf.Deg2Rad),
            0);
    }

    private void StopFalling()
    {
        rigid.bodyType = RigidbodyType2D.Kinematic;
        rigid.velocity = Vector2.zero;
        col.enabled = true;
        canFlow = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pickupEffect.OnPickup(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
