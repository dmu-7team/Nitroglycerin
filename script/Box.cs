using UnityEngine;

public class Box : MonoBehaviour
{
    public ItemData[] itemOptions;
    private bool isOpened = false;

    public void Open(GameObject player)
    {
        if (isOpened) return;
        isOpened = true;

        GiveItemToPlayer(player);
        Destroy(gameObject);
    }

    private void GiveItemToPlayer(GameObject player)
    {
        if (itemOptions.Length == 0) return;

        int index = Random.Range(0, itemOptions.Length);
        ItemData selectedItem = itemOptions[index];

        Inventory inventory = player.GetComponent<Inventory>();
        if (inventory != null)
        {
            inventory.AddItem(selectedItem);
            Debug.Log($"[Box] {selectedItem.name} ม๖ฑÞตส");
        }
    }
}
