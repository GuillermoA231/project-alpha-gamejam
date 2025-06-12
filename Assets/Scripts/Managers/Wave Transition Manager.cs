using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;
using NaughtyAttributes;
using Unity.VisualScripting;

public class WaveTransitionManager : MonoBehaviour, IGameStateListener
{
    [Header(" Elements")]
    [SerializeField] private PlayerStatsManager playerStatsManager;
    [SerializeField] private UpgradeContainer[] upgradeContainers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.WAVETRANSITION:
                ConfigureUpgradeContainers();
                break;
        }
    }

    [Button]
    private void ConfigureUpgradeContainers()
    {
        for (int i = 0; i < upgradeContainers.Length; i++)
        {
            int randomIndex = Random.Range(0, Enum.GetValues(typeof(Stat)).Length);
            Stat stat = (Stat)Enum.GetValues(typeof(Stat)).GetValue(randomIndex);

            Sprite upgradeIcon = ResourcesManager.GetStatIcon(stat);

            string randomStatString = Enums.FormatStatName(stat);

            string buttonString;
            Action action = GetActionToPerform(stat, out buttonString);

            upgradeContainers[i].Configure(upgradeIcon, randomStatString, buttonString);


            upgradeContainers[i].Button.onClick.RemoveAllListeners();
            upgradeContainers[i].Button.onClick.AddListener(() => action?.Invoke());
            upgradeContainers[i].Button.onClick.AddListener(() => BonusSelectedCallback());
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
                value = Random.Range(5, 18);
                buttonString = "+" + value.ToString() + "%";
                break;
            case Stat.AttackSpeed:
                value = Random.Range(5, 25);
                buttonString = "+" + value.ToString() + "%";
                break;
            case Stat.CriticalChance:
                value = Random.Range(5, 17);
                buttonString = "+" + value.ToString() + "%";
                break;
            case Stat.CriticalDamage:
                value = Random.Range(1.4f, 2.4f);
                buttonString = "+" + value.ToString("F1") + "x";
                break;
            case Stat.MoveSpeed:
                value = Random.Range(10, 30);
                buttonString = "+" + value.ToString();
                break;
            case Stat.MaxHealth:
                value = Random.Range(5, 15);
                buttonString = "+" + value.ToString();
                break;
            case Stat.Range:
                value = Random.Range(1, 30);
                buttonString = "+" + value.ToString() + " (GUN ONLY)";
                break;
            case Stat.HealthRegeneration:
                value = Random.Range(1, 24);
                buttonString = "+" + value.ToString() + "%";
                break;
            case Stat.Armor:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString();
                break;
            case Stat.Luck:
                value = Random.Range(1, 40);
                buttonString = "[DONT TOUCH] +" + value.ToString();
                break;
            case Stat.Dodge:
                value = Random.Range(1, 24);
                buttonString = "+" + value.ToString() + "%";
                break;
            case Stat.Lifesteal:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;

            default:
                return () => Debug.Log("Invalid Stat");

        }
        return () => playerStatsManager.AddPlayerStat(stat, value);
    }
}

