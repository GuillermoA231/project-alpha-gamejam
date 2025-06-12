// ===================================
// Author: Andrada Joaquín Guillermo
// Script: DropsManager
// Type: MonoBehaviour
//
// Description:
// Manages pooling and spawning of droppable items such as money, diamonds, and chests upon enemy death.
// Controls drop chances and handles collection callbacks to recycle pooled objects.
//
// Course: Tabsil Unity 2D Game - Kawaii Survivor - The Coolest Roguelike Ever
//
// Date: 28/05/2025
// ===================================
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Pool;
public class DropsManager : MonoBehaviour
{
    [Header(" Elements")]
    [SerializeField] private Money moneyPrefab;
    [SerializeField] private Diamond diamondPrefab;
    [SerializeField] private Chest chestPrefab;


    [Header("Settings")]
    [SerializeField] [Range(0,100)]private int diamondDropChance;
    [SerializeField] [Range(0,100)]private int chestDropChance;

    private Player player;

    [Header(" Pooling ")]
    [SerializeField] ObjectPool<Money> moneyPool;
    [SerializeField] ObjectPool<Diamond> diamondPool;
    [SerializeField] ObjectPool<Chest> chestPool;

    private void Awake()
    {
        // Subscribe when this object becomes active
        Enemy.onDeath += EnemyDeathCallback;
        Money.onCollected += MoneyRelease;
        Diamond.onCollected += DiamondRelease;
        Chest.onCollected += ChestRelease;
    }
    private void OnDestroy()
    {
        // Unsubscribe when this object is destroyed or disabled
        Enemy.onDeath -= EnemyDeathCallback;
        Money.onCollected -= MoneyRelease;
        Diamond.onCollected -= DiamondRelease;
        Chest.onCollected -= ChestRelease;
    }
    void Start()
    {
        moneyPool = new ObjectPool<Money>(MoneyCreateFunction
        , MoneyActionOnGet
        , MoneyActionOnRelease
        , MoneyActionOnDestroy);
        diamondPool = new ObjectPool<Diamond>(DiamondCreateFunction
        , DiamondActionOnGet
        , DiamondActionOnRelease
        , DiamondActionOnDestroy);
        chestPool = new ObjectPool<Chest>(ChestCreateFunction
        , ChestActionOnGet
        , ChestActionOnRelease
        , ChestActionOnDestroy);
    }

    
    

    private Money MoneyCreateFunction()                     => Instantiate(moneyPrefab,transform);
    private void MoneyActionOnGet(Money money)              => money.gameObject.SetActive(true);
    private void MoneyActionOnRelease(Money money)          => money.gameObject.SetActive(false);
    private void MoneyActionOnDestroy(Money money)          => Destroy(money.gameObject);

    private Diamond DiamondCreateFunction()                 => Instantiate(diamondPrefab,transform);
    private void DiamondActionOnGet(Diamond diamond)        => diamond.gameObject.SetActive(true);
    private void DiamondActionOnRelease(Diamond diamond)    => diamond.gameObject.SetActive(false);
    private void DiamondActionOnDestroy(Diamond diamond)    => Destroy(diamond.gameObject);


    private Chest ChestCreateFunction()                 => Instantiate(chestPrefab,transform);
    private void ChestActionOnGet(Chest chest)        => chest.gameObject.SetActive(true);
    private void ChestActionOnRelease(Chest chest)    => chest.gameObject.SetActive(false);
    private void ChestActionOnDestroy(Chest chest)    => Destroy(chest.gameObject);



    private void EnemyDeathCallback(Vector2 enemyPosition)
    {
        bool shouldSpawnDiamond = Random.Range(0, 101) <= diamondDropChance;

        DroppableCurrency droppable = shouldSpawnDiamond ? diamondPool.Get() : moneyPool.Get();
        droppable.transform.position = enemyPosition;

        TryDropChest(enemyPosition);
    }

    private void TryDropChest(Vector2 spawnPosition)
    {
        bool shouldSpawnChest = Random.Range(0, 101) <= chestDropChance;

        if (!shouldSpawnChest)
            return;

        Instantiate(chestPrefab, spawnPosition, Quaternion.identity, transform);

    }

    private void MoneyRelease(Money money)              => moneyPool.Release(money); 
    private void DiamondRelease(Diamond diamond)        => diamondPool.Release(diamond); 
    private void ChestRelease(Chest chest)          => chestPool.Release(chest); 
}
