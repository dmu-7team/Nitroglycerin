using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // UI 요소
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI levelUpMessageText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI itemEffectText;

    private int coinCount = 0;
    private PlayerStats playerStats;

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
        playerStats = FindObjectOfType<PlayerStats>();

        if (playerStats == null)
        {
            Debug.LogError("PlayerStats를 찾을 수 없습니다! Player 오브젝트에 PlayerStats 컴포넌트가 있는지 확인하세요.");
            return;
        }

        UpdateUI();
    }
    public void AddExperience(float exp)
    {
        if (playerStats != null)
        {
            playerStats.AddExp(exp);  // PlayerStats에서 AddExp 메서드를 호출
        }
        else
        {
            Debug.LogError("PlayerStats가 null입니다.");
        }
    }
    private void Update()
    {
        UpdateUI();
    }

    public void ApplyItemEffect(Item.ItemType itemType, float effectAmount, float duration, PlayerStats playerStats)
    {
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats가 null입니다.");
            return;
        }

        switch (itemType)
        {
            case Item.ItemType.SpeedBoost:
                playerStats.ApplySpeedBoost(effectAmount, duration);
                Debug.Log($"속도 증가 효과 적용: 배율 {effectAmount}, 지속 시간 {duration}");
                break;

            case Item.ItemType.DamageBoost:
                playerStats.ApplyDamageBoost(effectAmount, duration);
                Debug.Log($"데미지 증가 효과 적용: 배율 {effectAmount}, 지속 시간 {duration}");
                break;

            default:
                Debug.LogWarning("정의되지 않은 아이템 타입입니다.");
                break;
        }
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (playerStats != null)
        {
            coinText.text = $"코인: {coinCount}";
            expText.text = $"레벨: {playerStats.level} | 경험치: {playerStats.currentExp}/{playerStats.expToLevelUp}";
            healthText.text = $"체력: {playerStats.currentHealth}/{playerStats.maxHealth}";
            damageText.text = $"공격력: {playerStats.attackDamage}";
            speedText.text = $"이동 속도: {playerStats.moveSpeed}";
        }
    }

    public void ShowLevelUpMessage()
    {
        if (levelUpMessageText != null)
        {
            levelUpMessageText.text = $"레벨 업! 레벨 {playerStats.level}";
            levelUpMessageText.gameObject.SetActive(true);
            StartCoroutine(HideLevelUpMessage());
        }
    }

    private IEnumerator HideLevelUpMessage()
    {
        yield return new WaitForSeconds(2f);
        levelUpMessageText.gameObject.SetActive(false);
    }
}
