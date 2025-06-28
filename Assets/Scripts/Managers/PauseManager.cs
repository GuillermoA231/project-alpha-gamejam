using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private UIManager uIManager;
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
            }
            else
            {
                Time.timeScale = 1;
                uIManager.GameResumedCallback();
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
}
