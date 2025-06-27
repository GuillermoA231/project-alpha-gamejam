using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CurrencyText : MonoBehaviour
{
    [Header(" Elements")]
    private TextMeshProUGUI text;
    public void UpdateText(string currencyString)
    {
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
        }
        text.text = currencyString;
    }
}
