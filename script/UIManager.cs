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

    [Header("체력바 UI")]
    public Image healthBarImage;

    [Header("경험치바 UI")]
    public Image expBarImage; // 경험치바 추가

    private Coroutine expBarCoroutine; // 경험치 애니메이션용
    private Coroutine healthBarCoroutine; //  추가 (현재 재생 중인 체력 애니메이션 기억)
    private Coroutine chestMessageCoroutine;
    private Coroutine levelUpCoroutine;
    private Coroutine itemEffectCoroutine;
    private Coroutine healthBlinkCoroutine;
    private Coroutine expLevelUpEffectCoroutine; //  레벨업 이펙트 코루틴 저장

    private bool isBlinking = false;         

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
    public void SetExp(float current, float max)
    {
        expText.text = $"{current}/{max}";

        if (expBarImage != null)
        {
            float targetFill = current / max;

            if (expBarCoroutine != null)
                StopCoroutine(expBarCoroutine);

            expBarCoroutine = StartCoroutine(AnimateExpBar(targetFill));
        }
    }


    public void SetHealth(int current, int max)
    {
        healthText.text = $"{current}/{max}";

        if (healthBarImage != null)
        {
            float targetFill = (float)current / max;

            if (healthBarCoroutine != null)
                StopCoroutine(healthBarCoroutine);

            healthBarCoroutine = StartCoroutine(AnimateHealthBar(targetFill));

            // 체력 색상 변경
            if (targetFill <= 0.3f)
            {
                healthBarImage.color = Color.red;
            }
            else
            {
                healthBarImage.color = Color.green;
            }

            //  깜빡이기 처리 추가
            if (targetFill <= 0.1f)
            {
                if (!isBlinking)
                {
                    isBlinking = true;
                    healthBlinkCoroutine = StartCoroutine(BlinkHealthBar());
                }
            }
            else
            {
                if (isBlinking)
                {
                    isBlinking = false;
                    if (healthBlinkCoroutine != null)
                        StopCoroutine(healthBlinkCoroutine);
                    healthBarImage.enabled = true; // 깜빡임 중지하고 다시 켬
                }
            }
        }
    }
    

    //  체력바 깜빡이는 코루틴
    private IEnumerator BlinkHealthBar()
    {
        while (true)
        {
            healthBarImage.enabled = !healthBarImage.enabled;
            yield return new WaitForSeconds(0.3f); // 0.3초 간격으로 깜빡
        }
    }


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

    //  추가: 체력바 부드럽게 애니메이션
    private IEnumerator AnimateHealthBar(float targetFill)
    {
        float duration = 0.5f; // 부드럽게 이동하는 데 걸리는 시간
        float elapsed = 0f;
        float startFill = healthBarImage.fillAmount;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            healthBarImage.fillAmount = Mathf.Lerp(startFill, targetFill, elapsed / duration);
            yield return null;
        }

        healthBarImage.fillAmount = targetFill; // 최종 값 정확히 맞추기
    }
    //  경험치바 부드럽게 채우는 코루틴
    private IEnumerator AnimateExpBar(float targetFill)
    {
        float duration = 0.5f;
        float elapsed = 0f;
        float startFill = expBarImage.fillAmount;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            expBarImage.fillAmount = Mathf.Lerp(startFill, targetFill, elapsed / duration);
            yield return null;
        }

        expBarImage.fillAmount = targetFill;
    }
    // 경험치바 레벨업 반짝이는 이펙트
    public void PlayExpLevelUpEffect()
    {
        if (expBarImage == null) return;

        if (expLevelUpEffectCoroutine != null)
            StopCoroutine(expLevelUpEffectCoroutine);

        expLevelUpEffectCoroutine = StartCoroutine(ExpLevelUpEffect());
    }

    private IEnumerator ExpLevelUpEffect()
    {
        Color originalColor = expBarImage.color;
        Color highlightColor = Color.yellow; // 반짝일 때 노란색으로 (원하는 색 가능)

        float flashDuration = 0.2f; // 한번 반짝이는 시간
        int flashCount = 3; // 몇 번 반짝일지

        for (int i = 0; i < flashCount; i++)
        {
            expBarImage.color = highlightColor;
            yield return new WaitForSeconds(flashDuration);
            expBarImage.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }

        expBarImage.color = originalColor; // 마지막에 원래 색 복구
    }

}
