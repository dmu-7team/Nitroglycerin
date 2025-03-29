using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    public float health = 100f; // �ʱ� ü�� ����

    void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� �̻������� Ȯ��
        if (collision.gameObject.CompareTag("Missile"))
        {
            health -= 20f; // �̻��� �浹 �� ü�� 20 ����
            Debug.Log("���� ü��: " + health);

            if (health <= 0)
            {
                Destroy(gameObject); // ü���� 0 ���ϰ� �Ǹ� ���� ����
                Debug.Log("���Ͱ� �ı��Ǿ����ϴ�!");
            }
        }
    }
}