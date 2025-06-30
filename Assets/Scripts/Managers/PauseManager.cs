using UnityEngine;
using System;
public class PauseManager : MonoBehaviour
{
    [SerializeField] private UIManager uIManager;
    
    public static Action onGamePaused;
    public static Action onGameResumed;
    private bool isPaused;
    private bool canPause = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.CurrentGameState == GameState.GAME)
        {
            togglePause();
        }
    }

    public void togglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0;
            uIManager.GamePausedCallback();
            PauseButtonCallback();

        }
        else
        {
            Time.timeScale = 1;
            uIManager.GameResumedCallback();
            ResumeButtonCallback();
        }
    }

    public bool IsPaused()
    {
        return isPaused;
    }
    public void CanPause(bool check)
    {
        canPause = check;
    }
    
    
    public void PauseButtonCallback()
    {
        // Time.timeScale = 0;
        onGamePaused?.Invoke();
    }
    public void ResumeButtonCallback()
    {
        
        // Time.timeScale = 1;
        onGameResumed?.Invoke();
    }
}
