using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        SpeedBoost,
        DamageBoost
    }

    public ItemType itemType;
    public float effectAmount = 2f; // ȿ�� ������ (�ӵ�: 2��, ������: 2��)
    public float effectDuration = 10f; // ȿ�� ���� �ð� (��)

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                GameManager.Instance.ApplyItemEffect(itemType, effectAmount, effectDuration);
                Destroy(gameObject);
                Debug.Log($"{itemType} ������ ȹ��!");
            }
        }
    }
}