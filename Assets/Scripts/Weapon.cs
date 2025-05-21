using System;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    enum State
    {
        Idle,
        Attack
    }

    private State state;

    [Header(" Elements ")]
    [SerializeField] private Transform hitDetectionTransform;
    [SerializeField] private float hitDetectionRadius;


    [Header("Settings")]
    [SerializeField] private float range;
    [SerializeField] private LayerMask enemyMask;
    [Header("Attack")]
    [SerializeField] private int damage;

    [Header("Animations")]
    [SerializeField] private float aimLerp;
    [SerializeField] private Animator animator;

    [Header("DEBUG")]
    [SerializeField] private bool showGizmos;


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
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;

        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);
    }


    [NaughtyAttributes.Button]
    private void StartAttack()
    {
        animator.Play("Attack");
        state = State.Attack;
    }

    private void Attacking()
    {
        Attack();
    }

    private void StopAttack()
    {
        state = State.Idle;
    }

    private void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(hitDetectionTransform.position, hitDetectionRadius, enemyMask);

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<Enemy>().TakeDamage(damage);
        }

    }

    private Enemy GetClosestEnemy()
    {


        Enemy closestEnemy = null;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyMask);

        if (enemies.Length <= 0)
            return null;

        float minDistance = range;

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemyChecked = enemies[i].GetComponent<Enemy>();

            float distanceEnemy = Vector2.Distance(transform.position, enemyChecked.transform.position);
            if (distanceEnemy < minDistance)
            {
                closestEnemy = enemyChecked;
                minDistance = distanceEnemy;
            }

        }

        return closestEnemy;
    }


    private void OnDrawGizmosSelected()
    {
        if (!showGizmos)
            return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, hitDetectionRadius);
    }
}
