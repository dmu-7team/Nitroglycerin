using UnityEngine;

public class Player_Look : MonoBehaviour
{
    public Camera playerCamera;
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;
    private Vector2 lookInput;

    void Start()
    {
        // playerCamera가 설정되지 않았다면 자식에서 자동으로 찾기
        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
        }

        if (playerCamera == null)
        {
            Debug.LogError("Player_Look: playerCamera가 설정되지 않았습니다. 인스펙터에서 카메라를 연결하거나 자식 오브젝트에 카메라를 추가해주세요.", this);
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
            Debug.LogWarning("Player_Look: playerCamera가 설정되지 않아 회전을 실행할 수 없습니다.", this);
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