using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;
using NUnit.Framework;
using System;

public class ShopObjectContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI buyText;
    [SerializeField] public Button purchaseButton;

    [Header("Stats")]
    [SerializeField] private Transform statsContainerParent;

    [Header("Lock Button")]
    [SerializeField] private Image lockImage;
    [SerializeField] private Image lockContainer;
    [SerializeField] private Sprite unlockedSprite, lockedSprite;
    public bool IsLocked { get; private set; }


    [Header("Color")]
    [SerializeField] private Image[] containerImage;
    [SerializeField] private Image nameBackgroundColor;
    [SerializeField] private Outline containerOutline;


    [Header("Actions")]
    public static Action<ShopObjectContainer, int> onPurchased;

    [Header("Purchase")]
    private int weaponLevel;
    private int objectLevel;
    public WeaponDataSO WeaponData { get; private set; }
    public ObjectDataSO ObjectData { get; private set; }

    [Header("Localization")]
    private string objectKey;
    private string weaponKey;

    private void Awake()
    {
        CurrencyManager.onUpdated += CurrencyUpdatedCallback;
    }
    
    private void OnDestroy()
    {
        CurrencyManager.onUpdated -= CurrencyUpdatedCallback;
    }

    private void CurrencyUpdatedCallback()
    {
        if (WeaponData == null && ObjectData == null)
        return;

        
        int itemPrice;

        if (WeaponData != null)
        {
            itemPrice = WeaponStatsCalculator.getPurchasePrice(WeaponData, weaponLevel);
        }
        else
        {
            itemPrice = ObjectData.Price;
        }

        purchaseButton.interactable = CurrencyManager.instance.HasEnoughCurrency(itemPrice);
    }

    

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private void OnLocaleChanged(UnityEngine.Localization.Locale _)
    {
        UpdateWeaponName();
    }

    private void UpdateWeaponName()
    {
        string localizationTable = "Weapons";
        int displayLevel = weaponLevel + 1;
        var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(localizationTable, weaponKey);

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
    private void UpdateObjectName()
    {
        string localizationTable = "Objects";
        int displayLevel = objectLevel + 1;
        var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(localizationTable, objectKey);

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



    public void Configure(WeaponDataSO weaponData, int level)
    {
        WeaponData = weaponData;
        icon.sprite = weaponData.Sprite;
        icon.transform.rotation = Quaternion.Euler(0, 0, 30);
        weaponLevel = level;
        weaponKey = weaponData.name;
        int weaponPrice = WeaponStatsCalculator.getPurchasePrice(weaponData, level);
        buyText.text = weaponPrice.ToString();


        Color imageColor = ColorHolder.GetColor(level);
        Color outlineColor = ColorHolder.GetOutlineColor(level);
        nameText.color = imageColor;
        nameBackgroundColor.color = outlineColor;
        containerOutline.effectColor = outlineColor;

        foreach (var img in containerImage)
            img.color = imageColor;

        UpdateWeaponName();

        var calculatedStats = WeaponStatsCalculator.GetStats(weaponData, level);
        GenerateStatContainers(calculatedStats);


        purchaseButton.onClick.AddListener(Purchase);
        purchaseButton.interactable = CurrencyManager.instance.HasEnoughCurrency(weaponPrice);
    }
    public void Configure(ObjectDataSO objectData)
    {
        ObjectData = objectData;

        icon.sprite = objectData.Icon;
        icon.transform.rotation = Quaternion.Euler(0, 0, 0);
        objectLevel = objectData.Rarity;
        objectKey = objectData.name;
        buyText.text = objectData.Price.ToString();

        Color imageColor = ColorHolder.GetColor(objectData.Rarity);
        Color outlineColor = ColorHolder.GetOutlineColor(objectData.Rarity);
        nameText.color = imageColor;
        nameBackgroundColor.color = outlineColor;
        containerOutline.effectColor = outlineColor;

        foreach (var img in containerImage)
            img.color = imageColor;

        UpdateObjectName();

        GenerateStatContainers(objectData.baseStats);

        purchaseButton.onClick.AddListener(Purchase);
        
        //CurrencyManager.instance.UseCoins(ObjectData.Price);

        purchaseButton.interactable = CurrencyManager.instance.HasEnoughCurrency(objectData.Price);
    }

    private void Purchase()
    {
        onPurchased?.Invoke(this, weaponLevel);
    }


    private void GenerateStatContainers(Dictionary<Stat, float> stats)
    {
        statsContainerParent.Clear();
        StatContainerManager.GenerateStatContainers(stats, statsContainerParent);
    }

    public void LockButtonCallback()
    {
        IsLocked = !IsLocked;

        UpdateLockVisuals();
    }

    private void UpdateLockVisuals()
    {
        Color color = lockContainer.color;
        color.a = IsLocked ? 1f : 0.5f;
        lockContainer.color = color;
        lockImage.sprite = IsLocked ? lockedSprite : unlockedSprite;
    }
}
