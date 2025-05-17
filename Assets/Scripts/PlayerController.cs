
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rig;

    [Header("Stats")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float speedX, speedY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rig.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        WASDMove();
    }

    void WASDMove()
    {
        speedX = Input.GetAxis("Horizontal") * moveSpeed;
        speedY = Input.GetAxis("Vertical") * moveSpeed;
        rig.linearVelocity = new Vector2(speedX, speedY);
    }
}
