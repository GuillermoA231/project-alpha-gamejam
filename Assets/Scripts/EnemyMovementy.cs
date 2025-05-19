
using UnityEngine;

public class EnemyMovementy : MonoBehaviour
{

    [Header("Spawn Sequence")]
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private SpriteRenderer spawnIndicator;
    private bool hasSpawned;
    [SerializeField] private float spawnScaling, spawnScalingTime;

    [Header(" Elements")]
    private Player player;
    [Header(" Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float playerDetectionRadius;


    [Header("Effects")]
    [SerializeField] private ParticleSystem deathParticles;

    [Header("DEBUG")]
    [SerializeField] private bool showGizmos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Player>();

        if (player == null)
        {
            Debug.LogWarning("No player found, Destroying");
            Destroy(gameObject);
        }

        // Hide the renderer
        // Show the spawn indicator
        renderer.enabled = false;
        spawnIndicator.enabled = true;

        // Prevent Following & Attacking during the spawn sequence

        // Scale up and down the spawn indicator
        Vector3 targetScale = spawnIndicator.transform.localScale * spawnScaling;
        LeanTween.scale(spawnIndicator.gameObject, targetScale, spawnScalingTime)
                 .setLoopPingPong(3)
                 .setOnComplete(SpawnSequenceCompleted);
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasSpawned)
            return;

        FollowPlayer();

        TryAttack();

    }


    private void SpawnSequenceCompleted()
    {
        // Show the enemy after 2.5 seconds

        // Show the renderer
        renderer.enabled = true;
        // Hide the spawn indicator
        spawnIndicator.enabled = false;
        hasSpawned = true;

    }

    private void FollowPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;

        Vector2 targetPosition = (Vector2)transform.position + direction * moveSpeed * Time.deltaTime;

        transform.position = targetPosition;

    }

    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= playerDetectionRadius)
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }

}
