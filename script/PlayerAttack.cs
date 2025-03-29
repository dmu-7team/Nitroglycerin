using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public float attackCooldown = 0.5f;
    public Camera playerCamera;
    public GameObject missilePrefab;
    public Transform missileSpawnPoint;

    private float lastAttackTime;
    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Attack.performed += ctx => TryAttack();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
        {
            return;
        }

        lastAttackTime = Time.time;

        if (playerCamera == null || missilePrefab == null)
        {
            Debug.LogError("playerCamera 또는 missilePrefab이 설정되지 않았습니다!", this);
            return;
        }

        // 미사일 발사 위치: 몬스터와 같은 높이로 조정
        Vector3 spawnPosition = missileSpawnPoint != null ? missileSpawnPoint.position : transform.position + playerCamera.transform.forward * 1f;
        spawnPosition.y = 1.0f; // 몬스터 Y값과 동일하게 고정
        GameObject missile = Instantiate(missilePrefab, spawnPosition, playerCamera.transform.rotation);
        Debug.Log("미사일 발사: " + spawnPosition);
    }
}