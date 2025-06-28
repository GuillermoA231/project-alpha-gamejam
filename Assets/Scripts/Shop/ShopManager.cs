using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private Transform containersParent;
    [SerializeField] private ShopObjectContainer shopItemContainerPrefab;


    [Header("Player Elements")]
    [SerializeField] private PlayerWeapons playerWeapons;
    [SerializeField] private PlayerObjects playerObjects;

    [Header("Reroll Settings (temporary, no wave)")]
    [SerializeField] private int baseRerollPrice = 5;
    [SerializeField] private int rerollPriceIncrement = 2;

    [Header("Reroll UI")]
    [SerializeField] private Button rerollButton;
    [SerializeField] private Image rerollPriceContainerImage;
    [SerializeField] private Image rerollPriceOutlineImage;
    [SerializeField] private TextMeshProUGUI rerollPriceText;


    [Header("Actions")]
    public static Action onItemPurchased;


    private int rerollCount;
    private int freeRerolls;

    private void Awake()
    {
        ShopObjectContainer.onPurchased += ObjectPurchasedCallback;
        CurrencyManager.onUpdated += CurrencyUpdatedCallback;
    }

    private void OnDestroy()
    {
        ShopObjectContainer.onPurchased -= ObjectPurchasedCallback;
        CurrencyManager.onUpdated -= CurrencyUpdatedCallback;
    }


    public void PlayRerollSound()
    {
        AudioManager.instance?.PlaySFX2D(AudioManager.instance.rerollClip);
    }

    public void GameStateChangedCallback(GameState gameState)
    {
        if (gameState == GameState.SHOP)
        {
            StartNewShopSession();
            Configure();
            UpdateRerollVisual();
        }
    }

    public void StartNewShopSession()
    {
        rerollCount = 0;
        freeRerolls = 0;
    }

    private void Configure()
    {
        List<GameObject> toDestroy = new List<GameObject>();
        for (int i = 0; i < containersParent.childCount; i++)
        {
            ShopObjectContainer shopContainer = containersParent.GetChild(i).GetComponent<ShopObjectContainer>();
            if (!shopContainer.IsLocked)
                toDestroy.Add(shopContainer.gameObject);
        }
        foreach (var go in toDestroy)
        {
            go.transform.SetParent(null);
            Destroy(go);
        }

        int containerstoAdd = Mathf.Max(0, 6 - containersParent.childCount);
        int weaponContainerCount = Random.Range(0, containerstoAdd + 1);
        int objectContainerCount = containerstoAdd - weaponContainerCount;

        for (int i = 0; i < weaponContainerCount; i++)
        {
            var weaponContainerInstance = Instantiate(shopItemContainerPrefab, containersParent);
            WeaponDataSO randomWeapon = ResourcesManager.getRandomWeapon();
            weaponContainerInstance.name = "Weapon Container " + i;
            weaponContainerInstance.Configure(randomWeapon, Random.Range(0, 4));
        }
        for (int i = 0; i < objectContainerCount; i++)
        {
            var objectContainerInstance = Instantiate(shopItemContainerPrefab, containersParent);
            ObjectDataSO randomObject = ResourcesManager.getRandomObject();
            objectContainerInstance.name = "Object Container " + i;
            objectContainerInstance.Configure(randomObject);
        }
    }

    public void Reroll()
    {
        int cost = GetCurrentRerollPrice();
        if (freeRerolls > 0)
        {
            freeRerolls--;
        }
        else if (CurrencyManager.instance.HasEnoughCurrency(cost))
        {
            AudioManager.instance.PlayRerollSound();
            CurrencyManager.instance.UseCoins(cost);
        }
        else
        {
            AudioManager.instance.PlayDenySound();
            return;
        }

        rerollCount++;
        Configure();
        UpdateRerollVisual();
    }

    private int GetCurrentRerollPrice()
    {
        return baseRerollPrice + rerollCount * rerollPriceIncrement;
    }

    private bool CanReroll()
    {
        int cost = GetCurrentRerollPrice();
        return freeRerolls > 0 || CurrencyManager.instance.HasEnoughCurrency(cost);
    }

    private void UpdateRerollVisual()
    {
        bool canReroll = CanReroll();
        rerollPriceText.text = freeRerolls > 0 ? "Free" : GetCurrentRerollPrice().ToString();
        rerollButton.interactable = canReroll;
        ApplyRerollColors(canReroll);
    }

    private void CurrencyUpdatedCallback()
    {
        UpdateRerollVisual();
    }

    public void AddFreeRerolls(int amount = 1)
    {
        freeRerolls += amount;
        UpdateRerollVisual();
    }

    public void OnBoughtAllItems()
    {
        AddFreeRerolls(1);
    }

    private void ApplyRerollColors(bool can)
    {
        Color bgColor;
        Color outlineColor;
        Color textColor;
        if (can)
        {
            ColorUtility.TryParseHtmlString("#FFE500", out bgColor);
            ColorUtility.TryParseHtmlString("#AD7D00", out outlineColor);
            textColor = Color.white;
        }
        else
        {
            ColorUtility.TryParseHtmlString("#FF0000", out bgColor);
            ColorUtility.TryParseHtmlString("#8B0000", out outlineColor);
            textColor = Color.black;
        }
        if (rerollPriceContainerImage != null)
            rerollPriceContainerImage.color = bgColor;
        if (rerollPriceOutlineImage != null)
            rerollPriceOutlineImage.color = outlineColor;
        if (rerollPriceText != null)
            rerollPriceText.color = textColor;
    }

    private void ObjectPurchasedCallback(ShopObjectContainer objectContainer, int weaponLevel)
    {
        if (objectContainer.WeaponData != null)
        {
            TryPurchaseWeapon(objectContainer, weaponLevel);
        }
        if (objectContainer.ObjectData != null)
        {
            TryPurchaseObject(objectContainer);
        }

        InventoryManager.instance.Configure();
    }

    private void TryPurchaseWeapon(ShopObjectContainer container, int weaponLevel)
    {
        if (playerWeapons.TryAddWeapon(container.WeaponData, weaponLevel))
        {
            int price = WeaponStatsCalculator.getPurchasePrice(container.WeaponData, weaponLevel);
            CurrencyManager.instance.UseCoins(price);

            AudioManager.instance.PlayBuySound();

            Destroy(container.gameObject);
        }
        else
        {
            AudioManager.instance.PlayDenySound();
        }
        onItemPurchased?.Invoke();
    }
    private void TryPurchaseObject(ShopObjectContainer container)
    {
        playerObjects.AddObject(container.ObjectData);
        CurrencyManager.instance.UseCoins(container.ObjectData.Price);

        AudioManager.instance.PlayBuySound();

        Destroy(container.gameObject);
        onItemPurchased?.Invoke();
    }
    
}
