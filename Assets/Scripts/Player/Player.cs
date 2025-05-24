using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public class Player : MonoBehaviour
{

    [Header(" Components")]
    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
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

}
