using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class RangedWeapon : Weapon
{
    [Header("Elements")]
    [SerializeField] private WeaponBullet weaponBulletPrefab;
    [SerializeField] private Transform shootingPoint;
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
        weaponBullet.transform.position  = shootingPoint.position;
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
        Vector2 targetUpVector = Vector3.up;

        if (closestEnemy != null)
        {
            transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            ManageShooting();
        }
        transform.up = targetUpVector;
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
        WeaponBullet weaponBulletInstance = weaponBulletPool.Get();
        weaponBulletInstance.Shoot(damage, transform.up);
    }
}
