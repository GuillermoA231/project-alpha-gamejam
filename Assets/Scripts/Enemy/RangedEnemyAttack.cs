using UnityEngine;
using UnityEngine.Pool;

public class RangedEnemyAttack : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private EnemyBullet bulletPrefab;
    private ObjectPool<EnemyBullet> enemyBulletPool;
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

        enemyBulletPool = new ObjectPool<EnemyBullet>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    private EnemyBullet CreateFunction()
    {
        Transform parent = BulletManager.Instance.Container;
        EnemyBullet bulletInstance = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity, parent);
        bulletInstance.Configure(this);

        return bulletInstance;
    }

    private void ActionOnGet(EnemyBullet enemyBullet)
    {
        enemyBullet.Reload();
        enemyBullet.transform.position  = shootingPoint.position;
        enemyBullet.gameObject.SetActive(true);
    }

    private void ActionOnRelease(EnemyBullet enemyBullet)
    {
        enemyBullet.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(EnemyBullet enemyBullet)
    {
        Destroy(enemyBullet.gameObject);
    }

    public void ReleaseBullet(EnemyBullet enemyBullet)
    {
        enemyBulletPool.Release(enemyBullet); 
    }

    public void StorePlayer(Player player)
    {
        this.player = player;
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

        EnemyBullet bulletInstance = enemyBulletPool.Get();

        bulletInstance.Shoot(damage, direction);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(shootingPoint.position, (Vector2)shootingPoint.position + gizmosDirection * 5);
    }
}
