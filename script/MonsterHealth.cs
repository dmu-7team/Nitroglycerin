using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    public float health = 100f; // 초기 체력 설정

    void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트가 미사일인지 확인
        if (collision.gameObject.CompareTag("Missile"))
        {
            health -= 20f; // 미사일 충돌 시 체력 20 감소
            Debug.Log("몬스터 체력: " + health);

            if (health <= 0)
            {
                Destroy(gameObject); // 체력이 0 이하가 되면 몬스터 제거
                Debug.Log("몬스터가 파괴되었습니다!");
            }
        }
    }
}