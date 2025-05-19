
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public event Action<GameObject> OnEnemyDied;

    public void Die()
    {
        OnEnemyDied?.Invoke(this.gameObject);
        Destroy(gameObject);
    }
}
