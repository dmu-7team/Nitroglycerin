using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject missilePrefab;
    public Transform missileSpawnPoint;
    private PlayerInputActions inputActions;
    private Player_Look playerLook;
    private Player_Move playerMove;

    private bool isNearTreasureChest = false;
    private TreasureChest currentChest;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        playerLook = GetComponent<Player_Look>();
        playerMove = GetComponent<Player_Move>();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ctx => playerMove.SetMoveInput(ctx.ReadValue<Vector2>());
        inputActions.Player.Move.canceled += ctx => playerMove.SetMoveInput(Vector2.zero);
        inputActions.Player.Look.performed += ctx => playerLook.SetLookInput(ctx.ReadValue<Vector2>());
        inputActions.Player.Look.canceled += ctx => playerLook.SetLookInput(Vector2.zero);

        inputActions.Player.Attack.performed += ctx => AttackMissile();

        // Interact 버튼 (E) 수정 및 디버그 메시지 추가
        inputActions.Player.Interact.performed += ctx =>
        {
            Debug.Log("E 버튼 눌림 - 보물상자를 여는 시도 중...");
            OpenTreasureChest();
        };
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Update()
    {
        playerMove.Move();
        playerLook.Look();
    }

    void AttackMissile()
    {
        if (missilePrefab == null || missileSpawnPoint == null)
        {
            Debug.LogError("미사일 프리팹 또는 발사 위치가 설정되지 않았습니다!", this);
            return;
        }

        GameObject missile = Instantiate(missilePrefab, missileSpawnPoint.position, missileSpawnPoint.rotation);
        Debug.Log("미사일 발사: " + missile.transform.position);
    }

    void OpenTreasureChest()
    {
        if (isNearTreasureChest && currentChest != null)
        {
            Debug.Log("보물상자를 열었습니다.");
            currentChest.OpenChest();
        }
        else
        {
            Debug.Log("보물상자가 근처에 없습니다.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TreasureChest"))
        {
            Debug.Log("보물상자 범위 안에 들어왔습니다.");
            isNearTreasureChest = true;
            currentChest = other.GetComponent<TreasureChest>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TreasureChest"))
        {
            Debug.Log("보물상자 범위에서 벗어났습니다.");
            isNearTreasureChest = false;
            currentChest = null;
        }
    }

}
