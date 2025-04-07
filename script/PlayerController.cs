using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

        // 수정된 부분: 입력 액션 이름을 Attack으로 변경
        inputActions.Player.Attack.performed += ctx => AttackMissile();
        inputActions.Player.Interact.performed += ctx => OpenTreasureChest();
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

    void AttackMissile()
    {
        if (missilePrefab == null || missileSpawnPoint == null)
        {
            Debug.LogError("미사일 프리팹 또는 발사 위치가 설정되지 않았습니다!", this);
            return;
        }

        GameObject missile = Instantiate(missilePrefab, missileSpawnPoint.position, missileSpawnPoint.rotation);
        Debug.Log("미사일 발사 (Attack): " + missile.transform.position);
    }

    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void OpenTreasureChest()
    {
        if (isNearTreasureChest && currentChest != null)
        {
            currentChest.OpenChest();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TreasureChest"))
        {
            isNearTreasureChest = true;
            currentChest = other.GetComponent<TreasureChest>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TreasureChest"))
        {
            isNearTreasureChest = false;
            currentChest = null;
        }
    }
}
