using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private int score = 0;
    public int CurrentScore => score;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Call this to add to the player's score.
    /// </summary>
    public void AddScore(int amount)
    {
        score += amount;
        // Optionally: update any in-game score UI here
        Debug.Log($"Score is now {score}");
    }

    /// <summary>
    /// Resets the score back to zero. Call when starting a new game.
    /// </summary>
    public void ResetScore()
    {
        score = 0;
    }
}
