using UnityEngine;
using UnityEngine.Pool;

public class RangedWeaponAttack : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private WeaponBullet bulletPrefab;
    private ObjectPool<WeaponBullet> weaponBulletPool;
    private Player player;


    [Header("Attack Settings")]

    [SerializeField] private int damage;
    [SerializeField] private float attackFrequency;
    private float attackDelay, attackTimer;



    Vector2 gizmosDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackDelay = 1f / attackFrequency;
        attackTimer = attackDelay;

        // weaponBulletPool = new ObjectPool<WeaponBullet>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    public void AutoAim()
    {
        ManageShooting();
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
        Vector2 direction = (player.GetShootPosition() - (Vector2)shootingPoint.position).normalized;

        WeaponBullet bulletInstance = weaponBulletPool.Get();

        bulletInstance.Shoot(damage, direction);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(shootingPoint.position, (Vector2)shootingPoint.position + gizmosDirection * 5);
    }
    
}
