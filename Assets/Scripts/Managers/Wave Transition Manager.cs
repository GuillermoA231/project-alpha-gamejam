using System;
using UnityEngine;
using UnityEngine.Localization.Settings;
using NaughtyAttributes;
using Random = UnityEngine.Random;
using Unity.VisualScripting;
using Object = UnityEngine.Object;

public class WaveTransitionManager : MonoBehaviour, IGameStateListener
{
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

    public void Awake()
    {
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
                break;
        }
    }

    private void TryOpenChest()
    {
        chestContainerParent.Clear();

        if (chestsCollected > 0)
        {
            ShowObject();
        }
        else
            ConfigureUpgradeContainers();
    }

    private void ShowObject()
    {
 
        chestsCollected--;

        upgradeContainersParent.SetActive(false);

        ObjectDataSO[] objectDatas = ResourcesManager.Objects;
        ObjectDataSO randomObjectData = objectDatas[Random.Range(0, objectDatas.Length)];

        ChestObjectContainer containerInstance = Instantiate(chestContainerPrefab, chestContainerParent);
        containerInstance.Configure(randomObjectData, randomObjectData.Rarity);


        containerInstance.InstallButton.onClick.AddListener(() => InstallButtonCallback(randomObjectData));
    }
    

    private void InstallButtonCallback(ObjectDataSO objectToTake)
    {
        playerObjects.AddObject(objectToTake);
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
                buttonString = "[DONT TOUCH] +" + value.ToString("F0");
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
}
