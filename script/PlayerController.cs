using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject missilePrefab;
    public Transform missileSpawnPoint;
    private PlayerInputActions inputActions;
    private Player_Look playerLook;
    private Player_Move playerMove;

    private bool isNearBox = false;
    private Box currentBox;

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

        inputActions.Player.Interact.performed += ctx =>
        {
            Debug.Log("E 버튼 눌림 - 상자 열기 시도 중...");
            OpenBox();
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

    void OpenBox()
    {
        if (isNearBox && currentBox != null)
        {
            currentBox.Open(this.gameObject); //  올바른 인자 전달
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Box>() != null)
        {
            Debug.Log("상자 범위 안에 들어왔습니다.");
            isNearBox = true;
            currentBox = other.GetComponent<Box>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Box>() != null)
        {
            Debug.Log("상자 범위에서 벗어났습니다.");
            isNearBox = false;
            currentBox = null;
        }
    }
}
