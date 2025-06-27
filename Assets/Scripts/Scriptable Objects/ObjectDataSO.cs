using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Object Data", menuName = "Scriptable Objects/New Object", order = 0)]
public class ObjectDataSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public int Price { get; private set; }
    [field: SerializeField] public int MeltPrice { get; private set; }

    [field: Range(0, 3)]
    [field: SerializeField] public int Rarity { get; private set; }

    [SerializeField] private StatData[] statDatas;
    
    public Dictionary<Stat, float> baseStats
    {
        get
        {
            Dictionary<Stat,float> stats = new Dictionary<Stat,float>();

            foreach (StatData data in statDatas)
            {
                stats.Add(data.stat,data.value);
            }
            return stats;
        }
        private set
        {

        }
    }
}

[System.Serializable]
public struct StatData
{
    public Stat stat;
    public float value;
}