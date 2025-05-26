using UnityEngine;


[RequireComponent(typeof(Player))]
public class PlayerDetection : MonoBehaviour
{
    [Header(" Colliders ")]
    [SerializeField] private CapsuleCollider2D playerCollider;

    // private void FixedUpdate()
    // {
    //     Vector2 position = (Vector2)transform.position + playerCollider.offset;
    //     Vector2 size = playerCollider.size;
    //     CapsuleDirection2D direction = playerCollider.direction;

    //     Collider2D[] moneyColliders = Physics2D.OverlapCapsuleAll(position, size, direction, 0f);

    //     foreach (Collider2D moneyCollider in moneyColliders)
    //     {

    //         if (moneyCollider.TryGetComponent(out Money money))
    //         {

    //             Debug.Log("Collected: " + money.name);
    //             money.Collect(GetComponent<Player>());
    //         }
    //     }
    // }
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.TryGetComponent(out Money money))
        {
            if (!collider2D.IsTouching(playerCollider))
                return;


            Debug.Log("Collected: " + money.name);
            money.Collect(GetComponent<Player>());
        }
        if (collider2D.TryGetComponent(out Diamond diamond))
        {
            if (!collider2D.IsTouching(playerCollider))
                return;


            Debug.Log("Collected: " + diamond.name);
            diamond.Collect(GetComponent<Player>());
        }
    }
}
