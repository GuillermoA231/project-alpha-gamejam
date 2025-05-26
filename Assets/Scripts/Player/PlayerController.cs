using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Directional Sprites")]
    [SerializeField] private Sprite spriteUp;
    [SerializeField] private Sprite spriteDown;
    [SerializeField] private Sprite spriteLeft;
    [SerializeField] private Sprite spriteRight;

    [Header("Stats")]
    [SerializeField] private float moveSpeed;
    private float speedX, speedY;

    void Start()
    {
        // Auto-assign Rigidbody2D if not manually set
        if (rig == null)
            rig = GetComponent<Rigidbody2D>();

        // Find the child named "Player Renderer" and get its SpriteRenderer
        if (spriteRenderer == null)
        {
            Transform child = transform.Find("Player Renderer");
            if (child != null)
            {
                spriteRenderer = child.GetComponent<SpriteRenderer>();
            }
            else
            {
                Debug.LogError("Child object 'Player Renderer' not found!");
            }
        }
    }

    private void FixedUpdate()
    {
        WASDMove();
    }

    void WASDMove()
    {
        speedX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        speedY = Input.GetAxisRaw("Vertical") * moveSpeed;
        rig.linearVelocity = new Vector2(speedX, speedY);

        // Change sprite based on movement direction
        if (speedY > 0)
        {
            spriteRenderer.sprite = spriteUp;
        }
        else if (speedY < 0)
        {
            spriteRenderer.sprite = spriteDown;
        }
        else if (speedX > 0)
        {
            spriteRenderer.sprite = spriteRight;
        }
        else if (speedX < 0)
        {
            spriteRenderer.sprite = spriteLeft;
        }
    }
}
