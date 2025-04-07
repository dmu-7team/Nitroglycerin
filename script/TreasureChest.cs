using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    [Header("보상 아이템 설정")]
    public GameObject[] rewardPrefabs;   // Inspector에서 설정할 수 있는 보상 아이템 배열 (여기에 프리팹 넣기)

    private bool isOpen = false;

    public void OpenChest()
    {
        if (isOpen)
        {
            Debug.Log("이미 열려있는 보물상자입니다.");
            return;
        }

        isOpen = true;
        Debug.Log("보물상자가 열렸습니다! 보상을 획득합니다.");

        // 보상 생성 (두 종류 중 하나 랜덤하게)
        if (rewardPrefabs != null && rewardPrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, rewardPrefabs.Length); // 배열에서 무작위로 선택
            GameObject selectedReward = rewardPrefabs[randomIndex];  // 선택된 아이템

            // 보물상자가 있던 자리에 아이템 생성
            GameObject reward = Instantiate(selectedReward, transform.position, Quaternion.identity);

            Debug.Log("보상이 나타났습니다: " + reward.name + " 위치: " + reward.transform.position);
        }
        else
        {
            Debug.LogError("보상 프리팹이 설정되지 않았습니다! (보물상자 Inspector에서 보상 아이템을 추가하세요)");
        }

        // 보물상자 오브젝트 제거하기
        Destroy(gameObject);
    }
}
