using UnityEngine;

public class Player_Move : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            Debug.LogWarning("Rigidbody가 player 오브젝트에 없습니다. Rigidbody를 추가해주세요.");
        }
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    public void Move()
    {
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody가 없어 이동할 수 없습니다.");
            return;
        }

        Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized * moveSpeed;
        move.y = rb.linearVelocity.y;
        rb.linearVelocity = move;
    }
}