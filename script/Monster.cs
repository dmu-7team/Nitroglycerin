using UnityEngine;

public class Monster : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float attackDamage = 10f;
    public float attackCooldown = 1f;
    public float maxHealth = 50f;
    public GameObject itemPrefab; // ü�� ȸ�� ������ ������
    public float expReward = 10f; // ���Ͱ� �ִ� ����ġ
    public int coinReward = 1; // ���Ͱ� �ִ� ����

    private Transform player;
    private Rigidbody rb;
    private float currentHealth;
    private float lastAttackTime;

    public System.Action OnMonsterDestroy;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("�÷��̾ ã�� �� �����ϴ�! player ������Ʈ�� Tag�� 'Player'�� �����Ǿ� �ִ��� Ȯ���ϼ���.", this);
        }
    }

    void Update()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 move = direction * moveSpeed;
        move.y = rb.linearVelocity.y;
        rb.linearVelocity = move;
        Debug.Log("���� ��ġ: " + transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("���� �浹 ����: " + other.name + ", �±�: " + other.tag);
        if (other.CompareTag("Player"))
        {
            Debug.Log("�÷��̾�� �浹 ������, ���� �õ�");
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack(other);
                lastAttackTime = Time.time;
            }
            else
            {
                Debug.Log("���� ��ٿ� ��: " + (Time.time - lastAttackTime) + "/" + attackCooldown);
            }
        }
        else if (other.CompareTag("Missile"))
        {
            TakeDamage(20f);
            Debug.Log("���Ͱ� �̻��Ͽ� ����: " + other.name);
        }
    }

    void Attack(Collider other)
    {
        Debug.Log("Attack �޼��� ȣ��, ���: " + other.name);
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log("���Ͱ� �÷��̾ ����: " + attackDamage + " ������");
        }
        else
        {
            Debug.LogError("PlayerHealth ������Ʈ�� ã�� �� �����ϴ�! ������Ʈ: " + other.name, this);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("���� ü��: " + currentHealth + "/" + maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Die �޼��� ���� ����");

        // ü�� ȸ�� ������ ���
        if (itemPrefab != null)
        {
            Vector3 dropPosition = transform.position;
            dropPosition.y = 1.0f;
            GameObject item = Instantiate(itemPrefab, dropPosition, Quaternion.identity);
            Debug.Log("���Ͱ� �������� ����߽��ϴ�: " + item.name + ", ��ġ: " + dropPosition);
        }
        else
        {
            Debug.LogWarning("������ �������� �������� �ʾҽ��ϴ�!", this);
        }

        // ����ġ �߰�
        PlayerStats playerStats = FindFirstObjectByType<PlayerStats>(); // ������ �κ�
        if (playerStats != null)
        {
            playerStats.AddExp(expReward);
        }
        else
        {
            Debug.LogError("PlayerStats�� ã�� �� �����ϴ�! player ������Ʈ�� PlayerStats ������Ʈ�� �ִ��� Ȯ���ϼ���.", this);
        }

        // ���� �߰�
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCoin(coinReward);
            Debug.Log($"���� ������� ���� ȹ��: {coinReward}");
        }
        else
        {
            Debug.LogError("GameManager�� ã�� �� �����ϴ�! GameManager ������Ʈ�� ���� �ִ��� Ȯ���ϼ���.", this);
        }

        Destroy(gameObject);
        Debug.Log("���Ͱ� �ı��Ǿ����ϴ�!");
    }

    void OnDestroy()
    {
        OnMonsterDestroy?.Invoke();
    }
}