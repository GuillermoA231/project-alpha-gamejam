// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class WaveManager2 : MonoBehaviour
// {
//     [Header("Wave Settings")]
//     [SerializeField] private Wave[] wave;
//     [SerializeField] private float waveDuration = 15f;

//     [Header("Enemy Prefabs")]
//     [SerializeField] private GameObject meleePrefab;
//     [SerializeField] private GameObject rangedPrefab;

//     [Header("Melee Base Settings")]
//     [SerializeField] private float meleeBaseFreq       = 1f;    // spawns per second
//     [SerializeField] private float meleeBaseEndPercent = 30f;   // % of waveDuration

//     [Header("Ranged Base Settings (activates at wave 3)")]
//     [SerializeField] private float rangedBaseFreq       = 1f;
//     [SerializeField] private float rangedBaseEndPercent = 15f;

//     [Header("Spawn Points")]
//     [SerializeField] private Vector2[] spawnPoints = new Vector2[]
//     {
//         new Vector2(-7.5f, 12),
//         new Vector2(-14f, 6),
//         new Vector2(-14f, -12),
//         new Vector2(-3f, -12),
//         new Vector2(5f, -12),
//         new Vector2(16f, -12),
//         new Vector2(16f, 4),
//         new Vector2(15f, 12)
//     };

//     private int    currentWaveIndex = -1;
//     private float  globalTimer      = 0f;
//     private int[]  spawnCounters;

//     void OnEnable()
//     {
//         SceneManager.sceneLoaded += OnSceneLoaded;
//     }

//     void OnDisable()
//     {
//         SceneManager.sceneLoaded -= OnSceneLoaded;
//     }

//     void Start()
//     {
//         // In case the scene is already loaded before OnEnable fires:
//         if (SceneManager.GetActiveScene().name == "BasicArena")
//             ResetWaveManager();
//     }

//     void Update()
//     {
//         globalTimer += Time.deltaTime;

//         int wave = Mathf.FloorToInt(globalTimer / waveDuration);
//         float tInWave = globalTimer - wave * waveDuration;

//         if (wave != currentWaveIndex)
//         {
//             currentWaveIndex = wave;
//             InitWaveCounters();
//         }

//         ManageWaveSpawning(wave, tInWave);
//     }

//     private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//     {
//         if (scene.name == "BasicArena")
//             ResetWaveManager();
//     }

//     private void ResetWaveManager()
//     {
//         // 1) reset all timers/counters
//         globalTimer      = 0f;
//         currentWaveIndex = -1;
//         InitWaveCounters();

//         // 2) destroy any leftover enemies under this manager
//         //    (we parent all instantiates to `this.transform`)
//         for (int i = transform.childCount - 1; i >= 0; i--)
//         {
//             Destroy(transform.GetChild(i).gameObject);
//         }
//     }

//     private void InitWaveCounters()
//     {
//         // melee always, + ranged once wave>=2 (i.e. wave 3)
//         int segments = 1 + (currentWaveIndex >= 2 ? 1 : 0);
//         spawnCounters = new int[segments];
//         for (int i = 0; i < segments; i++)
//             spawnCounters[i] = 1;
//     }

//     private void ManageWaveSpawning(int wave, float tInWave)
//     {
//         // determine current growth factors
//         float freqGrowth, endGrowth;
//         if (wave >= 14)
//         {
//             freqGrowth = 1.5f; endGrowth = 1.8f;
//         }
//         else if (wave >= 7)
//         {
//             freqGrowth = 1.25f; endGrowth = 1.4f;
//         }
//         else
//         {
//             freqGrowth = 1.1f; endGrowth = 1.2f;
//         }

//         // ─── Melee ───
//         {
//             float spawnFactor = Mathf.Pow(freqGrowth, wave);
//             float endPct      = meleeBaseEndPercent * Mathf.Pow(endGrowth, wave);

//             float tStart = 0f;
//             float tEnd   = endPct / 100f * waveDuration;
//             float freq   = meleeBaseFreq * spawnFactor;
//             float delay  = 1f / freq;

//             if (tInWave >= tStart && tInWave <= tEnd &&
//                 tInWave / delay > spawnCounters[0])
//             {
//                 SpawnEnemy(meleePrefab);
//                 spawnCounters[0]++;
//             }
//         }

//         // ─── Ranged (from wave 3) ───
//         if (wave >= 2)
//         {
//             int idx       = 1;
//             float wCount  = wave - 2;
//             float spawnFactor = Mathf.Pow(freqGrowth, wCount);
//             float endPct      = rangedBaseEndPercent * Mathf.Pow(endGrowth, wCount);

//             float tStart = 0f;
//             float tEnd   = endPct / 100f * waveDuration;
//             float freq   = rangedBaseFreq * spawnFactor;
//             float delay  = 1f / freq;

//             if (tInWave >= tStart && tInWave <= tEnd &&
//                 tInWave / delay > spawnCounters[idx])
//             {
//                 SpawnEnemy(rangedPrefab);
//                 spawnCounters[idx]++;
//             }
//         }
//     }

//     private void SpawnEnemy(GameObject prefab)
//     {
//         Vector2 pos = spawnPoints[Random.Range(0, spawnPoints.Length)];
//         var enemy = Instantiate(prefab, pos, Quaternion.identity, transform);

//         var mv = enemy.GetComponent<EnemyMovement>();
//         if (mv != null)
//             mv.SetRandomSpeed(0.5f, 2.5f);
//     }
// }
