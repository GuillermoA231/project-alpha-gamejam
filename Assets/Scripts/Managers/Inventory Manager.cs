using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameStateListener
{

    public static InventoryManager instance;

    [Header("Player Components")]
    [SerializeField] private PlayerWeapons playerWeapons;
    [SerializeField] private PlayerObjects playerObjects;

    [Header("Elements")]
    [SerializeField] private Transform inventoryItemsParent;
    [SerializeField] private InventoryItemContainer inventoryItemsContainer;
    [SerializeField] private ShopManagerUI shopManagerUI;
    [SerializeField] private InventoryItemInfo itemInfo;

    public void Awake()
    {
        ShopManager.onItemPurchased += ItemPurchasedCallback;

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    public void OnDestroy()
    {
        ShopManager.onItemPurchased -= ItemPurchasedCallback;
    }

    private void ItemPurchasedCallback() => Configure();
    public void GameStateChangedCallback(GameState gameState)
    {
        if (gameState == GameState.SHOP)
            Configure();
    }

    public void Configure()
    {
        inventoryItemsParent.Clear();

        // Player Weapons / Objects
        //Weapons
        Weapon[] weapons = playerWeapons.GetWeapons();

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null)
                continue;
            InventoryItemContainer container = Instantiate(inventoryItemsContainer, inventoryItemsParent);

            // Color containerColor = ColorHolder.GetColor(weaponDatas[i].Level);
            // Color outlineColor = ColorHolder.GetOutlineColor(weaponDatas[i].Level);
            // Sprite icon = weaponDatas[i].WeaponData.Sprite;
            container.Configure(weapons[i],i, () => ShowItemInfo(container));
        }

        //Objects
        ObjectDataSO[] objectDatas = playerObjects.Objects.ToArray();

        for (int i = 0; i < objectDatas.Length; i++)
        {
            InventoryItemContainer container = Instantiate(inventoryItemsContainer, inventoryItemsParent);

            // Color containerColor = ColorHolder.GetColor(objectDatas[i].Rarity);
            // Color outlineColor = ColorHolder.GetOutlineColor(objectDatas[i].Rarity);
            // Sprite icon = objectDatas[i].Icon;
            container.Configure(objectDatas[i], () => ShowItemInfo(container));
        }

    }

    private void ShowItemInfo(InventoryItemContainer container)
    {
        if (container.Weapon != null)
            ShowWeaponInfo(container.Weapon, container.Index);
        else
            ShowObjectInfo(container.ObjectData);

    }

    private void ShowWeaponInfo(Weapon weapon, int index)
    {
        itemInfo.ConfigureWeapon(weapon);

        
        itemInfo.MeltButton.onClick.RemoveAllListeners();
        itemInfo.MeltButton.onClick.AddListener(() => MeltWeapon(index));
        
        shopManagerUI.ShowItemInfo();
    }
    private void ShowObjectInfo(ObjectDataSO objectData)
    {
        itemInfo.ConfigureObject(objectData);
        itemInfo.MeltButton.onClick.RemoveAllListeners();
        itemInfo.MeltButton.onClick.AddListener(() => MeltObject(objectData));
        shopManagerUI.ShowItemInfo();
    }

    private void MeltObject(ObjectDataSO objectToRecycle)
    {
        playerObjects.MeltObject(objectToRecycle);

        Configure();

        shopManagerUI.HideItemInfo();
    }
    private void MeltWeapon(int index)
    {
        playerWeapons.MeltWeapon(index);

        Configure();

        shopManagerUI.HideItemInfo();
    }
}
