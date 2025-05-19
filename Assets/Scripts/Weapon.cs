using System;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Transform enemy;

    [Header("DEBUG")]
    [SerializeField] private LayerMask enemyMask;

    [Header("DEBUG")]
    [SerializeField] private bool showGizmos;
    [SerializeField] private float range;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Enemy closestEnemy = null;

        //Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyMask);

        float minDistance = 5000;

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemyChecked = enemies[i];

            float distanceEnemy = Vector2.Distance(transform.position, enemyChecked.transform.position);
            if (distanceEnemy < minDistance)
            {
                closestEnemy = enemyChecked;
                minDistance = distanceEnemy;
            }

        }

        if (closestEnemy == null)
        {
            transform.up = Vector3.up;
            return;
        }

        transform.up = (closestEnemy.transform.position - transform.forward).normalized;
    }
    
    
    private void OnDrawGizmosSelected()
    {
        if (!showGizmos)
            return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
