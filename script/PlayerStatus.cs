using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float currentExp = 0f;
    public float expToLevelUp = 100f;
    public int level = 1;

    // 플레이어 스탯
    public float maxHealth = 100f;
    public float currentHealth;
    public float attackDamage = 20f;
    public float moveSpeed = 5f;

    // 레벨업 시 증가량
    public float healthIncreasePerLevel = 10f;
    public float damageIncreasePerLevel = 5f;
    public float speedIncreasePerLevel = 0.5f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void AddExp(float exp)
    {
        currentExp += exp;
        Debug.Log($"경험치 획득: {exp}, 현재 경험치: {currentExp}/{expToLevelUp}, 레벨: {level}");

        while (currentExp >= expToLevelUp)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentExp -= expToLevelUp;
        level++;
        expToLevelUp *= 1.5f; // 레벨업 시 필요 경험치 1.5배 증가

        // 스탯 증가
        maxHealth += healthIncreasePerLevel;
        currentHealth = maxHealth; // 체력 회복
        attackDamage += damageIncreasePerLevel;
        moveSpeed += speedIncreasePerLevel;

        Debug.Log($"레벨업! 현재 레벨: {level}, 체력: {maxHealth}, 공격력: {attackDamage}, 이동 속도: {moveSpeed}");

        // GameManager를 통해 레벨업 알림 표시
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ShowLevelUpMessage();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"플레이어 체력: {currentHealth}/{maxHealth}");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("플레이어가 사망했습니다!");
        // 게임 오버 로직 추가 (예: 게임 오버 화면 표시)
    }
}