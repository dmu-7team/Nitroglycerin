using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float currentExp = 0f;
    public float expToLevelUp = 100f;
    public int level = 1;

    // �÷��̾� ����
    public float maxHealth = 100f;
    public float currentHealth;
    public float attackDamage = 20f;
    public float moveSpeed = 5f;

    // ������ �� ������
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
        Debug.Log($"����ġ ȹ��: {exp}, ���� ����ġ: {currentExp}/{expToLevelUp}, ����: {level}");

        while (currentExp >= expToLevelUp)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentExp -= expToLevelUp;
        level++;
        expToLevelUp *= 1.5f; // ������ �� �ʿ� ����ġ 1.5�� ����

        // ���� ����
        maxHealth += healthIncreasePerLevel;
        currentHealth = maxHealth; // ü�� ȸ��
        attackDamage += damageIncreasePerLevel;
        moveSpeed += speedIncreasePerLevel;

        Debug.Log($"������! ���� ����: {level}, ü��: {maxHealth}, ���ݷ�: {attackDamage}, �̵� �ӵ�: {moveSpeed}");

        // GameManager�� ���� ������ �˸� ǥ��
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ShowLevelUpMessage();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"�÷��̾� ü��: {currentHealth}/{maxHealth}");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("�÷��̾ ����߽��ϴ�!");
        // ���� ���� ���� �߰� (��: ���� ���� ȭ�� ǥ��)
    }
}