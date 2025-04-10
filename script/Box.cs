using UnityEngine;

public class Box : MonoBehaviour
{
    public enum ItemType { SpeedBoost, DamageBoost }

    [System.Serializable]
    public class ItemData
    {
        public ItemType itemType;
        public Sprite icon;
        public float effectAmount = 1.5f;
        public float effectDuration = 5f;
    }

    public ItemData[] itemOptions;
    public float interactionDistance = 3f;
    public GameObject visualEffect;

    private bool isOpened = false;

    public void Open(GameObject player)
    {
        if (isOpened) return;
        isOpened = true;

        if (visualEffect != null)
            visualEffect.SetActive(true);

        GiveItemToInventory(player);

        Debug.Log("[Box] 상자 열림, 오브젝트 제거됨");
        Destroy(gameObject); //  상자 오브젝트 제거
    }

    public void GiveItemToInventory(GameObject player)
    {
        if (itemOptions.Length == 0) return;

        int randomIndex = Random.Range(0, itemOptions.Length);
        ItemData selectedItem = itemOptions[randomIndex];

        Inventory inventory = player.GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("[Box] 플레이어에게 Inventory 컴포넌트가 없습니다!");
            return;
        }

        bool added = inventory.AddItem(
            selectedItem.itemType,
            selectedItem.icon,
            selectedItem.effectAmount,
            selectedItem.effectDuration
        );

        if (added)
        {
            Debug.Log($"[Box] {selectedItem.itemType} 아이템 인벤토리 추가 성공");
        }
        else
        {
            Debug.Log("[Box] 인벤토리에 아이템 추가 실패");
        }
    }
}
