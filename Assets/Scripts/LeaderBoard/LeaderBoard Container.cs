using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// LeaderBoardContainer retrieves the current game stats from ScoreManager
/// and displays them in the UI via serialized Text fields.
/// Attach this script to your leaderboard UI container object.
/// </summary>
public class LeaderBoardContainer : MonoBehaviour
{
    [Header("UI Text References")]
    [SerializeField] private TextMeshProUGUI winUI;
    [SerializeField] private TextMeshProUGUI waveUI;
    [SerializeField] private TextMeshProUGUI enemiesUI;
    [SerializeField] private TextMeshProUGUI moneyUI;
    [SerializeField] private TextMeshProUGUI levelUI;

    private ScoreManager scoreManager;

    private void OnEnable()
    {
        if (ScoreManager.Instance == null)
        {
            Debug.LogError("ScoreManager instance not found!");
            return;
        }

        scoreManager = ScoreManager.Instance;
        UpdateLeaderBoardUI();
    }


    private void UpdateLeaderBoardUI()
    {
        bool didWin = scoreManager.DidWin;
        int wave = scoreManager.WaveReached;
        int enemies = scoreManager.EnemiesKilled;
        int money = scoreManager.MoneyCollected;
        int level = scoreManager.LevelReached;

        Debug.Log(didWin + " - " + wave + " - " + enemies + " - " + money + " - " + level);

        winUI.text = didWin ? "Y" : "N";
        waveUI.text = wave.ToString();
        enemiesUI.text = enemies.ToString();
        moneyUI.text = money.ToString();
        levelUI.text = level.ToString();
    }
}
