
using UnityEngine;

public class IronPickup : MonoBehaviour
{
    public int ironAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IronCounter.Instance.AddIron(ironAmount);
            Destroy(gameObject);
        }
    }
}
