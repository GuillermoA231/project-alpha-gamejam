using UnityEngine;
using System;


[RequireComponent(typeof(EnemyMovement), typeof(RangedEnemyAttack))]
public class RangedEnemy : MonoBehaviour
{

    [Header("Components")]
    private EnemyMovement movement;
    private RangedEnemyAttack rangedAttack;

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
    [SerializeField] private float playerDetectionRadius;


    [Header("Actions")]
    public static Action<int, Vector2> onDamageTaken;

    [Header("DEBUG")]
    [SerializeField] private bool showGizmos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;

        movement = GetComponent<EnemyMovement>();
        rangedAttack = GetComponent<RangedEnemyAttack>();

        player = FindFirstObjectByType<Player>();

        rangedAttack.StorePlayer(player);

        if (player == null)
        {
            Debug.LogWarning("No player found, Destroying");
            Destroy(gameObject);
        }
        StartSpawnSequence();

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



    void Update()
    {
        if (!renderer.enabled)
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


    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;

        onDamageTaken?.Invoke(damage, transform.position);

        if (health <= 0)
            OnDeath();
    }


    private void OnDeath()
    {
        deathParticles.transform.SetParent(null);
        deathParticles.Play();
        Destroy(gameObject);

    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);

    }


}
