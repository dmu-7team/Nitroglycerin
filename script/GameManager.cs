using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int coinCount = 0;

    private PlayerStatus playerStatus;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        playerStatus = FindFirstObjectByType<PlayerStatus>();
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    public void AddExperience(float amount)
    {
        if (playerStatus != null)
            playerStatus.AddExp(amount);
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        UIManager.instance?.SetCoin(coinCount);
    }

    public void UpdateUI()
    {
        if (playerStatus == null || UIManager.instance == null) return;

        UIManager.instance.SetCoin(coinCount);
        UIManager.instance.SetExp(playerStatus.currentExp, playerStatus.expToLevelUp);
        UIManager.instance.SetHealth(playerStatus.currentHealth, playerStatus.maxHealth);
        UIManager.instance.SetDamage(playerStatus.attackDamage);
        UIManager.instance.SetSpeed(playerStatus.moveSpeed);
    }
}
