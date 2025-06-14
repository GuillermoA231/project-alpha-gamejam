using System;
using UnityEngine;
using UnityEngine.Localization.Settings;
using NaughtyAttributes;

public class WaveTransitionManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private PlayerStatsManager playerStatsManager;
    [SerializeField] private UpgradeContainer[] upgradeContainers;

    public void GameStateChangedCallback(GameState gameState)
    {
        if (gameState == GameState.WAVETRANSITION)
            ConfigureUpgradeContainers();
    }

    [Button]
    private void ConfigureUpgradeContainers()
    {
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
                value = UnityEngine.Random.Range(5, 18);
                buttonString = "+" + value.ToString("F0") + "%";
                break;
            case Stat.AttackSpeed:
                value = UnityEngine.Random.Range(5, 25);
                buttonString = "+" + value.ToString("F0") + "%";
                break;
            case Stat.CriticalChance:
                value = UnityEngine.Random.Range(5, 17);
                buttonString = "+" + value.ToString("F0") + "%";
                break;
            case Stat.CriticalDamage:
                value = UnityEngine.Random.Range(1.4f, 2.4f);
                buttonString = "+" + value.ToString("F1") + "x";
                break;
            case Stat.MoveSpeed:
                value = UnityEngine.Random.Range(10, 30);
                buttonString = "+" + value.ToString("F0");
                break;
            case Stat.MaxHealth:
                value = UnityEngine.Random.Range(5, 15);
                buttonString = "+" + value.ToString("F0");
                break;
            case Stat.Range:
                value = UnityEngine.Random.Range(1, 30);
                buttonString = "+" + value.ToString("F0") + " (GUN ONLY)";
                break;
            case Stat.HealthRegeneration:
                value = UnityEngine.Random.Range(1, 24);
                buttonString = "+" + value.ToString("F0") + "%";
                break;
            case Stat.Armor:
                value = UnityEngine.Random.Range(1, 10);
                buttonString = "+" + value.ToString("F0");
                break;
            case Stat.Luck:
                value = UnityEngine.Random.Range(1, 40);
                buttonString = "[DONT TOUCH] +" + value.ToString("F0");
                break;
            case Stat.Dodge:
                value = UnityEngine.Random.Range(1, 24);
                buttonString = "+" + value.ToString("F0") + "%";
                break;
            case Stat.Lifesteal:
                value = UnityEngine.Random.Range(1, 10);
                buttonString = "+" + value.ToString("F0") + "%";
                break;
            default:
                return () => Debug.Log("Invalid Stat");
        }
        return () => playerStatsManager.AddPlayerStat(stat, value);
    }
}
