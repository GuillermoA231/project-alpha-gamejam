using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Object Data", menuName = "Scriptable Objects/New Object", order = 0)]
public class ObjectDataSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public int Price { get; private set; }

    [field: Range(0, 3)]
    [field: SerializeField] public int Rarity { get; private set; }

    [SerializeField] private StatData[] statDatas;
    
    
    [HorizontalLine]

    [SerializeField] private float attack;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float criticalChance;
    [SerializeField] private float criticalDamage;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxHealth;
    [SerializeField] private float range;
    [SerializeField] private float healthRegeneration;
    [SerializeField] private float armor;
    [SerializeField] private float luck;
    [SerializeField] private float dodge;
    [SerializeField] private float lifesteal;


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
                    { Stat.     MoveSpeed,                       moveSpeed },
                    { Stat.     MaxHealth,                       maxHealth },
                    { Stat.     Range,                           range },
                    { Stat.     HealthRegeneration,              healthRegeneration },
                    { Stat.     Armor,                           armor },
                    { Stat.     Luck,                            luck },
                    { Stat.     Dodge,                           dodge },
                    { Stat.     Lifesteal,                       lifesteal }
                };
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