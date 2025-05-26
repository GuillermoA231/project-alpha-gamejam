
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject spawnEffectPrefab;
    public GameObject ironPrefab;
    public Transform[] spawnPoints;

    public int initialEnemyCount = 15;
    public float spawnDelay = 1.0f;
    public float ironDropChance = 0.3f;
    public float respawnCooldown = 2.0f;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool playerInZone = true;

    void Start()
    {
        SpawnInitialEnemies();
    }

    void SpawnInitialEnemies()
    {
        for (int i = 0; i < initialEnemyCount; i++)
        {
            Transform spawnPoint = RandomSpawnPoint();
            StartCoroutine(SpawnEnemyWithEffect(spawnPoint));
        }
    }

    IEnumerator SpawnEnemyWithEffect(Transform spawnPoint)
    {
        if (spawnEffectPrefab != null)
            Instantiate(spawnEffectPrefab, spawnPoint.position, Quaternion.identity);

        yield return new WaitForSeconds(spawnDelay);

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        if (Random.value < ironDropChance)
        {
            DropIron(spawnPoint.position);
        }

        MeleeEnemy enemyScript = enemy.GetComponent<MeleeEnemy>();
        if (enemyScript != null)
            //enemyScript.OnEnemyDied += HandleEnemyDied;

        activeEnemies.Add(enemy);
    }

    void DropIron(Vector3 position)
    {
        Instantiate(ironPrefab, position, Quaternion.identity);
    }

    void HandleEnemyDied(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
        if (playerInZone)
            StartCoroutine(RespawnEnemyAfterCooldown());
    }

    IEnumerator RespawnEnemyAfterCooldown()
    {
        yield return new WaitForSeconds(respawnCooldown);
        Transform spawnPoint = RandomSpawnPoint();
        StartCoroutine(SpawnEnemyWithEffect(spawnPoint));
    }

    Transform RandomSpawnPoint()
    {
        int index = Random.Range(0, spawnPoints.Length);
        return spawnPoints[index];
    }

    public void SetPlayerInZone(bool inZone)
    {
        playerInZone = inZone;
    }
}
