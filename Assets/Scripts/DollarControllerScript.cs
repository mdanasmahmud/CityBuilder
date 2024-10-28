using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DollarManager : MonoBehaviour
{
    public TextMeshProUGUI dollarTotalText;
    public TextMeshProUGUI dollarRateText;
    public Button[] buttons;
    public TextMeshProUGUI[] buttonCostTexts;
    public int[] buttonCosts;

    private float dollarTotal = 0f;
    public float dollarRate = 1f;
    private bool isRateIncreaseActive = false;
    private float baseRate = 1f;  // Store the original rate
    private int tapCount = 0;     // Track number of taps
    private const int MAX_TAP_MULTIPLIER = 5;  // Maximum multiplier

    void Start()
    {
        // Existing Start code remains the same...
        if (!string.IsNullOrEmpty(dollarTotalText.text))
        {
            string dollarTotalTextValue = dollarTotalText.text.Replace("$", "").Trim();
            if (!float.TryParse(dollarTotalTextValue, out dollarTotal))
            {
                Debug.LogError($"Invalid format for dollarTotalText: '{dollarTotalText.text}'. Setting dollarTotal to 0.");
                dollarTotal = 0f;
            }
        }
        else
        {
            Debug.LogError("dollarTotalText is empty. Setting dollarTotal to 0.");
            dollarTotal = 0f;
        }

        if (!string.IsNullOrEmpty(dollarRateText.text))
        {
            string dollarRateTextValue = dollarRateText.text.Replace("$", "").Trim();
            if (!float.TryParse(dollarRateTextValue, out dollarRate))
            {
                Debug.LogError($"Invalid format for dollarRateText: '{dollarRateText.text}'. Setting dollarRate to 5.");
                dollarRate = 5f;
            }
        }
        else
        {
            Debug.LogError("dollarRateText is empty. Setting dollarRate to 5.");
            dollarRate = 5f;
        }

        baseRate = dollarRate;  // Store the initial rate
        dollarRateText.text = $"${FormatDollarAmount(dollarRate)} /S";

        for (int i = 0; i < buttonCostTexts.Length; i++)
        {
            buttonCostTexts[i].text = $"${FormatDollarAmount(buttonCosts[i], true)}";
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }

        InvokeRepeating("IncreaseDollarTotal", 1f, 1f);
    }

    public void IncreaseDollarRateByTap(float duration)
    {
        if (!isRateIncreaseActive)
        {
            StartCoroutine(IncreaseDollarRateCoroutine(duration));
        }
        else
        {
            // If already active, increment tap count if below maximum
            if (tapCount < MAX_TAP_MULTIPLIER)
            {
                tapCount++;
                UpdateRateMultiplier();
            }
        }
    }

    private void UpdateRateMultiplier()
    {
        float multiplier = 1f + tapCount;
        dollarRate = baseRate * multiplier;
        dollarRateText.text = $"${FormatDollarAmount(dollarRate)} /S";
    }

    private IEnumerator IncreaseDollarRateCoroutine(float duration)
    {
        isRateIncreaseActive = true;
        tapCount = 1;
        float originalRate = baseRate;

        UpdateRateMultiplier();

        yield return new WaitForSeconds(duration);

        // Reset everything
        tapCount = 0;
        dollarRate = originalRate;
        dollarRateText.text = $"${FormatDollarAmount(dollarRate)} /S";
        isRateIncreaseActive = false;
    }

    // Rest of the existing DollarManager methods remain the same...
    void IncreaseDollarTotal()
    {
        dollarTotal += dollarRate;
        dollarTotalText.text = $"${FormatDollarAmount(dollarTotal)}";
        CheckButtonStatus();
    }

    void CheckButtonStatus()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Image buttonImage = buttons[i].GetComponent<Image>();
            if (dollarTotal < buttonCosts[i])
            {
                buttonImage.color = Color.gray;
                buttons[i].interactable = false;
            }
            else
            {
                buttonImage.color = Color.white;
                buttons[i].interactable = true;
            }
        }
    }

    void OnButtonClick(int index)
    {
        if (dollarTotal >= buttonCosts[index])
        {
            dollarTotal -= buttonCosts[index];
            dollarTotalText.text = $"${FormatDollarAmount(dollarTotal)}";

            buttonCosts[index] *= 2;
            buttonCostTexts[index].text = $"${FormatDollarAmount(buttonCosts[index], true)}";

            baseRate += 1;
            dollarRate = isRateIncreaseActive ? baseRate * (1f + tapCount) : baseRate;
            dollarRateText.text = $"${FormatDollarAmount(dollarRate)} /S";

            CheckButtonStatus();
        }
    }

    string FormatDollarAmount(float amount, bool isCost = false)
    {
        if (isCost)
        {
            return $"{Mathf.FloorToInt(amount)}";
        }
        else
        {
            if (amount >= 1000000)
            {
                return $"{(amount / 1000000f):F2}M";
            }
            else if (amount >= 1000)
            {
                return $"{(amount / 1000f):F2}K";
            }
            else
            {
                return $"{amount:F2}";
            }
        }
    }
}
