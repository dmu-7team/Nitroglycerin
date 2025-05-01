using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("몬스터 설정")]
    public float moveSpeed = 3f;
    public float attackDamage = 10f;
    public float attackCooldown = 1f;
    public float maxHealth = 50f;
    public GameObject treasureChestPrefab;
    public float treasureChestDropChance = 0.5f;
    public float expReward = 10f;
    public int coinReward = 1;

    [Header("미니맵 설정")]
    public RectTransform miniMapIconPrefab;    // 미니맵에 띄울 몬스터 아이콘 프리팹
    private Transform miniMapIconsParent;      // 미니맵 아이콘들이 붙는 부모 (런타임에 찾음)
    private RectTransform miniMapIconInstance; // 생성된 아이콘 인스턴스

    private Transform player;
    private Rigidbody rb;
    private float currentHealth;
    private float lastAttackTime;

    public System.Action OnMonsterDestroy;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("플레이어를 찾을 수 없습니다! Player 태그 확인 필요.", this);
        }

        // MiniMapIconsParent 자동 찾기
        if (miniMapIconsParent == null)
        {
            GameObject parentObj = GameObject.Find("MiniMapIconsParent");
            if (parentObj != null)
            {
                miniMapIconsParent = parentObj.transform;
            }
            else
            {
                Debug.LogError("MiniMapIconsParent 오브젝트를 찾을 수 없습니다!", this);
            }
        }

        // 미니맵 아이콘 생성
        if (miniMapIconPrefab != null && miniMapIconsParent != null)
        {
            miniMapIconInstance = Instantiate(miniMapIconPrefab, miniMapIconsParent);
            MiniMapManager miniMapManager = FindFirstObjectByType<MiniMapManager>(); // 최신 API
            if (miniMapManager != null)
            {
                MiniMapIconData iconData = new MiniMapIconData
                {
                    target = transform,
                    icon = miniMapIconInstance
                };
                miniMapManager.AddMonsterIcon(iconData);
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 move = direction * moveSpeed;
        move.y = rb.linearVelocity.y; // 최신 API: velocity → linearVelocity
        rb.linearVelocity = move;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack(other);
                lastAttackTime = Time.time;
            }
        }
        else if (other.CompareTag("Missile"))
        {
            TakeDamage(20f);
        }
    }

    void Attack(Collider other)
    {
        PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
        if (playerStatus != null)
        {
            playerStatus.TakeDamage((int)attackDamage);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 보물상자 드롭
        if (Random.value <= treasureChestDropChance && treasureChestPrefab != null)
        {
            Vector3 dropPosition = transform.position;
            dropPosition.y = 1.0f;
            Instantiate(treasureChestPrefab, dropPosition, Quaternion.identity);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddExperience(expReward);
            GameManager.Instance.AddCoin(coinReward);
        }

        Destroy(gameObject);
    }

    void OnDestroy()
    {
        OnMonsterDestroy?.Invoke();

        // 미니맵 아이콘 삭제
        if (miniMapIconInstance != null)
        {
            Destroy(miniMapIconInstance.gameObject);
        }
    }
}
