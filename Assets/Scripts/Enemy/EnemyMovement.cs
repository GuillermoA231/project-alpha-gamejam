
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header(" Elements")]
    private Player player;

    [Header(" Settings")]
    [SerializeField] private float moveSpeed;
    private bool isRangedEnemy;


    // Update is called once per frame
    void Update()
    {
        // if (player != null)
        //     FollowPlayer();
    }

    public void StorePlayer(Player player)
    {
        this.player = player; 
    }


    public void FollowPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;

        Vector2 targetPosition = (Vector2)transform.position + direction * moveSpeed * Time.deltaTime;

        transform.position = targetPosition;

    }



}
