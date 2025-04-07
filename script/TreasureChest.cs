using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public GameObject rewardPrefab;  // 보물상자에서 생성될 보상 아이템 프리팹
    public Transform rewardSpawnPoint; // 보상이 나타날 위치
    public bool isOpen = false;  // 보물상자가 열렸는지 여부
    public Animator chestAnimator;  // 보물상자의 애니메이터 (선택사항)

    void Start()
    {
        if (chestAnimator == null)
            chestAnimator = GetComponent<Animator>();
    }

    public void OpenChest()
    {
        if (isOpen)
        {
            Debug.Log("이미 열려있는 보물상자입니다.");
            return;
        }

        isOpen = true;

        // 보물상자 애니메이션 재생 (선택 사항)
        if (chestAnimator != null)
        {
            chestAnimator.SetTrigger("Open");
        }

        Debug.Log("보물상자가 열렸습니다! 보상을 획득합니다.");

        // 보상 생성
        if (rewardPrefab != null && rewardSpawnPoint != null)
        {
            GameObject reward = Instantiate(rewardPrefab, rewardSpawnPoint.position, rewardSpawnPoint.rotation);
            Debug.Log("보상이 나타났습니다: " + reward.name);
        }
    }
}
