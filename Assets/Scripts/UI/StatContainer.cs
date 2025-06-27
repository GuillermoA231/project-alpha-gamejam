// StatContainer.cs
using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;

public class StatContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private UnityEngine.UI.Image statImage;
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private TextMeshProUGUI statValueText;

    private Stat statType;
    private float statValue;

    private string localeStatTableName = "Stats";

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    public void InitializeStatContainer(Sprite icon, Stat stat, float value, bool useColor = false)
    {
        statImage.sprite = icon;
        statType = stat;
        statValue = value;
        if (useColor)
            ColorizeStatValueText(value);
        else
        {
            statValueText.color = Color.white;
            statValueText.text = FormatValue(value);
        }
        UpdateStatName();
    }

    private void OnLocaleChanged(UnityEngine.Localization.Locale _)
    {
        UpdateStatName();
    }

    private void UpdateStatName()
    {
        string key = statType.ToString();
        var stringOp = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(localeStatTableName, key);

        if (stringOp.IsDone)
        {
            string result = stringOp.Result;
            statText.text = string.IsNullOrEmpty(result) || result == key
                ? Enums.FormatStatName(statType)
                : result;
        }
        else
        {
            stringOp.Completed += handle =>
            {
                string result = handle.Result;
                statText.text = string.IsNullOrEmpty(result) || result == key
                    ? Enums.FormatStatName(statType)
                    : result;
            };
        }
    }
    private void ColorizeStatValueText(float value)
    {
        float sign = Mathf.Sign(value);
        if (value == 0) sign = 0;
        float absValue = Mathf.Abs(value);
        Color c = Color.white;
        if (sign > 0) c = Color.green;
        else if (sign < 0) c = Color.red;
        statValueText.color = c;
        statValueText.text = FormatValue(value);
    }
     private string FormatValue(float value)
    {
        if (Mathf.Approximately(value, Mathf.Round(value)))
            return Mathf.RoundToInt(value).ToString();
        return value.ToString("F2");
    }


    public float GetFontSize() => statText.fontSize;
    public void SetFontSize(float fontSize)
    {
        statText.fontSizeMax = fontSize;
        statValueText.fontSizeMax = fontSize;
    }
}
