using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        Application.targetFrameRate = 60;
        SetGameState(GameState.MENU);
    }


    public void StartGame() => SetGameState(GameState.GAME);
    public void StartWeaponSelection() => SetGameState(GameState.WEAPONSELECTION);
    public void OpenShop() => SetGameState(GameState.SHOP);
    public void SetGameState(GameState gameState)
    {
        Debug.Log(gameState);
        IEnumerable<IGameStateListener> gameStateListeners =
        FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
        .OfType<IGameStateListener>();

        foreach (IGameStateListener gameStateListener in gameStateListeners)
            gameStateListener.GameStateChangedCallback(gameState);

    }

    public void WaveCompletedCallback()
    {
        if (Player.instance.HasleveledUp())
        {
            SetGameState(GameState.WAVETRANSITION);
        }
        else
        {
            SetGameState(GameState.SHOP);
        }
    }
    public void ManageGameOver()
    {
        SceneManager.LoadScene("BasicArena", LoadSceneMode.Single);
    }
}

public interface IGameStateListener
{
    void GameStateChangedCallback(GameState gameState);
}
