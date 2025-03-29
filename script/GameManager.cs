using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // UI ���
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
            Debug.LogError("PlayerStats�� ã�� �� �����ϴ�! player ������Ʈ�� PlayerStats ������Ʈ�� �ִ��� Ȯ���ϼ���.", this);
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
            Debug.LogError("CoinMessageText�� ������� �ʾҽ��ϴ�!", this);
        }

        if (levelUpMessageText != null)
        {
            levelUpMessageText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("LevelUpMessageText�� ������� �ʾҽ��ϴ�!", this);
        }

        if (itemEffectText != null)
        {
            itemEffectText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("ItemEffectText�� ������� �ʾҽ��ϴ�!", this);
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
            levelUpMessageText.text = $"Level Up! Level {playerStats.level}"; // "������! ����" -> "Level Up! Level"
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
            coinText.text = $"Coins: {coinCount}"; // "����" -> "Coins"
        }
        else
        {
            Debug.LogError("CoinText�� ������� �ʾҽ��ϴ�!", this);
        }
    }

    void UpdateExpText()
    {
        if (expText != null && playerStats != null)
        {
            expText.text = $"Level: {playerStats.level} EXP: {playerStats.currentExp}/{playerStats.expToLevelUp}"; // "����", "����ġ" -> "Level", "EXP"
            Debug.Log($"ExpText ������Ʈ: {expText.text}");
        }
        else
        {
            Debug.LogError("ExpText �Ǵ� PlayerStats�� ������� �ʾҽ��ϴ�!", this);
        }
    }

    void UpdateStatsText()
    {
        if (playerStats != null)
        {
            if (healthText != null)
                healthText.text = $"Health: {playerStats.currentHealth}/{playerStats.maxHealth}"; // "ü��" -> "Health"
            if (damageText != null)
                damageText.text = $"Damage: {playerStats.attackDamage * damageBoostMultiplier}"; // "���ݷ�" -> "Damage"
            if (speedText != null)
                speedText.text = $"Speed: {playerStats.moveSpeed * speedBoostMultiplier}"; // "�̵� �ӵ�" -> "Speed"
        }
    }

    void UpdateItemEffectText()
    {
        if (itemEffectText != null)
        {
            string effectText = "";
            if (speedBoostTimer > 0)
                effectText += $"Speed Boost: {speedBoostTimer:F1}s\n"; // "�ӵ� ����" -> "Speed Boost"
            if (damageBoostTimer > 0)
                effectText += $"Damage Boost: {damageBoostTimer:F1}s"; // "������ ����" -> "Damage Boost"

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
            coinMessageText.text = $"Coins +{amount}"; // "����" -> "Coins"
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