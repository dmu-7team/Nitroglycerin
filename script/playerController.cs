using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject missilePrefab;
    public Transform missileSpawnPoint;
    private PlayerInputActions inputActions;
    private Player_Look playerLook;
    private Player_Move playerMove;

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
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        playerMove.Move();
        playerLook.Look();
    }
    void FireMissile()
    {
        if (missilePrefab == null || missileSpawnPoint == null)
        {
            Debug.LogError("�̻��� ������ �Ǵ� �߻� ��ġ�� �������� �ʾҽ��ϴ�!", this);
            return;
        }

        GameObject missile = Instantiate(missilePrefab, missileSpawnPoint.position, missileSpawnPoint.rotation);
        Debug.Log("�̻��� �߻�: " + missile.transform.position);
    }
}