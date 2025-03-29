using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
            Debug.Log("체력 바 초기화: " + currentHealth + "/" + maxHealth);
        }
        else
        {
            Debug.LogError("체력 바가 연결되지 않았습니다!", this);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        Debug.Log("플레이어 체력 감소: " + damage + ", 현재 체력: " + currentHealth + "/" + maxHealth);

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
            Debug.Log("체력 바 업데이트: " + healthBar.value);
        }
        else
        {
            Debug.LogError("체력 바가 연결되지 않았습니다! 체력 업데이트 실패", this);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void RestoreHealth(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        Debug.Log("플레이어 체력 회복: " + amount + ", 현재 체력: " + currentHealth + "/" + maxHealth);

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
            Debug.Log("체력 바 업데이트: " + healthBar.value);
        }
    }

    void Die()
    {
        Debug.Log("플레이어가 사망했습니다!");
    }
}