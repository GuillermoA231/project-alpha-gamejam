using System;
using UnityEngine;
using UnityEngine.Localization.Settings;
using NaughtyAttributes;
using Random = UnityEngine.Random;
using Unity.VisualScripting;
using Object = UnityEngine.Object;
using System.Linq;

public class WaveTransitionManager : MonoBehaviour, IGameStateListener
{

    public static WaveTransitionManager instance;

    [Header("Player")]
    [SerializeField] private PlayerObjects playerObjects;

    [Header("Elements")]
    [SerializeField] private PlayerStatsManager playerStatsManager;
    [SerializeField] private UpgradeContainer[] upgradeContainers;
    [SerializeField] private GameObject upgradeContainersParent;

    [Header(" Settings")]
    private int chestsCollected;

    [Header("Chest Related Stuff")]
    [SerializeField] private ChestObjectContainer chestContainerPrefab;
    [SerializeField] private Transform chestContainerParent;

    [SerializeField] private float[] chestRarityWeights = new float[] { 50f, 24f, 15f, 11f };

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Chest.onCollected += ChestCollectedCallback;
    }
    public void OnDestroy()
    {
        Chest.onCollected -= ChestCollectedCallback;
    }


    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {

            case GameState.WAVETRANSITION:
                TryOpenChest();
                CurrencyManager.instance.UpdateCurrencyText();
                break;
        }
    }

    private void TryOpenChest()
    {
        chestContainerParent.Clear();

        bool canLevelUp = Player.instance.GetComponent<PlayerEXP>().CanLevelUpThisWave();

        if (chestsCollected > 0)
        {
            ShowObject();
            return;
        }
        else if (canLevelUp)
        {
            ConfigureUpgradeContainers();
            return;
        }
        else
        {
            GameManager.instance.SetGameState(GameState.SHOP);
            return;
        }
    }

    private void ShowObject()
    {
        // Old version, no weighted random
        // chestsCollected--;

        // upgradeContainersParent.SetActive(false);

        // ObjectDataSO[] objectDatas = ResourcesManager.Objects;

        // ObjectDataSO randomObjectData = objectDatas[Random.Range(0, objectDatas.Length)];

        // ChestObjectContainer containerInstance = Instantiate(chestContainerPrefab, chestContainerParent);
        // containerInstance.Configure(randomObjectData, randomObjectData.Rarity);

        // containerInstance.SetupButtonCallbacks(
        // () => InstallButtonCallback(randomObjectData),
        // () => MeltButtonCallback(randomObjectData)
        // );
        // ---------

        // new Version with Weighted Randoms

        chestsCollected--;
        upgradeContainersParent.SetActive(false);

        // Retrieve all possible objectDatas
        ObjectDataSO[] objectDatas = ResourcesManager.Objects;

        // 1) Determine rarity index via weights
        int rarityIndex = WeightedRandom.GetIndexByWeight(chestRarityWeights);
        int targetRarity = rarityIndex;

        // 2) Filter objects by that rarity
        var objectCandidates = objectDatas.Where(o => o.Rarity == targetRarity).ToArray();
        if (objectCandidates.Length == 0)
        {
            Debug.LogWarning($"No objects found for rarity {targetRarity}, falling back to random pick.");
            objectCandidates = objectDatas;
        }

        // 3) Pick one at random from objectCandidates
        ObjectDataSO chosenData = objectCandidates[Random.Range(0, objectCandidates.Length)];

        // 4) Instantiate and configure container
        ChestObjectContainer containerInstance = Instantiate(chestContainerPrefab, chestContainerParent);
        containerInstance.Configure(chosenData, rarityIndex);
        containerInstance.SetupButtonCallbacks(
            () => InstallButtonCallback(chosenData),
            () => MeltButtonCallback(chosenData)
        );


    }


    private void InstallButtonCallback(ObjectDataSO objectToTake)
    {
        playerObjects.AddObject(objectToTake);
        TryOpenChest();
    }
    private void MeltButtonCallback(ObjectDataSO objectToMelt)
    {
        CurrencyManager.instance.AddCurrency(objectToMelt.MeltPrice);
        TryOpenChest();
    }


    [Button]
    private void ConfigureUpgradeContainers()
    {
        upgradeContainersParent.SetActive(true);
        var stats = Enum.GetValues(typeof(Stat));
        for (int i = 0; i < upgradeContainers.Length; i++)
        {
            Stat stat = (Stat)stats.GetValue(UnityEngine.Random.Range(0, stats.Length));
            Sprite upgradeIcon = ResourcesManager.GetStatIcon(stat);

            string buttonString;
            Action action = GetActionToPerform(stat, out buttonString);

            upgradeContainers[i].Configure(upgradeIcon, stat, buttonString);

            var btn = upgradeContainers[i].Button;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => action?.Invoke());
            btn.onClick.AddListener(BonusSelectedCallback);
        }
    }

    private void BonusSelectedCallback()
    {
        Player.instance.GetComponent<PlayerEXP>().ConsumeLevelThisWave();
        GameManager.instance.WaveCompletedCallback();
    }

    private Action GetActionToPerform(Stat stat, out string buttonString)
    {
        buttonString = "";
        float value;
        switch (stat)
        {
            case Stat.Attack:
                value = UnityEngine.Random.Range(5, 12);
                buttonString = "+" + value.ToString("F0") + "%";
                break;
            case Stat.AttackSpeed:
                value = UnityEngine.Random.Range(5, 15);
                buttonString = "+" + value.ToString("F0") + "%";
                break;
            case Stat.CriticalChance:
                value = UnityEngine.Random.Range(5, 15);
                buttonString = "+" + value.ToString("F0") + "%";
                break;
            case Stat.CriticalDamage:
                value = UnityEngine.Random.Range(1.1f, 1.4f);
                buttonString = "+" + value.ToString("F1") + "x";
                break;
            case Stat.MoveSpeed:
                value = UnityEngine.Random.Range(5, 15);
                buttonString = "+" + value.ToString("F0");
                break;
            case Stat.MaxHealth:
                value = UnityEngine.Random.Range(5, 15);
                buttonString = "+" + value.ToString("F0");
                break;
            case Stat.Range:
                value = UnityEngine.Random.Range(1, 15);
                buttonString = "+" + value.ToString("F0") + " (GUN ONLY)";
                break;
            case Stat.HealthRegeneration:
                value = UnityEngine.Random.Range(1, 20);
                buttonString = "+" + value.ToString("F0") + "%";
                break;
            case Stat.Armor:
                value = UnityEngine.Random.Range(1, 12);
                buttonString = "+" + value.ToString("F0");
                break;
            case Stat.Luck:
                value = UnityEngine.Random.Range(1, 20);
                buttonString = "+" + value.ToString("F0");
                break;
            case Stat.Dodge:
                value = UnityEngine.Random.Range(1, 15);
                buttonString = "+" + value.ToString("F0") + "%";
                break;
            case Stat.Lifesteal:
                value = UnityEngine.Random.Range(1, 7);
                buttonString = "+" + value.ToString("F0") + "%";
                break;
            default:
                return () => Debug.Log("Invalid Stat");
        }
        return () => playerStatsManager.AddPlayerStat(stat, value);
    }


    private void ChestCollectedCallback(Chest chest)
    {
        if (chest.GetType() != typeof(Chest))
        {
            chestsCollected++;
            return;
        }

        chestsCollected++;
        Debug.Log($"Chest collected: {chestsCollected}");
    }

    public bool HasCollectedChest()
    {
        return chestsCollected > 0;
    }
}
