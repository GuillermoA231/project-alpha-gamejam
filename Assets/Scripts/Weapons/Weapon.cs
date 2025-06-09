
using System.Collections.Generic;
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
    [field: SerializeField] public int Level { get; private set; }
    [Header("Audio")]
    private AudioSource audioSource;


    [Header("DEBUG")]
    [SerializeField] private bool showGizmos;


    protected void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = WeaponData.AttackSound;
    }

    protected void PlayAttackSound()
    {
        audioSource.pitch = Random.Range(.95f, 1.05f);
        audioSource.Play();
    }
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
        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(WeaponData, Level);

        damage                  = Mathf.RoundToInt(calculatedStats[Stat.Attack]);
        attackDelay             = Mathf.Max(0.08f, 1f/ calculatedStats[Stat.AttackSpeed]);
        criticalChance          = Mathf.RoundToInt(calculatedStats[Stat.CriticalChance]);
        criticalDamage          = calculatedStats[Stat.CriticalDamage];
        range                   = calculatedStats[Stat.Range];
    }
    public void UpgradeTo(int level)
    {
        Level = level;
        ConfigureStats();
    }
}