using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int WaveReached { get; private set; }
    public int EnemiesKilled { get; private set; }
    public int MoneyCollected { get; private set; }
    public int LevelReached { get; private set; }
    public bool DidWin { get; private set; } = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterEnemyKill() => EnemiesKilled++;
    public void RegisterCoin(int amount) => MoneyCollected += amount;
    public void SetWave(int wave) => WaveReached = wave;
    public void SetLevel(int lvl) => LevelReached = lvl;
    public void SetWin(bool didWin) => DidWin = didWin;


    public void ResetStats()
    {
        WaveReached = 0;
        EnemiesKilled = 0;
        MoneyCollected = 0;
        LevelReached = 0;
        DidWin = false;
    }
}
