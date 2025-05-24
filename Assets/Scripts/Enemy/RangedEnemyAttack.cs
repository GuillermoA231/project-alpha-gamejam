using UnityEngine;

public class RangedEnemyAttack : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject bulletPrefab;
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
    }

    void Update()
    {

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
        
        gizmosDirection = direction;

        GameObject bulletInstance = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        bulletInstance.transform.right = direction;

        bulletInstance.GetComponent<Rigidbody2D>().linearVelocity = direction * 5;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(shootingPoint.position, (Vector2)shootingPoint.position + gizmosDirection *5);
    }
}
