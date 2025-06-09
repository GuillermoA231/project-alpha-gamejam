using UnityEngine;

public static class ResourcesManager
{
    const string statIconDataPath = "Data/Stat Icons";

    private static StatIcon[] statIcons;
    public static Sprite GetStatIcon(Stat stat)
    {
        if (statIcons == null)
        {

            StatIconDataSO data = Resources.Load<StatIconDataSO>(statIconDataPath);
            statIcons = data.StatIcons;
        }

        foreach (StatIcon statIcon in statIcons)
        {
            if (stat == statIcon.stat)
                return statIcon.icon;
        }
        Debug.LogError("No Icon found for: " + stat);

        return null;
    }
}
