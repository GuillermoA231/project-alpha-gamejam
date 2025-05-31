using System;
using UnityEngine;

public class Chest : MonoBehaviour, ICollectable
{
    [Header(" Actions ")]
    public static Action<Chest> onCollected;

    public void Collect(Player player)
    {
        onCollected?.Invoke(this);
        Destroy(gameObject);
    }

}
