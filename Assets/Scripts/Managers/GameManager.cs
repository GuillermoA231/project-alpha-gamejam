using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using System;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Actions")]
    public static Action onGamePaused;
    public static Action onGameResumed;
    public static GameState CurrentGameState { get; private set; }

    private ScoreManager scoreManagerInstance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Awake()
    {
        CultureInfo culture = new CultureInfo("es-ES");

        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        Debug.Log("Current Culture: " + CultureInfo.CurrentCulture.Name);

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        Application.targetFrameRate = 60;
        SetGameState(GameState.MENU);

        scoreManagerInstance = ScoreManager.Instance;
    }

    public void SetEnglish() =>
        LocalizationSettings.SelectedLocale =
            LocalizationSettings.AvailableLocales.Locales
                .FirstOrDefault(l => l.Identifier.Code == "en");

    public void SetSpanish() =>
        LocalizationSettings.SelectedLocale =
            LocalizationSettings.AvailableLocales.Locales
                .FirstOrDefault(l => l.Identifier.Code == "es");


    public void StartGame() => SetGameState(GameState.GAME);
    public void Tutorial() => SetGameState(GameState.TUTORIAL);
    public void Menu() => SetGameState(GameState.MENU);
    public void StartWeaponSelection() => SetGameState(GameState.WEAPONSELECTION);
    public void OpenShop() => SetGameState(GameState.SHOP);
    public void SetGameState(GameState gameState)
    {
        CurrentGameState = gameState;
        
        IEnumerable<IGameStateListener> gameStateListeners =
        FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
        .OfType<IGameStateListener>();

        foreach (IGameStateListener gameStateListener in gameStateListeners)
            gameStateListener.GameStateChangedCallback(gameState);

    }

    public void WaveCompletedCallback()
    {
        bool canLevelUp = Player.instance.GetComponent<PlayerEXP>().CanLevelUpThisWave();
        bool hasChest = WaveTransitionManager.instance.HasCollectedChest();

        if (canLevelUp || hasChest)
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
        scoreManagerInstance.SetWin(false);
        scoreManagerInstance.ResetStats();
        SceneManager.LoadScene("BasicArena", LoadSceneMode.Single);
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

public interface IGameStateListener
{
    void GameStateChangedCallback(GameState gameState);
}
