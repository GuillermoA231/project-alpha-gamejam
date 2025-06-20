using UnityEngine;

public static class ResourcesManager
{
    const string statIconDataPath = "Data/Stat Icons";
    const string objectDataPath = "Data/Objects";

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


    private static ObjectDataSO[] objectDatas;

    public static ObjectDataSO[] Objects
    {
        get
        {
            if (objectDatas == null)
                objectDatas = Resources.LoadAll<ObjectDataSO>(objectDataPath);

            return objectDatas;
        }
        private set { }
    }
}
