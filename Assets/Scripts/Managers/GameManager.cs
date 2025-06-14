using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Awake()
    {
        // Set culture to Spanish (Spain)
        CultureInfo culture = new CultureInfo("es-ES");

        // Apply to the current thread (affects date, number formats, etc.)
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        // Optional: Print to confirm
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
