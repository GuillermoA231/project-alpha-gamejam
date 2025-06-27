using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;
using System;

public class InventoryItemInfo : MonoBehaviour
{
    [Header(" Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI recyclePriceText;

    [Header(" Colors")]
    [SerializeField] private Image containerColor;
    [SerializeField] private Image outlineColor;


    [Header(" Stats")]
    [SerializeField] private Transform statsParent;
    [Header("Buttons")]
    [SerializeField] private Button upgradeButton;
    [field: SerializeField]public Button MeltButton { get; private set; }
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        if (upgradeButton != null)
            upgradeButton.onClick.AddListener(() => PlayUpgradeSound());

        if (MeltButton != null)
            MeltButton.onClick.AddListener(() => PlayMeltSound());

        if (closeButton != null)
            closeButton.onClick.AddListener(() => PlayClickSound());
    }

    private void PlayClickSound()
    {
        AudioManager.instance?.PlaySFX2D(AudioManager.instance.clickClip);
    }

    private void PlayMeltSound()
    {
        AudioManager.instance?.PlaySFX2D(AudioManager.instance.sellClip);
    }

    private void PlayUpgradeSound()
    {
        AudioManager.instance?.PlaySFX2D(AudioManager.instance.upgradeClip);
    }

    private string UpdateWeaponName(string weaponName, int weaponLevel)
    {
        string localizedWeapon = "";
        string localizationTable = "Weapons";
        var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(localizationTable, weaponName);

        if (op.IsDone)
        {
            localizedWeapon = $"{op.Result} (LVL {weaponLevel + 1})";
        }
        else
        {
            op.Completed += handle =>
            {
                localizedWeapon = $"{handle.Result} (LVL {weaponLevel + 1})";
            };
        }

        return localizedWeapon;
    }


    private string UpdateObjectName(string objectName)
    {
        string localizedObject = "";
        string localizationTable = "Objects";
        var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(localizationTable, objectName);
        localizedObject = op.Result;

        if (op.IsDone)
        {
            localizedObject = op.Result;
        }
        else
        {
            op.Completed += handle =>
            {
                localizedObject = op.Result;
            };
        }
        return localizedObject;
    }
    public void ConfigureWeapon(Weapon weapon)
    {
        Configure(

            weapon.WeaponData.Sprite,

            UpdateWeaponName(weapon.WeaponData.name, weapon.Level),

            WeaponStatsCalculator.getMeltPrice(weapon.WeaponData, weapon.Level),

            ColorHolder.GetColor(weapon.Level),

            ColorHolder.GetOutlineColor(weapon.Level),

            WeaponStatsCalculator.GetStats(weapon.WeaponData, weapon.Level)

        );
        
        if(weapon.Level != 3)
            upgradeButton.gameObject.SetActive(true);
        //MeltButton.gameObject.SetActive(true);
    }


    public void ConfigureObject(ObjectDataSO objectData)
    {

        Configure(

            objectData.Icon,

            UpdateObjectName(objectData.name),

            objectData.MeltPrice,

            ColorHolder.GetColor(objectData.Rarity),

            ColorHolder.GetOutlineColor(objectData.Rarity),

            objectData.baseStats

        );
        upgradeButton.gameObject.SetActive(false);
        //MeltButton.gameObject.SetActive(false);
    }

    private void Configure(Sprite itemIcon, string name, int recylcePrice, Color mainColor, Color secondColor, Dictionary<Stat, float> stats)
    {
        icon.sprite = itemIcon;

        itemNameText.text = name;

        recyclePriceText.text = recylcePrice.ToString();

        containerColor.color = mainColor;
        outlineColor.color = secondColor;
        itemNameText.color = secondColor;

        StatContainerManager.GenerateStatContainers(stats, statsParent);
    }
}
