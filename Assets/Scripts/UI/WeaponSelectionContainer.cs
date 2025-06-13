using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;

public class WeaponSelectionContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private Image nameBackgroundColor;
    [SerializeField] private Outline containerOutline;
    [SerializeField] private TextMeshProUGUI nameText;

    [Header("Stats")]
    [SerializeField] private Transform statsContainerParent;

    [Header("Color")]
    [SerializeField] private Image[] containerImage;

    [field: SerializeField] public Button Button { get; private set; }

    private string weaponKey;
    private int weaponLevel;

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    public void Configure(Sprite sprite, int level, WeaponDataSO weaponData)
    {
        icon.sprite = sprite;
        weaponLevel = level;
        weaponKey = weaponData.name;

        Color imageColor = ColorHolder.GetColor(level);
        Color outlineColor = ColorHolder.GetOutlineColor(level);
        nameText.color = imageColor;
        nameBackgroundColor.color = outlineColor;
        containerOutline.effectColor = outlineColor;
        foreach (var img in containerImage)
            img.color = imageColor;

        UpdateWeaponName();

        var calculatedStats = WeaponStatsCalculator.GetStats(weaponData, level);
        StatContainerManager.GenerateStatContainers(calculatedStats, statsContainerParent);
    }

    private void OnLocaleChanged(UnityEngine.Localization.Locale _)
    {
        UpdateWeaponName();
    }

    private void UpdateWeaponName()
    {
        string table = "Weapons";
        int displayLevel = weaponLevel + 1;
        var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(table, weaponKey);

        if (op.IsDone)
        {
            nameText.text = $"{op.Result} (LVL {displayLevel})";
        }
        else
        {
            op.Completed += handle =>
            {
                nameText.text = $"{handle.Result} (LVL {displayLevel})";
            };
        }
    }

    public void Select()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one * 1.075f, .3f)
                 .setEase(LeanTweenType.easeInOutSine);
    }

    public void Deselect()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one, .3f)
                 .setEase(LeanTweenType.easeInOutSine);
    }
}
