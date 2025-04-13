using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [Header("슬롯 설정")]
    public Image[] slots;
    private ItemData[] items;

    [Header("상자 메시지")]
    public TextMeshProUGUI chestMessageText;
    private Coroutine chestMessageCoroutine;

    private PlayerStatus playerStatus;
    private PlayerInputActions inputActions;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        Debug.Log("[Inventory] 이 컴포넌트가 붙어 있는 오브젝트 이름: " + gameObject.name);

        playerStatus = GetComponent<PlayerStatus>();
        if (playerStatus == null)
            Debug.LogError("[Inventory] PlayerStatus 연결 실패! Player 오브젝트에 붙어 있어야 합니다.");
    }

    private void Start()
    {
        items = new ItemData[slots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
                slots[i].enabled = false;
        }

        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();

        inputActions.Player.UseSlot1.performed += ctx => UseItem(0);
        inputActions.Player.UseSlot2.performed += ctx => UseItem(1);
        inputActions.Player.UseSlot3.performed += ctx => UseItem(2);
    }

    public bool AddItem(ItemData itemData)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = itemData;

                if (slots[i] != null)
                {
                    slots[i].sprite = itemData.icon;
                    slots[i].enabled = true;
                }

                ShowChestMessage("상자를 열었습니다!"); // 메시지 표시

                return true;
            }
        }

        Debug.Log("인벤토리가 가득 찼습니다.");
        return false;
    }

    private void UseItem(int index)
    {
        if (playerStatus == null) return;
        if (index < 0 || index >= items.Length || items[index] == null) return;

        var item = items[index];

        playerStatus.ApplyItemEffect(item.itemType, item.effectAmount, item.effectDuration);

        items[index] = null;
        slots[index].sprite = null;
        slots[index].enabled = false;
    }

    private void OnEnable() => inputActions?.Player.Enable();
    private void OnDisable() => inputActions?.Player.Disable();

    // 메시지 표시 함수
    private void ShowChestMessage(string message, float duration = 2f)
    {
        if (chestMessageText == null) return;

        if (chestMessageCoroutine != null)
            StopCoroutine(chestMessageCoroutine);

        chestMessageCoroutine = StartCoroutine(ShowChestMessageCoroutine(message, duration));
    }

    private IEnumerator ShowChestMessageCoroutine(string message, float duration)
    {
        chestMessageText.text = message;
        chestMessageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        chestMessageText.gameObject.SetActive(false);
    }
}
