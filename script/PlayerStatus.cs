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

    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
        originalSpeed = moveSpeed;
        originalDamage = attackDamage;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        UIManager.instance.SetHealth(currentHealth, maxHealth);
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("[PlayerStatus] 플레이어 사망!");
        UIManager.instance.ShowItemEffectMessage("사망", 3f);

        // 움직임 및 입력 차단
        var move = GetComponent<Player_Move>();
        if (move != null) move.enabled = false;

        var controller = GetComponent<PlayerController>();
        if (controller != null) controller.enabled = false;

        // 리스폰 타이머 시작
        StartCoroutine(Respawn(5f));
    }

    private IEnumerator Respawn(float delay)
    {
        yield return new WaitForSeconds(delay);

        isDead = false;
        currentHealth = maxHealth;

        // 위치 초기화
        transform.position = Vector3.zero;

        // 다시 이동 가능하게
        var move = GetComponent<Player_Move>();
        if (move != null) move.enabled = true;

        var controller = GetComponent<PlayerController>();
        if (controller != null) controller.enabled = true;

        UIManager.instance.SetHealth(currentHealth, maxHealth);
        Debug.Log("[PlayerStatus] 리스폰 완료");
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

        UIManager.instance.SetHealth(currentHealth, maxHealth);
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
