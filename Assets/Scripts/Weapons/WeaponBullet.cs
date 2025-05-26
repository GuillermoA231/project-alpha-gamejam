using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class WeaponBullet : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody2D rigidbody2D;
    private Collider2D collider2D;
    private RangedWeapon rangedWeapon;
    


    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask enemyMask;
    private int damage;


    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();


        //Alternative option with LeanTween if I don't want to use Coroutines
        // LeanTween.delayedCall(gameObject, 5, () => RangedWeaponAttack.ReleaseBullet(this));
        StartCoroutine(ReleaseCoroutine());

    }

    public void Configure(RangedWeapon rangedWeapon)
    {
        this.rangedWeapon = rangedWeapon;
    }

    IEnumerator ReleaseCoroutine()
    {
        yield return new WaitForSeconds(5);

        //RangedWeaponAttack.ReleaseBullet(this);
    }

    public void Reload()
    {
        rigidbody2D.linearVelocity = Vector2.zero;
        collider2D.enabled = true;
    }
    public void Shoot(int damage, Vector2 direction)
    {
        Invoke("Release", 1);

        this.damage = damage;

        transform.right = direction;
        rigidbody2D.linearVelocity = direction * moveSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (IfIsInLayerMask(collider.gameObject.layer, enemyMask))
        {
            CancelInvoke();
            Attack(collider.GetComponent<Enemy>());
            Release();
        }
    }

    private void Release()
    {
        if(!gameObject.activeSelf)
            return;
        rangedWeapon.ReleaseBullet(this);
    }

    private void Attack(Enemy enemy)
    {
        enemy.TakeDamage(damage);
    }

    private bool IfIsInLayerMask(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) != 0;
    }

}
