using UnityEngine;


[RequireComponent(typeof(Player))]
public class PlayerDetection : MonoBehaviour
{
    [Header(" Colliders ")]
    [SerializeField] private CircleCollider2D collectableCollider;

    // private void FixedUpdate()
    // {
    //     Vector2 position = (Vector2)transform.position + collectableCollider.offset;
    //     Vector2 size = collectableCollider.size;
    //     CapsuleDirection2D direction = collectableCollider.direction;

    //     Collider2D[] moneyColliders = Physics2D.OverlapCapsuleAll(position, size, direction, 0f);

    //     foreach (Collider2D moneyCollider in moneyColliders)
    //     {

    //         if (moneyCollider.TryGetComponent(out Money collectable))
    //         {

    //             Debug.Log("Collected: " + collectable.name);
    //             collectable.Collect(GetComponent<Player>());
    //         }
    //     }
    // }
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.TryGetComponent(out ICollectable collectable))
        {
            if (!collider2D.IsTouching(collectableCollider))
                return;
            collectable.Collect(GetComponent<Player>());
        }
    }
}
