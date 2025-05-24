
using UnityEngine;
using System;
using TMPro;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{

    [Header("Components")]
    private EnemyMovement movement;

    [Header("Health")]
    [SerializeField] private int maxHealth;
    private int health;

    [Header(" Elements")]
    private Player player;


    [Header("Spawn Sequence")]
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private SpriteRenderer spawnIndicator;
    [SerializeField] private float spawnScaling, spawnScalingTime;
    [SerializeField] private Collider2D collider;
    private bool hasSpawned;

    [Header("Effects")]
    [SerializeField] private ParticleSystem deathParticles;


    [Header("Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float attackFrequency;
    [SerializeField] private float playerDetectionRadius;
    private float attackDelay, attackTimer;


    [Header("Actions")]
    public static Action<int, Vector2> onDamageTaken;


    [Header("DEBUG")]
    [SerializeField] private bool showGizmos;

    //public event Action<GameObject> OnEnemyDied;

    void Start()
    {
        health = maxHealth;

        movement = GetComponent<EnemyMovement>();

        player = FindFirstObjectByType<Player>();

        if (player == null)
        {
            Debug.LogWarning("No player found, Destroying");
            Destroy(gameObject);
        }
        StartSpawnSequence();

        attackDelay = 1f / attackFrequency;
    }

    void Update()
    {

        if (!renderer.enabled)
            return;

        if (attackTimer >= attackDelay)
            TryAttack();
        else
            Wait();

        movement.FollowPlayer();
    }

    private void StartSpawnSequence()
    {
        SetRenderersVisible(false);
        Vector3 targetScale = spawnIndicator.transform.localScale * spawnScaling;
        LeanTween.scale(spawnIndicator.gameObject, targetScale, spawnScalingTime)
                 .setLoopPingPong(3)
                 .setOnComplete(SpawnSequenceCompleted);


    }


    private void SpawnSequenceCompleted()
    {
        SetRenderersVisible(true);
        hasSpawned = true;

        collider.enabled = true;

        movement.StorePlayer(player);

    }


    private void SetRenderersVisible(bool visibility)
    {
        renderer.enabled = visibility;
        spawnIndicator.enabled = !visibility;

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


    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;

        onDamageTaken?.Invoke(damage, transform.position);

        if (health <= 0)
            OnDeath();
    }

    private void Wait()
    {
        attackTimer += Time.deltaTime;
    }


    private void OnDeath()
    {
        deathParticles.transform.SetParent(null);
        deathParticles.Play();
        Destroy(gameObject);

    }

    // public void Die()
    // {
    //     OnEnemyDied?.Invoke(this.gameObject);
    //     Destroy(gameObject);
    // }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);


    }
}
