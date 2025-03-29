using UnityEngine;

public class Monster : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float attackDamage = 10f;
    public float attackCooldown = 1f;
    public float maxHealth = 50f;
    public GameObject itemPrefab; // 체력 회복 아이템 프리팹
    public float expReward = 10f; // 몬스터가 주는 경험치
    public int coinReward = 1; // 몬스터가 주는 코인

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
            Debug.LogError("플레이어를 찾을 수 없습니다! player 오브젝트의 Tag가 'Player'로 설정되어 있는지 확인하세요.", this);
        }
    }

    void Update()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 move = direction * moveSpeed;
        move.y = rb.linearVelocity.y;
        rb.linearVelocity = move;
        Debug.Log("몬스터 위치: " + transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("몬스터 충돌 감지: " + other.name + ", 태그: " + other.tag);
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어와 충돌 감지됨, 공격 시도");
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack(other);
                lastAttackTime = Time.time;
            }
            else
            {
                Debug.Log("공격 쿨다운 중: " + (Time.time - lastAttackTime) + "/" + attackCooldown);
            }
        }
        else if (other.CompareTag("Missile"))
        {
            TakeDamage(20f);
            Debug.Log("몬스터가 미사일에 맞음: " + other.name);
        }
    }

    void Attack(Collider other)
    {
        Debug.Log("Attack 메서드 호출, 대상: " + other.name);
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log("몬스터가 플레이어를 공격: " + attackDamage + " 데미지");
        }
        else
        {
            Debug.LogError("PlayerHealth 컴포넌트를 찾을 수 없습니다! 오브젝트: " + other.name, this);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("몬스터 체력: " + currentHealth + "/" + maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Die 메서드 실행 시작");

        // 체력 회복 아이템 드롭
        if (itemPrefab != null)
        {
            Vector3 dropPosition = transform.position;
            dropPosition.y = 1.0f;
            GameObject item = Instantiate(itemPrefab, dropPosition, Quaternion.identity);
            Debug.Log("몬스터가 아이템을 드롭했습니다: " + item.name + ", 위치: " + dropPosition);
        }
        else
        {
            Debug.LogWarning("아이템 프리팹이 설정되지 않았습니다!", this);
        }

        // 경험치 추가
        PlayerStats playerStats = FindFirstObjectByType<PlayerStats>(); // 수정된 부분
        if (playerStats != null)
        {
            playerStats.AddExp(expReward);
        }
        else
        {
            Debug.LogError("PlayerStats를 찾을 수 없습니다! player 오브젝트에 PlayerStats 컴포넌트가 있는지 확인하세요.", this);
        }

        // 코인 추가
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCoin(coinReward);
            Debug.Log($"몬스터 사망으로 코인 획득: {coinReward}");
        }
        else
        {
            Debug.LogError("GameManager를 찾을 수 없습니다! GameManager 오브젝트가 씬에 있는지 확인하세요.", this);
        }

        Destroy(gameObject);
        Debug.Log("몬스터가 파괴되었습니다!");
    }

    void OnDestroy()
    {
        OnMonsterDestroy?.Invoke();
    }
}