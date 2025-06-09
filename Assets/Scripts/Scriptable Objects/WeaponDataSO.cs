using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.UIElements.Experimental;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Scriptable Objects/New Weapon Data", order = 0)]
public class WeaponDataSO : ScriptableObject
{

    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public int PurchasePrice { get; private set; }
    [field: SerializeField] public AudioClip AttackSound { get; private set; }
    [field: SerializeField] public Weapon Prefab { get; private set; }


    [HorizontalLine]
    [SerializeField] private float attack;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float criticalChance;
    [SerializeField] private float criticalDamage;
    [SerializeField] private float range;

    public Dictionary<Stat, float> baseStats
    {
        get
        {
            return new Dictionary<Stat, float>
                {
                    { Stat.     Attack,                          attack },
                    { Stat.     AttackSpeed,                     attackSpeed },
                    { Stat.     CriticalChance,                  criticalChance },
                    { Stat.     CriticalDamage,                  criticalDamage },
                    { Stat.     Range,                           range }
                };
        }
        private set
        {

        }
    }

    public float GetStatValue(Stat stat)
    {
        foreach (KeyValuePair<Stat, float> kvp in baseStats)
            if (kvp.Key == stat)
                return kvp.Value;
        Debug.Log("Stat not found, Check weapon data SO");
        return 0;

    }


}
