using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("메시지 UI")]
    public TextMeshProUGUI chestMessageText;
    public TextMeshProUGUI levelUpMessageText;
    public TextMeshProUGUI itemEffectText;

    [Header("스탯 UI")]
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI speedText;

    private Coroutine chestMessageCoroutine;
    private Coroutine levelUpCoroutine;
    private Coroutine itemEffectCoroutine;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        chestMessageText?.gameObject.SetActive(false);
        levelUpMessageText?.gameObject.SetActive(false);
        itemEffectText?.gameObject.SetActive(false);
    }

    public void SetCoin(int amount) => coinText.text = $"코인 : {amount}";
    public void SetExp(float current, float max) => expText.text = $"{current}/{max}";
    public void SetHealth(int current, int max) => healthText.text = $"{current}/{max}";
    public void SetDamage(float amount) => damageText.text = $"공격력: {amount}";
    public void SetSpeed(float amount) => speedText.text = $"속도: {amount}";

    public void ShowChestMessage(string message, float duration = 2f)
    {
        if (chestMessageCoroutine != null) StopCoroutine(chestMessageCoroutine);
        chestMessageCoroutine = StartCoroutine(ShowTempMessage(chestMessageText, message, duration));
    }

    public void ShowLevelUpMessage(string message, float duration = 2f)
    {
        if (levelUpCoroutine != null) StopCoroutine(levelUpCoroutine);
        levelUpCoroutine = StartCoroutine(ShowTempMessage(levelUpMessageText, message, duration));
    }

    public void ShowItemEffectMessage(string message, float duration = 2f)
    {
        if (itemEffectCoroutine != null) StopCoroutine(itemEffectCoroutine);
        itemEffectCoroutine = StartCoroutine(ShowTempMessage(itemEffectText, message, duration));
    }

    private IEnumerator ShowTempMessage(TextMeshProUGUI target, string message, float duration)
    {
        target.text = message;
        target.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        target.gameObject.SetActive(false);
    }
}
