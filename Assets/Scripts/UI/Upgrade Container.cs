using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class UpgradeContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI upgradeNameText;
    [SerializeField] private TextMeshProUGUI upgradeValueText;
    [field: SerializeField] public Button Button { get; private set; }

    private Stat statType;

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    public void Configure(Sprite icon, Stat stat, string upgradeValue)
    {
        image.sprite = icon;
        statType = stat;
        upgradeValueText.text = upgradeValue;
        UpdateStatName();
    }

    private void OnLocaleChanged(Locale _)
    {
        UpdateStatName();
    }

    private void UpdateStatName()
    {
        string key = statType.ToString();
        var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("Stats", key);

        if (op.IsDone)
        {
            upgradeNameText.text = op.Result;
        }
        else
        {
            op.Completed += handle =>
            {
                upgradeNameText.text = handle.Result;
            };
        }
    }
}
