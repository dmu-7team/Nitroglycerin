using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [Header("인벤토리 슬롯 UI")]
    public Image[] slots;  // 3칸의 인벤토리 슬롯 (UI Image 배열)
    private Item[] items;   // 실제로 보관되는 아이템 객체

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            items = new Item[slots.Length];  // 슬롯 크기만큼 배열 초기화
            Debug.Log($"인벤토리 초기화 완료. 슬롯 개수: {items.Length}");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool AddItem(Item item)
    {
        Debug.Log($"아이템 추가 요청: {item.itemType}");

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null) // 빈 슬롯 찾기
            {
                items[i] = item;  // 배열에 아이템 등록
                slots[i].sprite = item.icon;
                slots[i].enabled = true;

                Debug.Log($"{item.itemType} 아이템이 인벤토리 슬롯 {i + 1}에 추가되었습니다.");
                Destroy(item.gameObject); // 씬에서 아이템 제거

                return true;
            }
        }
        Debug.Log("인벤토리가 가득 찼습니다.");
        return false;
    }

    public void UseItem(int slotIndex, PlayerStats playerStats)
    {
        if (slotIndex < 0 || slotIndex >= items.Length)
        {
            Debug.LogError("잘못된 슬롯 번호입니다.");
            return;
        }

        if (items[slotIndex] != null)
        {
            Debug.Log($"{items[slotIndex].itemType} 아이템을 사용합니다.");

            // 아이템 효과 적용 (제대로 설정되었는지 확인)
            GameManager.Instance.ApplyItemEffect(
                items[slotIndex].itemType,
                items[slotIndex].effectAmount,
                items[slotIndex].effectDuration,
                playerStats
            );

            // 아이템 사용 후 초기화
            items[slotIndex] = null;
            slots[slotIndex].sprite = null;
            slots[slotIndex].enabled = false;

            Debug.Log($"슬롯 {slotIndex + 1}이 비워졌습니다.");
        }
        else
        {
            Debug.Log($"슬롯 {slotIndex + 1}은 이미 비어 있습니다.");
        }
    }
}
