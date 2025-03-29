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
            Debug.LogError("playerCamera �Ǵ� missilePrefab�� �������� �ʾҽ��ϴ�!", this);
            return;
        }

        // �̻��� �߻� ��ġ: ���Ϳ� ���� ���̷� ����
        Vector3 spawnPosition = missileSpawnPoint != null ? missileSpawnPoint.position : transform.position + playerCamera.transform.forward * 1f;
        spawnPosition.y = 1.0f; // ���� Y���� �����ϰ� ����
        GameObject missile = Instantiate(missilePrefab, spawnPosition, playerCamera.transform.rotation);
        Debug.Log("�̻��� �߻�: " + spawnPosition);
    }
}