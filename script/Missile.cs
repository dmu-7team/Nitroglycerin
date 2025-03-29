using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 20f;
    public float lifetime = 5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        Vector3 direction = transform.forward;
        direction.y = 0;
        direction = direction.normalized;
        rb.linearVelocity = direction * speed;
        Debug.Log("미사일 초기 속도: " + rb.linearVelocity);
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        Debug.Log("미사일 위치: " + transform.position + ", 속도: " + rb.linearVelocity);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("미사일 충돌 감지: " + other.name + ", 태그: " + other.tag);
        if (other.CompareTag("Monster"))
        {
            Monster monster = other.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(damage);
                Debug.Log("미사일이 몬스터에 맞음: " + other.name);
            }
            Destroy(gameObject);
        }
    }
}