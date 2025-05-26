
using UnityEngine;

using Random = UnityEngine.Random;

public class DropsManager : MonoBehaviour
{
    [Header(" Elements")]
    [SerializeField] private Money moneyPrefab;
    [SerializeField] private Diamond diamondPrefab;

    private void Awake()
    {
        Enemy.onDeath += EnemyDeathCallback;
    }

    private void OnDestroy()
    {
        Enemy.onDeath += EnemyDeathCallback;
    }

    private void EnemyDeathCallback(Vector2 enemyPosition)
    {
        bool shouldSpawnDiamond = Random.Range(0, 101) <= 0;

        GameObject droppable = shouldSpawnDiamond ? diamondPrefab.gameObject : moneyPrefab.gameObject;

        GameObject droppableInstance = Instantiate(droppable, enemyPosition, Quaternion.identity, transform);
        droppableInstance.name = "Droppable: " + Random.Range(0, 500);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
