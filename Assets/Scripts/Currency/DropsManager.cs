using UnityEngine;
using Random = UnityEngine.Random;

public class DropsManager : MonoBehaviour
{
    [Header(" Elements")]
    [SerializeField] private Money moneyPrefab;
    [SerializeField] private Diamond diamondPrefab;

    private void OnEnable()
    {
        // Subscribe when this object becomes active
        Enemy.onDeath += EnemyDeathCallback;
    }

    private void OnDisable()
    {
        // Unsubscribe when this object is destroyed or disabled
        Enemy.onDeath -= EnemyDeathCallback;
    }

    private void EnemyDeathCallback(Vector2 enemyPosition)
    {
        bool shouldSpawnDiamond = Random.Range(0, 101) <= 0;

        GameObject droppable = shouldSpawnDiamond
            ? diamondPrefab.gameObject
            : moneyPrefab.gameObject;

        Instantiate(
            droppable,
            enemyPosition,
            Quaternion.identity,
            transform
        ).name = "Droppable: " + Random.Range(0, 500);
    }
}
