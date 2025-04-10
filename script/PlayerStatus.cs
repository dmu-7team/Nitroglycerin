using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{
    public float currentExp = 0f;
    public float expToLevelUp = 100f;
    public int level = 1;

    public float maxHealth = 100f;
    public float currentHealth;
    public float attackDamage = 20f;
    public float moveSpeed = 5f;

    public float healthIncreasePerLevel = 10f;
    public float damageIncreasePerLevel = 5f;
    public float speedIncreasePerLevel = 0.5f;

    private float originalSpeed;
    private float originalDamage;

    private void Start()
    {
        currentHealth = maxHealth;
        originalSpeed = moveSpeed;
        originalDamage = attackDamage;
    }

    public void AddExp(float exp)
    {
        currentExp += exp;

        while (currentExp >= expToLevelUp)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentExp -= expToLevelUp;
        level++;
        expToLevelUp *= 1.5f;

        maxHealth += healthIncreasePerLevel;
        currentHealth = maxHealth;
        attackDamage += damageIncreasePerLevel;
        moveSpeed += speedIncreasePerLevel;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ShowLevelUpMessage();
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
        Debug.Log("플레이어 사망");
    }

    public void ApplySpeedBoost(float amount, float duration)
    {
        StopCoroutine("SpeedBoostCoroutine");
        StartCoroutine(SpeedBoostCoroutine(amount, duration));
    }

    public void ApplyDamageBoost(float amount, float duration)
    {
        StopCoroutine("DamageBoostCoroutine");
        StartCoroutine(DamageBoostCoroutine(amount, duration));
    }

    private IEnumerator SpeedBoostCoroutine(float amount, float duration)
    {
        moveSpeed = originalSpeed * amount;
        Debug.Log($"SpeedBoost 활성화: 현재 속도 = {moveSpeed}");

        yield return new WaitForSeconds(duration);

        moveSpeed = originalSpeed;
        Debug.Log("SpeedBoost 종료: 속도가 원래대로 돌아왔습니다.");
    }

    private IEnumerator DamageBoostCoroutine(float amount, float duration)
    {
        attackDamage = originalDamage * amount;
        Debug.Log($"DamageBoost 활성화: 현재 공격력 = {attackDamage}");

        yield return new WaitForSeconds(duration);

        attackDamage = originalDamage;
        Debug.Log("DamageBoost 종료: 공격력이 원래대로 돌아왔습니다.");
    }
}
