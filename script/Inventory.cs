using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [Serializable]
    public class ItemSlot
    {
        public Box.ItemType itemType;
        public Sprite icon;
        public float effectAmount;
        public float effectDuration;

        public ItemSlot(Box.ItemType type, Sprite icon, float amount, float duration)
        {
            this.itemType = type;
            this.icon = icon;
            this.effectAmount = amount;
            this.effectDuration = duration;
        }
    }

    public Image[] slots;
    private ItemSlot[] items;
    private PlayerStatus playerStatus;
    private PlayerInputActions inputActions;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        playerStatus = GetComponent<PlayerStatus>();  // 같은 오브젝트에서 찾기

        if (playerStatus == null)
        {
            // `playerStatus`가 여전히 null이라면, 다른 오브젝트에서 찾아서 연결
            playerStatus = GameObject.Find("player").GetComponent<PlayerStatus>(); // "player" 오브젝트에서 찾기
        }

        if (playerStatus == null)
        {
            Debug.LogError("[Inventory] PlayerStatus가 연결되지 않았습니다! Player 오브젝트에 PlayerStatus 컴포넌트를 붙여주세요.");
        }
    }

    private void Start()
    {
        items = new ItemSlot[slots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
                slots[i].enabled = false;
            else
                Debug.LogError($"[Inventory-Start] slots[{i}]가 null입니다.");
        }

        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();

        inputActions.Player.UseSlot1.performed += ctx => UseItem(0);
        inputActions.Player.UseSlot2.performed += ctx => UseItem(1);
        inputActions.Player.UseSlot3.performed += ctx => UseItem(2);
    }


    public bool AddItem(Box.ItemType itemType, Sprite icon, float amount, float duration)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = new ItemSlot(itemType, icon, amount, duration);

                if (slots[i] == null)
                {
                    Debug.LogError($"[Inventory] slots[{i}]가 null입니다. UI 연결 안 됐는지 확인해주세요.");
                }
                else
                {
                    slots[i].sprite = icon;
                    slots[i].enabled = true;
                }

                return true;
            }
        }

        Debug.Log("인벤토리가 가득 찼습니다.");
        return false;
    }

    private void UseItem(int slotIndex)
    {
        if (playerStatus == null)
        {
            Debug.LogError("[Inventory] playerStatus가 연결되지 않았습니다!");
            return;
        }

        if (slotIndex < 0 || slotIndex >= items.Length || items[slotIndex] == null) return;

        var item = items[slotIndex];

        GameManager.Instance.ApplyItemEffect(item.itemType, item.effectAmount, item.effectDuration, playerStatus);

        items[slotIndex] = null;
        slots[slotIndex].sprite = null;
        slots[slotIndex].enabled = false;

        GameManager.Instance.UpdateUI();
    }

    private void OnEnable()
    {
        inputActions?.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions?.Player.Disable();
    }
}