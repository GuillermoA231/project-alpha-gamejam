using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine.UI;

public class ShopManagerUI : MonoBehaviour
{
    [Header("Player Stats Elements")]
    [SerializeField] private RectTransform playerStatsPanel;
    [SerializeField] private GameObject playerStatsContainer;
    [SerializeField] private RectTransform playerClosedButton;
    private Vector2 playerStatsOpenPos;
    private Vector2 playerStatsClosedPos;
    IEnumerator Start()
    {
        yield return null;

        ConfigurePlayerStatsPanel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ConfigurePlayerStatsPanel()
    {
        float width = Screen.width / (4 * playerStatsPanel.lossyScale.x);
        playerStatsPanel.offsetMax = playerStatsPanel.offsetMax.With(x: width);

        playerStatsOpenPos = playerStatsPanel.anchoredPosition;
        playerStatsClosedPos = playerStatsOpenPos + Vector2.left * width;

        playerStatsPanel.anchoredPosition = playerStatsClosedPos;

        HidePlayerStats();
    }

    [Button]
    public void ShowPlayerStats()
    {
        playerStatsContainer.SetActive(true);
        playerClosedButton.gameObject.SetActive(true);
        playerClosedButton.GetComponent<Image>().raycastTarget = true;

        LeanTween.cancel(playerStatsPanel);
        LeanTween.move(playerStatsPanel, playerStatsOpenPos, .5f).setEase(LeanTweenType.easeInCubic);

        
        LeanTween.cancel(playerClosedButton);
        LeanTween.alpha(playerClosedButton, 0.8f, .5f).setRecursive(false);
    }

    [Button]
    public void HidePlayerStats()
    {
        
        playerClosedButton.GetComponent<Image>().raycastTarget = false;

        LeanTween.cancel(playerStatsPanel);
        LeanTween
        .move(playerStatsPanel, playerStatsClosedPos, .5f)
        .setEase(LeanTweenType.easeOutCubic)
        .setOnComplete(() => playerStatsContainer.SetActive(false));

        LeanTween.cancel(playerClosedButton);
        LeanTween.alpha(playerClosedButton, 0, .5f).setRecursive(false)
        .setOnComplete(() => playerClosedButton.gameObject.SetActive(false));
    }
}
