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

    public void Awake()
    {
        
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
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
        Weapon[] weaponDatas = playerWeapons.GetWeapons();

        for (int i = 0; i < weaponDatas.Length; i++)
        {
            InventoryItemContainer container = Instantiate(inventoryItemsContainer, inventoryItemsParent);

            // Color containerColor = ColorHolder.GetColor(weaponDatas[i].Level);
            // Color outlineColor = ColorHolder.GetOutlineColor(weaponDatas[i].Level);
            // Sprite icon = weaponDatas[i].WeaponData.Sprite;
            container.Configure(weaponDatas[i]);
        }

        //Objects
        ObjectDataSO[] objectDatas = playerObjects.Objects.ToArray();

        for (int i = 0; i < objectDatas.Length; i++)
        {
            InventoryItemContainer container = Instantiate(inventoryItemsContainer, inventoryItemsParent);

            // Color containerColor = ColorHolder.GetColor(objectDatas[i].Rarity);
            // Color outlineColor = ColorHolder.GetOutlineColor(objectDatas[i].Rarity);
            // Sprite icon = objectDatas[i].Icon;
            container.Configure(objectDatas[i]);
        }
        
    }
}
