using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PlayerStatsManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private CharacterDataSO playerData;
    [Header("Settings")]
    private Dictionary<Stat, float> playerStats = new Dictionary<Stat, float>();
    private Dictionary<Stat, float> addends = new Dictionary<Stat, float>();
    private Dictionary<Stat, float> objectAddends = new Dictionary<Stat, float>();
    private void Awake()
    {
        playerStats = playerData.baseStats;

        foreach (KeyValuePair<Stat, float> kvp in playerStats)
        {
            addends.Add(kvp.Key, 0);
            objectAddends.Add(kvp.Key, 0);
        }
    }

    void Start() => UpdatePlayerStats();

    public void AddPlayerStat(Stat stat, float value)
    {
        if (addends.ContainsKey(stat))
            addends[stat] += value;
        else
            Debug.LogError($"The key {stat} has not been f  ound, check player stats manager");


        UpdatePlayerStats();
    }

    public void AddObject(Dictionary<Stat, float> objectStats)
    {
        Debug.Log("Dictionary Count: " + objectStats.Count);
        foreach (KeyValuePair<Stat, float> kvp in objectStats)
        {
            objectAddends[kvp.Key] += kvp.Value;
            Debug.Log($"Added {kvp.Value} to stat {kvp.Key}");
        }

        UpdatePlayerStats();
    }

    public float GetStatValue(Stat stat) => playerStats[stat] + addends[stat] + objectAddends[stat];
    
    private void UpdatePlayerStats()
    {
        IEnumerable<IPlayerStatsDependency> playerStatsDependencies =
        FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
        .OfType<IPlayerStatsDependency>();

        foreach (IPlayerStatsDependency dependency in playerStatsDependencies)
            dependency.UpdateStats(this);
    }

}
