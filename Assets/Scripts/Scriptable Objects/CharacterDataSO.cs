using Unity.VisualScripting;
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Character Data", menuName = "Scriptable Objects/New Character Data", order = 0)]
public class CharacterDataSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public int PurchasePrice { get; private set; }

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
