using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        SpeedBoost,
        DamageBoost
    }

    public ItemType itemType;
    public float effectAmount = 2f;
    public float effectDuration = 10f;
    public Sprite icon;  // 아이템 아이콘 이미지 추가

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory inventory = Inventory.instance;
            if (inventory != null)
            {
                if (inventory.AddItem(this))
                {
                    Debug.Log($"{itemType} 아이템을 인벤토리에 추가했습니다.");
                }
            }
        }
    }
}
