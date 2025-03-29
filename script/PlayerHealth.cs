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
            Debug.Log("ü�� �� �ʱ�ȭ: " + currentHealth + "/" + maxHealth);
        }
        else
        {
            Debug.LogError("ü�� �ٰ� ������� �ʾҽ��ϴ�!", this);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        Debug.Log("�÷��̾� ü�� ����: " + damage + ", ���� ü��: " + currentHealth + "/" + maxHealth);

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
            Debug.Log("ü�� �� ������Ʈ: " + healthBar.value);
        }
        else
        {
            Debug.LogError("ü�� �ٰ� ������� �ʾҽ��ϴ�! ü�� ������Ʈ ����", this);
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
        Debug.Log("�÷��̾� ü�� ȸ��: " + amount + ", ���� ü��: " + currentHealth + "/" + maxHealth);

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
            Debug.Log("ü�� �� ������Ʈ: " + healthBar.value);
        }
    }

    void Die()
    {
        Debug.Log("�÷��̾ ����߽��ϴ�!");
    }
}