using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // UI 요소
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI coinMessageText;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI levelUpMessageText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI itemEffectText;

    public float messageDisplayTime = 1f;

    private int coinCount = 0;
    private PlayerStats playerStats;
    private float speedBoostMultiplier = 1f;
    private float damageBoostMultiplier = 1f;
    private float speedBoostTimer = 0f;
    private float damageBoostTimer = 0f;

    void Awake()
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

    void Start()
    {
        playerStats = FindFirstObjectByType<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats를 찾을 수 없습니다! player 오브젝트에 PlayerStats 컴포넌트가 있는지 확인하세요.", this);
        }

        UpdateCoinText();
        UpdateExpText();
        UpdateStatsText();

        if (coinMessageText != null)
        {
            coinMessageText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("CoinMessageText가 연결되지 않았습니다!", this);
        }

        if (levelUpMessageText != null)
        {
            levelUpMessageText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("LevelUpMessageText가 연결되지 않았습니다!", this);
        }

        if (itemEffectText != null)
        {
            itemEffectText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("ItemEffectText가 연결되지 않았습니다!", this);
        }
    }

    void Update()
    {
        if (speedBoostTimer > 0)
        {
            speedBoostTimer -= Time.deltaTime;
            if (speedBoostTimer <= 0)
            {
                speedBoostMultiplier = 1f;
                UpdateStatsText();
                UpdateItemEffectText();
            }
        }

        if (damageBoostTimer > 0)
        {
            damageBoostTimer -= Time.deltaTime;
            if (damageBoostTimer <= 0)
            {
                damageBoostMultiplier = 1f;
                UpdateStatsText();
                UpdateItemEffectText();
            }
        }
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateCoinText();
        ShowCoinMessage(amount);
    }

    public void AddExp(float exp)
    {
        if (playerStats != null)
        {
            playerStats.AddExp(exp);
            UpdateExpText();
            UpdateStatsText();
        }
    }

    public void ShowLevelUpMessage()
    {
        if (levelUpMessageText != null)
        {
            levelUpMessageText.text = $"Level Up! Level {playerStats.level}"; // "레벨업! 레벨" -> "Level Up! Level"
            levelUpMessageText.gameObject.SetActive(true);
            StartCoroutine(FadeMessage(levelUpMessageText));
            UpdateStatsText();
        }
    }

    public void ApplyItemEffect(Item.ItemType itemType, float effectAmount, float duration)
    {
        switch (itemType)
        {
            case Item.ItemType.SpeedBoost:
                speedBoostMultiplier = effectAmount;
                speedBoostTimer = duration;
                break;
            case Item.ItemType.DamageBoost:
                damageBoostMultiplier = effectAmount;
                damageBoostTimer = duration;
                break;
        }
        UpdateStatsText();
        UpdateItemEffectText();
    }

    public float GetSpeedBoostMultiplier()
    {
        return speedBoostMultiplier;
    }

    public float GetDamageBoostMultiplier()
    {
        return damageBoostMultiplier;
    }

    void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = $"Coins: {coinCount}"; // "코인" -> "Coins"
        }
        else
        {
            Debug.LogError("CoinText가 연결되지 않았습니다!", this);
        }
    }

    void UpdateExpText()
    {
        if (expText != null && playerStats != null)
        {
            expText.text = $"Level: {playerStats.level} EXP: {playerStats.currentExp}/{playerStats.expToLevelUp}"; // "레벨", "경험치" -> "Level", "EXP"
            Debug.Log($"ExpText 업데이트: {expText.text}");
        }
        else
        {
            Debug.LogError("ExpText 또는 PlayerStats가 연결되지 않았습니다!", this);
        }
    }

    void UpdateStatsText()
    {
        if (playerStats != null)
        {
            if (healthText != null)
                healthText.text = $"Health: {playerStats.currentHealth}/{playerStats.maxHealth}"; // "체력" -> "Health"
            if (damageText != null)
                damageText.text = $"Damage: {playerStats.attackDamage * damageBoostMultiplier}"; // "공격력" -> "Damage"
            if (speedText != null)
                speedText.text = $"Speed: {playerStats.moveSpeed * speedBoostMultiplier}"; // "이동 속도" -> "Speed"
        }
    }

    void UpdateItemEffectText()
    {
        if (itemEffectText != null)
        {
            string effectText = "";
            if (speedBoostTimer > 0)
                effectText += $"Speed Boost: {speedBoostTimer:F1}s\n"; // "속도 증가" -> "Speed Boost"
            if (damageBoostTimer > 0)
                effectText += $"Damage Boost: {damageBoostTimer:F1}s"; // "데미지 증가" -> "Damage Boost"

            if (!string.IsNullOrEmpty(effectText))
            {
                itemEffectText.text = effectText;
                itemEffectText.gameObject.SetActive(true);
            }
            else
            {
                itemEffectText.gameObject.SetActive(false);
            }
        }
    }

    void ShowCoinMessage(int amount)
    {
        if (coinMessageText != null)
        {
            coinMessageText.text = $"Coins +{amount}"; // "코인" -> "Coins"
            coinMessageText.gameObject.SetActive(true);
            StartCoroutine(FadeMessage(coinMessageText));
        }
    }

    IEnumerator FadeMessage(TextMeshProUGUI messageText)
    {
        var canvasGroup = messageText.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = messageText.gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        float duration = messageDisplayTime / 2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            yield return null;
        }

        yield return new WaitForSeconds(duration);

        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            yield return null;
        }

        messageText.gameObject.SetActive(false);
    }
}