using UnityEngine;

[RequireComponent(typeof(PlayerHealth), typeof(PlayerEXP))]
public class Player : MonoBehaviour
{
    public static Player instance;
    [Header(" Components")]
    [SerializeField] private CapsuleCollider2D collider;
    private PlayerHealth playerHealth;
    private PlayerEXP playerEXP;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        playerHealth = GetComponent<PlayerHealth>();
        playerEXP = GetComponent<PlayerEXP>();
    }
    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void TakeDamage(int damage)
    {
        playerHealth.TakeDamage(damage);
    }

    public Vector2 GetShootPosition()
    {
        return (Vector2)transform.position + collider.offset;
    }

    public bool HasleveledUp()
    {
        return playerEXP.HasLeveledUp();
    }
}
