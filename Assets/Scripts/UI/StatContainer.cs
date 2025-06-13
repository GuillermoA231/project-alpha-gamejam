// StatContainer.cs
using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;

public class StatContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private UnityEngine.UI.Image statImage;
    [SerializeField] private TextMeshProUGUI statText;       // Localized stat name
    [SerializeField] private TextMeshProUGUI statValueText;  // Numeric value

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

    public void InitializeStatContainer(Sprite icon, Stat stat, float value)
    {
        statImage.sprite = icon;
        statType = stat;
        statValue = value;

        statValueText.text = statValue.ToString("F0");
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
            statText.text = stringOp.Result;
        }
        else
        {
            stringOp.Completed += handle =>
            {
                statText.text = handle.Result;
            };
        }
    }

    public float GetFontSize() => statText.fontSize;
    public void SetFontSize(float fontSize)
    {
        statText.fontSizeMax = fontSize;
        statValueText.fontSizeMax = fontSize;
    }
}
