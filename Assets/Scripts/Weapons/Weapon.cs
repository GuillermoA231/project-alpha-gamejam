
using System.Runtime.InteropServices;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IPlayerStatsDependency
{

    [field: SerializeField] public WeaponDataSO WeaponData { get; private set; }
    [Header("Settings")]
    [SerializeField] protected float range;
    [SerializeField] protected LayerMask enemyMask;
    [Header("Attack")]
    [SerializeField] protected int baseDamage;
    protected int damage;
    [SerializeField] protected float attackDelay;
    [SerializeField] protected Animator animator;
    protected float attackTimer;
    [Header("Critical Attack")]
    protected int criticalChance;
    protected float criticalDamage;

    [Header("Animations")]
    [SerializeField] protected float aimLerp;
    [Header("Level")]
    [field: SerializeField]public int Level { get; private set; }


    [Header("DEBUG")]
    [SerializeField] private bool showGizmos;

    
    protected Enemy GetClosestEnemy()
    {
        Enemy closestEnemy = null;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyMask);

        if (enemies.Length <= 0)
            return null;

        float minDistance = range;

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemyChecked = enemies[i].GetComponent<Enemy>();

            float distanceEnemy = Vector2.Distance(transform.position, enemyChecked.transform.position);
            if (distanceEnemy < minDistance)
            {
                closestEnemy = enemyChecked;
                minDistance = distanceEnemy;
            }
        }
        return closestEnemy;
    }


    protected int GetDamage(out bool isCriticalHit)
    {
        isCriticalHit = false;

        if (Random.Range(0, 101) <= criticalChance)
        {
            isCriticalHit = true;
            return Mathf.RoundToInt(damage * criticalDamage);
        }

        return damage;
    }

    private void OnDrawGizmosSelected()
    {
        if (!showGizmos)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public abstract void UpdateStats(PlayerStatsManager playerStatsManager);

    protected void ConfigureStats()
    {
        float multiplier = 1 + (float)Level / 3;
        damage = Mathf.RoundToInt(WeaponData.GetStatValue(Stat.Attack) * multiplier);
        attackDelay = Mathf.Max(0.08f, 1f / (WeaponData.GetStatValue(Stat.AttackSpeed) * multiplier));


        criticalChance = Mathf.RoundToInt(WeaponData.GetStatValue(Stat.CriticalChance) * multiplier);
        criticalDamage = WeaponData.GetStatValue(Stat.CriticalDamage) * multiplier;


        if(WeaponData.Prefab.GetType() == typeof(RangedWeapon))
            range = WeaponData.GetStatValue(Stat.Range) * multiplier;
    }
    public void UpgradeTo(int level)
    {
        Level = level;
        ConfigureStats();
    }
}