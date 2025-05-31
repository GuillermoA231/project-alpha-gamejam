using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header(" Elements")]
    private Player player;

    [Header(" Settings")]
    [SerializeField] private float baseMoveSpeed = 5f;
    private float currentMoveSpeed;

    void Awake()
    {
        currentMoveSpeed = baseMoveSpeed;
    }

    void Update()
    {
        // if you want auto-follow, uncomment:
        // if (player != null) FollowPlayer();
    }

    public void StorePlayer(Player player)
    {
        this.player = player;
    }

    public void FollowPlayer()
    {
        if (player == null) return;
        Vector2 dir = (player.transform.position - transform.position).normalized;
        transform.position += (Vector3)(dir * currentMoveSpeed * Time.deltaTime);
    }

    public void SetRandomSpeed(float minMultiplier = 0.5f, float maxMultiplier = 2.5f)
    {
        float m = Random.Range(minMultiplier, maxMultiplier);
        currentMoveSpeed = baseMoveSpeed * m;
    }
}
