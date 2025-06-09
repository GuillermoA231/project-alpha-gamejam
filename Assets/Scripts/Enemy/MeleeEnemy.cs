
using UnityEngine;
using System;
using TMPro;
[RequireComponent(typeof(EnemyMovement))]
public class MeleeEnemy : Enemy
{
    [Header("Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float attackFrequency;
    
    private float attackDelay, attackTimer;
    //public event Action<GameObject> OnEnemyDied;

    protected override void Start()
    {
        base.Start();
        attackDelay = 1f / attackFrequency;
    }
    void Update()
    {
        if (!CanAttack())
            return;
        
        if (attackTimer >= attackDelay)
            TryAttack();
        else
            Wait();
        movement.FollowPlayer();
    }
    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= playerDetectionRadius)
            Attack();

    }
    private void Attack()
    {
        attackTimer = 0;
        player.TakeDamage(damage);
    }
    private void Wait()
    {
        attackTimer += Time.deltaTime;
    }
}
