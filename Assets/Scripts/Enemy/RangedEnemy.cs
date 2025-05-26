using UnityEngine;
using System;
using UnityEditor.Rendering;
[RequireComponent(typeof(EnemyMovement), typeof(RangedEnemyAttack))]
public class RangedEnemy : Enemy
{
    private RangedEnemyAttack rangedAttack;
    protected override void Start()
    {
        base.Start();
        rangedAttack = GetComponent<RangedEnemyAttack>();
        rangedAttack.StorePlayer(player);
    }
    void Update()
    {
        if (!CanAttack())
            return;

        ManageAttack();

    }
    private void ManageAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer > playerDetectionRadius)
            movement.FollowPlayer();
        else
            TryAttack();
    }
    private void TryAttack()
    {
        rangedAttack.AutoAim();
    }
}
