using UnityEngine;

public class Player_Look : MonoBehaviour
{
    public Camera playerCamera;
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;
    private Vector2 lookInput;

    void Start()
    {
        // playerCamera�� �������� �ʾҴٸ� �ڽĿ��� �ڵ����� ã��
        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
        }

        if (playerCamera == null)
        {
            Debug.LogError("Player_Look: playerCamera�� �������� �ʾҽ��ϴ�. �ν����Ϳ��� ī�޶� �����ϰų� �ڽ� ������Ʈ�� ī�޶� �߰����ּ���.", this);
        }
    }

    public void SetLookInput(Vector2 input)
    {
        lookInput = input;
    }

    public void Look()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("Player_Look: playerCamera�� �������� �ʾ� ȸ���� ������ �� �����ϴ�.", this);
            return;
        }

        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}