using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryInput : MonoBehaviour
{
    private PlayerStats playerStats;
    private PlayerInputActions inputActions;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();

        inputActions = new PlayerInputActions();  // 이 부분이 null이면 문제가 발생함
        inputActions.Player.Enable();

        inputActions.Player.UseSlot1.performed += ctx => UseItem(0);
        inputActions.Player.UseSlot2.performed += ctx => UseItem(1);
        inputActions.Player.UseSlot3.performed += ctx => UseItem(2);

        Debug.Log("Input Actions 연결 완료.");
        Debug.Log($"PlayerStats 연결 상태: {(playerStats != null ? "정상 연결" : "null 에러 발생")}");
    }

    private void UseItem(int slotIndex)
    {
        Debug.Log($"슬롯 {slotIndex + 1} 아이템 사용 요청됨.");
        Inventory.instance.UseItem(slotIndex, playerStats);
    }

    private void OnEnable()
    {
        if (inputActions == null)  // 안전 검사 추가
        {
            inputActions = new PlayerInputActions();
            inputActions.Player.Enable();
        }
        else
        {
            inputActions.Player.Enable();
        }
    }

    private void OnDisable()
    {
        if (inputActions != null)  // 안전 검사 추가
        {
            inputActions.Player.Disable();
        }
    }
}
