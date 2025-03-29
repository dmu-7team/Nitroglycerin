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
            Debug.LogWarning("Rigidbody�� player ������Ʈ�� �����ϴ�. Rigidbody�� �߰����ּ���.");
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
            Debug.LogWarning("Rigidbody�� ���� �̵��� �� �����ϴ�.");
            return;
        }

        Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized * moveSpeed;
        move.y = rb.linearVelocity.y;
        rb.linearVelocity = move;
    }
}