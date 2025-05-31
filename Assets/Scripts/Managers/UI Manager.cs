using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour, IGameStateListener
{

    [Header("Panels")]
    [SerializeField] private GameObject menuPanel;
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
            menuPanel,
            weaponSelectionPanel,
            gamePanel,
            stageCompletePanel,
            waveTransitionPanel,
            shopPanel,
            gameOverPanel,
        });
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.MENU:
                ShowPanel(menuPanel);
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

    private void ShowPanel(GameObject panel, bool hidePreviousPanels =true)
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
}
