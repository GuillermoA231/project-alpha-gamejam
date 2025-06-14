using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StatContainerManager : MonoBehaviour
{
    public static StatContainerManager instance;
    [Header("Elements")]
    [SerializeField] private StatContainer statContainer;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void GenerateContainers(Dictionary<Stat, float> keyValuePairs, Transform parent)
    {
        List<StatContainer> statContainers = new List<StatContainer>();
        foreach (KeyValuePair<Stat, float> kvp in keyValuePairs)
        {
            StatContainer containerInstance = Instantiate(statContainer, parent);
            Sprite icon = ResourcesManager.GetStatIcon(kvp.Key);
            string statName = Enums.FormatStatName(kvp.Key);
            string statValue = kvp.Value.ToString("F0");
            containerInstance.InitializeStatContainer(icon, kvp.Key, kvp.Value);
        }

        LeanTween.delayedCall(Time.deltaTime * 3, () => ResizeText(statContainers));
    }

    private void ResizeText(List<StatContainer> statContainers)
    {
        float minFontSize = 10000;

        for (int i = 0; i < statContainers.Count; i++)
        {
            StatContainer statContainer = statContainers[i];
            float fontSize = statContainer.GetFontSize();

            if (fontSize < minFontSize)
                minFontSize = fontSize;
        }

        for (int i = 0; i < statContainers.Count; i++)
            statContainers[i].SetFontSize(minFontSize);
    }

    public static void GenerateStatContainers(Dictionary<Stat, float> keyValuePairs, Transform parent)
    {
        instance.GenerateContainers(keyValuePairs, parent);
    }

}
