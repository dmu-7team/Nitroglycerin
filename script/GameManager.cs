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
            Debug.LogError("PlayerStats를 찾을 수 없습니다! Player 오브젝트에 PlayerStats 컴포넌트가 있는지 확인하세요.");
            return;
        }

        UpdateUI();
    }

    // 내부에 이 두 public 함수가 있어야 함
 // PlayerStatus.cs 안에 아래 두 개 함수 추가!


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

    public void ShowLevelUpMessage()
    {
        if (levelUpMessageText != null)
        {
            levelUpMessageText.text = $"레벨 업! 레벨 {playerStatus.level}";
            levelUpMessageText.gameObject.SetActive(true);
            StartCoroutine(HideLevelUpMessage());
        }
    }

    private IEnumerator HideLevelUpMessage()
    {
        yield return new WaitForSeconds(2f);
        levelUpMessageText.gameObject.SetActive(false);
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
                Debug.Log($"[GameManager] SpeedBoost 적용됨: x{effectAmount}, {duration}초");
                break;

            case ItemData.ItemType.DamageBoost:
                playerStatus.ApplyDamageBoost(effectAmount, duration);
                Debug.Log($"[GameManager] DamageBoost 적용됨: x{effectAmount}, {duration}초");
                break;
        }

        UpdateUI();
    }


}
