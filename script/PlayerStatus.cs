using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{
    // ===== 체력 관련 =====
    public int maxHealth = 100;
    public int currentHealth;

    // ===== 기본 스탯 =====
    public float moveSpeed = 5f;
    public float attackDamage = 10f;
    private float originalSpeed;
    private float originalDamage;

    // ===== 레벨 시스템 =====
    public int level = 1;
    public float currentExp = 0f;
    public float expToLevelUp = 100f;
    public float levelUpMultiplier = 1.2f;

    // ===== UI 메시지 =====
    public TextMeshProUGUI levelUpMessageText;
    public TextMeshProUGUI itemEffectText;
    private Coroutine levelUpCoroutine;
    private Coroutine itemEffectCoroutine;

    private void Awake()
    {
        currentHealth = maxHealth;
        originalSpeed = moveSpeed;
        originalDamage = attackDamage;

        // 메시지 기본 숨김
        levelUpMessageText?.gameObject.SetActive(false);
        itemEffectText?.gameObject.SetActive(false);
    }

    // ===== 체력 처리 =====
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("[PlayerStatus] 플레이어 사망 처리");
        // TODO: 게임오버, 리스폰 등
    }

    // ===== 경험치 및 레벨업 =====
    public void AddExp(float exp)
    {
        currentExp += exp;

        while (currentExp >= expToLevelUp)
        {
            currentExp -= expToLevelUp;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        maxHealth += 10;
        currentHealth = maxHealth;
        moveSpeed *= levelUpMultiplier;
        attackDamage *= levelUpMultiplier;
        expToLevelUp *= 1.2f;

        Debug.Log($"[PlayerStatus] 레벨업! 현재 레벨: {level}");

        ShowLevelUpMessage($"레벨업! 레벨 {level}", 2f);
    }

    // ===== 아이템 효과 적용 =====
    public void ApplyItemEffect(ItemData.ItemType itemType, float amount, float duration)
    {
        switch (itemType)
        {
            case ItemData.ItemType.SpeedBoost:
                ApplySpeedBoost(amount, duration);
                ShowItemEffectText("스피드 증가!", duration);
                break;

            case ItemData.ItemType.DamageBoost:
                ApplyDamageBoost(amount, duration);
                ShowItemEffectText("공격력 증가!", duration);
                break;
        }
    }

    public void ApplySpeedBoost(float amount, float duration)
    {
        StartCoroutine(SpeedBoost(amount, duration));
    }

    public void ApplyDamageBoost(float amount, float duration)
    {
        StartCoroutine(DamageBoost(amount, duration));
    }

    private IEnumerator SpeedBoost(float amount, float duration)
    {
        moveSpeed = originalSpeed * amount;
        Debug.Log($"[SpeedBoost] 이동 속도 증가: {moveSpeed}");
        yield return new WaitForSeconds(duration);
        moveSpeed = originalSpeed;
        Debug.Log("[SpeedBoost] 이동 속도 복원");
    }

    private IEnumerator DamageBoost(float amount, float duration)
    {
        attackDamage = originalDamage * amount;
        Debug.Log($"[DamageBoost] 공격력 증가: {attackDamage}");
        yield return new WaitForSeconds(duration);
        attackDamage = originalDamage;
        Debug.Log("[DamageBoost] 공격력 복원");
    }

    // ===== 메시지 출력 =====
    public void ShowLevelUpMessage(string message, float duration = 2f)
    {
        if (levelUpMessageText == null) return;

        if (levelUpCoroutine != null)
            StopCoroutine(levelUpCoroutine);

        levelUpCoroutine = StartCoroutine(ShowMessageCoroutine(levelUpMessageText, message, duration));
    }

    public void ShowItemEffectText(string message, float duration = 2f)
    {
        if (itemEffectText == null) return;

        if (itemEffectCoroutine != null)
            StopCoroutine(itemEffectCoroutine);

        itemEffectCoroutine = StartCoroutine(ShowMessageCoroutine(itemEffectText, message, duration));
    }

    private IEnumerator ShowMessageCoroutine(TextMeshProUGUI target, string message, float duration)
    {
        target.text = message;
        target.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        target.gameObject.SetActive(false);
    }
}
