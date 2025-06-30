using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour, IGameStateListener
{

    [Header("Panels")]
    [SerializeField] private GameObject introPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject weaponSelectionPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject stageCompletePanel;
    [SerializeField] private GameObject waveTransitionPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject gameOverPanel;

    private List<GameObject> panels = new List<GameObject>();

    private void Awake()
    {
        panels.AddRange(new GameObject[]
        {
            introPanel,
            pausePanel,
            menuPanel,
            tutorialPanel,
            weaponSelectionPanel,
            gamePanel,
            stageCompletePanel,
            waveTransitionPanel,
            shopPanel,
            gameOverPanel,
        });

        PauseManager.onGamePaused += GamePausedCallback;
        PauseManager.onGameResumed += GameResumedCallback;
    }
    private void OnDestroy() {
        
        PauseManager.onGamePaused -= GamePausedCallback;
        PauseManager.onGameResumed -= GameResumedCallback;
    }

    
    private void ShowPanel(GameObject panel, bool hidePreviousPanels = true)
    {
        if (hidePreviousPanels)
        {
            foreach (GameObject p in panels)
                p.SetActive(p == panel);
        }
        else
        {
            panel.SetActive(true);
        }

    }

    //Use if I don't want to hide the other panels
    public void GamePausedCallback()
    {
        pausePanel.SetActive(true);
    }
    public void GameResumedCallback()
    {
        pausePanel.SetActive(false);
    }

    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.INTRO:
                ShowPanel(introPanel);
                break;
            case GameState.PAUSE:
                ShowPanel(pausePanel);
                break;
            case GameState.MENU:
                ShowPanel(menuPanel);
                break;
            case GameState.TUTORIAL:
                ShowPanel(tutorialPanel);
                break;
            case GameState.WEAPONSELECTION:
                ShowPanel(weaponSelectionPanel);
                break;
            case GameState.GAME:
                ShowPanel(gamePanel);
                break;
            case GameState.STAGECOMPLETE:
                ShowPanel(stageCompletePanel);
                break;
            case GameState.WAVETRANSITION:
                ShowPanel(waveTransitionPanel);
                break;
            case GameState.SHOP:
                ShowPanel(shopPanel);
                break;
            case GameState.GAMEOVER:
                ShowPanel(gameOverPanel);
                break;
        }
    }

}
