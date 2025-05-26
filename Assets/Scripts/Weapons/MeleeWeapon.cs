using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    enum State
    {
        Idle,
        Attack
    }

    private State state;

    [Header(" Elements ")]
    [SerializeField] private Transform hitDetectionTransform;
    [SerializeField] private BoxCollider2D hitCollider;

    [Header(" Settings ")]
    private List<Enemy> damagedEnemies = new List<Enemy>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                AutoAim();
                break;

            case State.Attack:
                Attacking();
                break;
        }

    }

    private void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy();
        Vector2 targetUpVector = Vector3.up;

        if (closestEnemy != null)
        {
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            transform.up = targetUpVector;
            ManageAttack();
        }

        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);
        IncrementAttackTimer();
    }
    private void ManageAttack()
    {
        if (attackTimer >= attackDelay)
        {
            attackTimer = 0;
            StartAttack();
        }
    }

    private void IncrementAttackTimer()
    {
        attackTimer += Time.deltaTime;
    }


    [NaughtyAttributes.Button]
    private void StartAttack()
    {
        animator.Play("Attack");
        state = State.Attack;

        damagedEnemies.Clear();

        animator.speed = 1f / attackDelay;
    }

    private void Attacking()
    {
        Attack();
    }

    private void StopAttack()
    {
        state = State.Idle;

        //Clear the attacked enemies
        // Damaged enemies list
        damagedEnemies.Clear();
    }

    private void Attack()
    {
        //Collider2D[] enemies = Physics2D.OverlapCircleAll(hitDetectionTransform.position, hitDetectionRadius, enemyMask);
        Collider2D[] enemies = Physics2D.OverlapBoxAll
        (
            hitDetectionTransform.position
            , hitCollider.bounds.size
            , hitDetectionTransform.localEulerAngles.z
            , enemyMask
            );

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemy = enemies[i].GetComponent<Enemy>();
            // 1. Is the enemy inside of the list ?
            if (!damagedEnemies.Contains(enemy))
            {
                int damage = GetDamage(out bool isCriticalHit);

                enemy.TakeDamage(damage, isCriticalHit);
                damagedEnemies.Add(enemy);
            }
        }

    }

}
