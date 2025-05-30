using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using Unity.VisualScripting;

public class WaveManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Player player;


    [Header("Settings")]
    [SerializeField] private float waveDuration;
    private bool isTimerOn;
    private float timer;
    private int currentWaveIndex;

    [Header("Wave")]
    [SerializeField] private Wave[] waves;
    private List<float> localCounters = new List<float>();

    void Start()
    {

        StartWave(currentWaveIndex);
    }

    void Update()
    {
        if (!isTimerOn)
            return;
        if (timer < waveDuration)
            manageCurrentWave();
        else
            StartWaveTransition();
    }

    private void StartWave(int waveIndex)
    {
        Debug.Log("Wave: " + waveIndex);
        localCounters.Clear();
        foreach (WaveSegment segment in waves[waveIndex].segments)
            localCounters.Add(1);

        timer = 0;
        isTimerOn = true;
    }

    private void manageCurrentWave()
    {
        Wave currentwave = waves[currentWaveIndex];

        for (int i = 0; i < currentwave.segments.Count; i++)
        {
            WaveSegment segment = currentwave.segments[i];

            float tStart = segment.tStartEnd.x / 100 * waveDuration;
            float tEnd = segment.tStartEnd.y / 100 * waveDuration;

            if (timer < tStart || timer > tEnd)
                continue;

            float timeSinceSegmentStart = timer - tStart;
            float spawnDelay = 1f / segment.spawnFrequency;

            if (timeSinceSegmentStart / spawnDelay > localCounters[i])
            {
                Instantiate(segment.prefab, GetSpawnPosition(), Quaternion.identity, transform);
                localCounters[i]++;
            }

        }

        timer += Time.deltaTime;
    }

    private void StartWaveTransition()
    {
        isTimerOn = false;
        //DefeatAllEnemies();
        currentWaveIndex++;

        if (currentWaveIndex >= waves.Length)
            Debug.Log("All waves cleared");
        else
            StartWave(currentWaveIndex);
    }


    private void DefeatAllEnemies()
    {
        while (transform.childCount > 0)
        {
            Transform child = transform.transform.GetChild(0);
            child.SetParent(null);
            Object.Destroy(child.gameObject);
        }
    }
    private Vector2 GetSpawnPosition()
    {
        Vector2 direction = Random.onUnitSphere;
        Vector2 offset = direction.normalized * Random.Range(6, 10);
        Vector2 targetPosition = (Vector2)player.transform.position + offset;

        targetPosition.x = Mathf.Clamp(targetPosition.x, -20, 20);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -16, 16);

        return targetPosition;
    }
}

[System.Serializable]
public struct Wave
{
    public string name;
    public List<WaveSegment> segments;
}

[System.Serializable]
public struct WaveSegment
{
    [MinMaxSlider(0, 100)] public Vector2 tStartEnd;
    public float spawnFrequency;
    public GameObject prefab;
}