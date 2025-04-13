using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI speedText;

    private int coinCount = 0;
    private PlayerStatus playerStatus;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerStatus = FindFirstObjectByType<PlayerStatus>();

        if (playerStatus == null)
        {
            Debug.LogError("PlayerStats를 찾을 수 없습니다!");
            return;
        }

        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    public void AddExperience(float exp)
    {
        if (playerStatus != null)
        {
            playerStatus.AddExp(exp);
        }
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
    }

    public void UpdateUI()
    {
        if (playerStatus != null)
        {
            coinText.text = $"코인: {coinCount}";
            expText.text = $"레벨: {playerStatus.level} | 경험치: {playerStatus.currentExp}/{playerStatus.expToLevelUp}";
            healthText.text = $"체력: {playerStatus.currentHealth}/{playerStatus.maxHealth}";
            damageText.text = $"공격력: {playerStatus.attackDamage}";
            speedText.text = $"이동 속도: {playerStatus.moveSpeed}";
        }
    }

    public void ApplyItemEffect(ItemData.ItemType itemType, float effectAmount, float duration, PlayerStatus playerStatus)
    {
        if (playerStatus == null)
        {
            Debug.LogError("PlayerStats가 null입니다.");
            return;
        }

        switch (itemType)
        {
            case ItemData.ItemType.SpeedBoost:
                playerStatus.ApplySpeedBoost(effectAmount, duration);
                break;

            case ItemData.ItemType.DamageBoost:
                playerStatus.ApplyDamageBoost(effectAmount, duration);
                break;
        }

        UpdateUI();
    }
}
