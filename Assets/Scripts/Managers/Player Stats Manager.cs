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
    private void Awake()
    {
        playerStats = playerData.baseStats;
        foreach (KeyValuePair<Stat, float> kvp in playerStats)
            addends.Add(kvp.Key, 0);
    }
    void Start()
    {

        UpdatePlayerStats();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddPlayerStat(Stat stat, float value)
    {
        if (addends.ContainsKey(stat))
            addends[stat] += value;
        else
            Debug.LogError($"The key {stat} has not been f  ound, check player stats manager");


        UpdatePlayerStats();
    }

    public float GetStatValue(Stat stat)
    {
        float value = playerStats[stat] + addends[stat];
        return value;
    }
    private void UpdatePlayerStats()
    {

        IEnumerable<IPlayerStatsDependency> playerStatsDependencies =
        FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
        .OfType<IPlayerStatsDependency>();

        foreach (IPlayerStatsDependency dependency in playerStatsDependencies)
            dependency.UpdateStats(this);
    }

}
