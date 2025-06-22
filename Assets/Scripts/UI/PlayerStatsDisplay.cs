using UnityEngine;

public class PlayerStatsDisplay : MonoBehaviour, IPlayerStatsDependency
{
    [Header("Elements")]
    [SerializeField] private Transform playerStatContainersParent;

    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        int index = 0;
        foreach (Stat stat in System.Enum.GetValues(typeof(Stat)))
        {
            StatContainer statContainer = playerStatContainersParent.GetChild(index).GetComponent<StatContainer>();

            Sprite statIcon = ResourcesManager.GetStatIcon(stat);

            float statValue = playerStatsManager.GetStatValue(stat);

            statContainer.InitializeStatContainer(statIcon, stat, statValue,true);

            statContainer.gameObject.SetActive(true);

            index++;
        }
        for (int i = index; i < playerStatContainersParent.childCount; i++)
        {
            playerStatContainersParent.GetChild(i).gameObject.SetActive(false);
        }
    }
}
