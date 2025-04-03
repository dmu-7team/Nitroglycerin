using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public GameObject speedBoostItemPrefab;
    public GameObject damageBoostItemPrefab;
    public float itemDropChance = 1f;

    private bool isOpened = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Missile") && !isOpened)
        {
            OpenChest();
            Destroy(collision.gameObject);
            Debug.Log("미사일이 보물상자와 충돌하여 상자가 열렸습니다!");
        }
    }

    public void OpenChest()
    {
        if (isOpened) return;

        isOpened = true;

        if (Random.value <= itemDropChance)
        {
            GameObject itemPrefab = Random.value > 0.5f ? speedBoostItemPrefab : damageBoostItemPrefab;
            if (itemPrefab != null)
            {
                Vector3 dropPosition = transform.position;
                dropPosition.y += 1.0f;
                GameObject item = Instantiate(itemPrefab, dropPosition, Quaternion.identity);
                Debug.Log($"보물상자에서 {item.name}이 드롭되었습니다: 위치: {dropPosition}");
            }
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ShowChestMessage();
        }

        StartCoroutine(OpenAnimation());
    }

    System.Collections.IEnumerator OpenAnimation()
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.up * 0.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            yield return null;
        }

        Destroy(gameObject);
        Debug.Log("보물상자가 열렸습니다!");
    }
}