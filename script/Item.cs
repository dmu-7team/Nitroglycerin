using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        SpeedBoost,
        DamageBoost
    }

    public ItemType itemType;
    public float effectAmount = 2f; // 효과 증가량 (속도: 2배, 데미지: 2배)
    public float effectDuration = 10f; // 효과 지속 시간 (초)

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                GameManager.Instance.ApplyItemEffect(itemType, effectAmount, effectDuration);
                Destroy(gameObject);
                Debug.Log($"{itemType} 아이템 획득!");
            }
        }
    }
}