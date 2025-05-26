using UnityEngine;
using System;

public abstract class Enemy : MonoBehaviour
{

    [Header("Components")]
    protected EnemyMovement movement;

    [Header("Health")]
    [SerializeField] protected int maxHealth;
    protected int health;

    [Header(" Elements")]
    protected Player player;

    [Header("Spawn Sequence")]
    [SerializeField] protected SpriteRenderer renderer;
    [SerializeField] protected SpriteRenderer spawnIndicator;
    [SerializeField] protected float spawnScaling, spawnScalingTime;
    [SerializeField] protected Collider2D collider;
    protected bool hasSpawned;

    [Header("Effects")]
    [SerializeField] protected ParticleSystem deathParticles;

    [Header("Attack")]
    [SerializeField] protected float playerDetectionRadius;

    
    [Header("Rotation")]
    [SerializeField] private float rotationOffset = 0f;


    [Header("Actions")]
    public static Action<int, Vector2, bool> onDamageTaken;
    public static Action<Vector2> onDeath;


    [Header("DEBUG")]
    [SerializeField] protected bool showGizmos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
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


    }

    protected void StartSpawnSequence()
    {
        SetRenderersVisible(false);
        Vector3 targetScale = spawnIndicator.transform.localScale * spawnScaling;
        LeanTween.scale(spawnIndicator.gameObject, targetScale, spawnScalingTime)
                 .setLoopPingPong(3)
                 .setOnComplete(SpawnSequenceCompleted);


    }


    protected void SpawnSequenceCompleted()
    {
        SetRenderersVisible(true);
        hasSpawned = true;

        collider.enabled = true;

        movement.StorePlayer(player);

    }


    protected void SetRenderersVisible(bool visibility)
    {
        renderer.enabled = visibility;
        spawnIndicator.enabled = !visibility;

    }

    // Update is called once per frame
    protected bool CanAttack()
    {
        return renderer.enabled;
    }

    protected virtual void LateUpdate()
    {
        if (hasSpawned)
            FacePlayer();
    }


     protected void FacePlayer()
    {
        if (player == null) return;

        Vector2 dir = (Vector2)player.transform.position - (Vector2)transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle += rotationOffset;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void TakeDamage(int damage, bool isCriticalHit)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;

        onDamageTaken?.Invoke(damage, transform.position, isCriticalHit);

        if (health <= 0)
            OnDeath();
    }

    protected void OnDeath()
    {
        onDeath?.Invoke(transform.position);

        deathParticles.transform.SetParent(null);
        deathParticles.Play();
        Destroy(gameObject);

    }



    
    protected void OnDrawGizmos()
    {
        if (!showGizmos)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);


    }
}
