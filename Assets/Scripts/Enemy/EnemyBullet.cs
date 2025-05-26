using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;



[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class EnemyBullet : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody2D rigidbody2D;
    private Collider2D collider2D;
    private RangedEnemyAttack RangedEnemyAttack;


    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    private int damage;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();

        
        //Alternative option with LeanTween if I don't want to use Coroutines
        // LeanTween.delayedCall(gameObject, 5, () => RangedEnemyAttack.ReleaseBullet(this));
        StartCoroutine(ReleaseCoroutine());

    }

    IEnumerator ReleaseCoroutine()
    {
        yield return new WaitForSeconds(5);

        RangedEnemyAttack.ReleaseBullet(this);
    }

    public void Configure(RangedEnemyAttack rangedEnemyAttack)
    {
        this.RangedEnemyAttack = rangedEnemyAttack;
    }

    public void Shoot(int damage, Vector2 direction)
    {
        this.damage = damage;

        transform.right = direction;
        rigidbody2D.linearVelocity = direction * moveSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            //LeanTween.cancel(gameObject);
            StopCoroutine(ReleaseCoroutine());

            StopAllCoroutines();

            player.TakeDamage(damage);
            collider2D.enabled = false;

            RangedEnemyAttack.ReleaseBullet(this);
        }
    }

    public void Reload()
    {
        rigidbody2D.linearVelocity = Vector2.zero;
        collider2D.enabled = true;
    }
}