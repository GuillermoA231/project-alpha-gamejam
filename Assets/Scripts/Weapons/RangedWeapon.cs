
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class RangedWeapon : Weapon
{
    [Header("Elements")]
    [SerializeField] private WeaponBullet weaponBulletPrefab;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header(" Pooling ")]
    [SerializeField] private ObjectPool<WeaponBullet> weaponBulletPool;
    void Start()
    {
        weaponBulletPool = new ObjectPool<WeaponBullet>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    private WeaponBullet CreateFunction()
    {
        WeaponBullet weaponBulletInstance = Instantiate(weaponBulletPrefab, shootingPoint.position, Quaternion.identity);
        weaponBulletInstance.Configure(this);

        return weaponBulletInstance;
    }

    private void ActionOnGet(WeaponBullet weaponBullet)
    {
        weaponBullet.Reload();
        weaponBullet.transform.position = shootingPoint.position;
        weaponBullet.gameObject.SetActive(true);
    }

    private void ActionOnRelease(WeaponBullet weaponBullet)
    {
        weaponBullet.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(WeaponBullet weaponBullet)
    {
        Destroy(weaponBullet.gameObject);
    }

    public void ReleaseBullet(WeaponBullet weaponBullet)
    {
        weaponBulletPool.Release(weaponBullet);
    }


    // Update is called once per frame
    void Update()
    {
        AutoAim();
    }

    private void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy();
        // Vector2 targetUpVector = Vector3.up;

        if (closestEnemy != null)
        {
            // transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);
            // targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            Vector2 shootingDirection = (closestEnemy.transform.position - transform.position).normalized;
            transform.up = Vector2.Lerp(transform.up, shootingDirection, Time.deltaTime * aimLerp);
            ManageShooting();
        }
        // transform.up = targetUpVector;
    }

    private void ManageShooting()
    {

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDelay)
        {
            attackTimer = 0;
            Shoot();
        }
    }


    private void Shoot()
    {
        int damage = GetDamage(out bool isCriticalHit);
        Enemy closestEnemy = GetClosestEnemy();
        Vector2 shootingDirection;

        if (closestEnemy != null)
            shootingDirection = (closestEnemy.transform.position).normalized;

        else
            shootingDirection = transform.up;
        
        WeaponBullet weaponBulletInstance = weaponBulletPool.Get();
        weaponBulletInstance.transform.position = shootingPoint.position;
        weaponBulletInstance.Shoot(damage, transform.up, isCriticalHit);
    }

    public override void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        ConfigureStats();
        damage = Mathf.RoundToInt((int)(damage * (1 + playerStatsManager.GetStatValue(Stat.Attack) / 100)));
        attackDelay /= 1 + (playerStatsManager.GetStatValue(Stat.AttackSpeed) / 100);

        criticalChance = Mathf.RoundToInt(criticalChance * (1 + playerStatsManager.GetStatValue(Stat.CriticalChance) / 100));
        criticalDamage += playerStatsManager.GetStatValue(Stat.CriticalDamage);

        range += playerStatsManager.GetStatValue(Stat.Range) / 10;
    }
}
