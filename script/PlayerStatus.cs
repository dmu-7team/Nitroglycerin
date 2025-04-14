using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public float moveSpeed = 5f;
    public float attackDamage = 10f;

    private float originalSpeed;
    private float originalDamage;

    public int level = 1;
    public float currentExp = 0f;
    public float expToLevelUp = 100f;
    public float levelUpMultiplier = 1.2f;

    private void Awake()
    {
        currentHealth = maxHealth;
        originalSpeed = moveSpeed;
        originalDamage = attackDamage;
    }

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
    }

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
        UIManager.instance.ShowLevelUpMessage($"레벨업! 레벨 {level}", 2f);
    }

    public void ApplyItemEffect(ItemData.ItemType itemType, float amount, float duration)
    {
        switch (itemType)
        {
            case ItemData.ItemType.SpeedBoost:
                ApplySpeedBoost(amount, duration);
                UIManager.instance.ShowItemEffectMessage("스피드 증가!", duration);
                break;
            case ItemData.ItemType.DamageBoost:
                ApplyDamageBoost(amount, duration);
                UIManager.instance.ShowItemEffectMessage("공격력 증가!", duration);
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
        yield return new WaitForSeconds(duration);
        moveSpeed = originalSpeed;
    }

    private IEnumerator DamageBoost(float amount, float duration)
    {
        attackDamage = originalDamage * amount;
        yield return new WaitForSeconds(duration);
        attackDamage = originalDamage;
    }
}
