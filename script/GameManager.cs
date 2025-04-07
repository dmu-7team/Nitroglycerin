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
    public TextMeshProUGUI chestMessageText;
    public float messageDisplayTime = 1f;

    // 폰트와 머티리얼 직접 할당
    public TMP_FontAsset notoSansKRFont; // Inspector에서 설정
    public Material notoSansKRMaterial;  // Inspector에서 설정

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

        // 폰트와 머티리얼을 Resources 폴더에서 로드
        notoSansKRFont = Resources.Load<TMP_FontAsset>("Fonts/NotoSansKR-Bold SDF");
        notoSansKRMaterial = notoSansKRFont.material;

        if (notoSansKRFont == null || notoSansKRMaterial == null)
        {
            Debug.LogError("NotoSansKR-Bold 폰트나 머티리얼이 Resources 폴더에서 로드되지 않았습니다.");
            return;
        }

        // 폰트 설정 확인 및 적용
        ApplyFontToText(coinText, "CoinText");
        ApplyFontToText(coinMessageText, "CoinMessageText");
        ApplyFontToText(expText, "ExpText");
        ApplyFontToText(levelUpMessageText, "LevelUpMessageText");
        ApplyFontToText(healthText, "HealthText");
        ApplyFontToText(damageText, "DamageText");
        ApplyFontToText(speedText, "SpeedText");
        ApplyFontToText(itemEffectText, "ItemEffectText");
        ApplyFontToText(chestMessageText, "ChestMessageText");

        UpdateCoinText();
        UpdateExpText();
        UpdateStatsText();
    }


    void ApplyFontToText(TextMeshProUGUI text, string textName)
    {
        if (text != null)
        {
            if (text.font == null || text.font.name != "NotoSansKR SDF")
            {
                if (notoSansKRFont != null && notoSansKRMaterial != null)
                {
                    text.font = notoSansKRFont;
                    text.fontMaterial = notoSansKRMaterial;
                    Debug.Log($"{textName}의 폰트를 NotoSansKR SDF로 설정했습니다.");
                }
                else
                {
                    Debug.LogError($"{textName}에 적용할 NotoSansKR SDF 폰트 또는 머티리얼이 Inspector에서 설정되지 않았습니다!", text);
                }
            }
        }
        else
        {
            Debug.LogError($"{textName}가 연결되지 않았습니다!", this);
        }
    }

    public void ShowChestMessage()
    {
        if (chestMessageText != null)
        {
            chestMessageText.text = "보물상자를 열었습니다!";
            chestMessageText.gameObject.SetActive(true);
            StartCoroutine(FadeMessage(chestMessageText));
            Debug.Log("ChestMessage 표시: 보물상자를 열었습니다!");
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
            levelUpMessageText.text = $"레벨업! 레벨 {playerStats.level}";
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
            coinText.text = $"코인: {coinCount}";
            Debug.Log($"CoinText 설정: {coinText.text}");
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
            expText.text = $"레벨: {playerStats.level} 경험치: {playerStats.currentExp}/{playerStats.expToLevelUp}";
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
                healthText.text = $"체력: {playerStats.currentHealth}/{playerStats.maxHealth}";
            if (damageText != null)
                damageText.text = $"공격력: {playerStats.attackDamage * damageBoostMultiplier}";
            if (speedText != null)
                speedText.text = $"이동 속도: {playerStats.moveSpeed * speedBoostMultiplier}";
        }
    }

    void UpdateItemEffectText()
    {
        if (itemEffectText != null)
        {
            string effectText = "";
            if (speedBoostTimer > 0)
                effectText += $"속도 증가: {speedBoostTimer:F1}초\n";
            if (damageBoostTimer > 0)
                effectText += $"데미지 증가: {damageBoostTimer:F1}초";

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
            coinMessageText.text = $"코인 +{amount}";
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