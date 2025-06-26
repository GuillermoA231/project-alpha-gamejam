using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine.UI;
using System;

public class ShopManagerUI : MonoBehaviour
{
    [Header("Player Stats Elements")]
    [SerializeField] private RectTransform playerStatsPanel;
    [SerializeField] private GameObject playerStatsContainer;
    [SerializeField] private RectTransform playerClosedButton;
    private Vector2 playerStatsOpenPos;
    private Vector2 playerStatsClosedPos;

    [Header("Inventory Elements")]
    [SerializeField] private RectTransform inventoryPanel;
    [SerializeField] private GameObject inventoryContainer;
    [SerializeField] private RectTransform inventoryClosedButton;
    private Vector2 inventoryOpenPos;
    private Vector2 inventoryClosedPos;

    [Header(" Item Info Elements")]
    [SerializeField] private RectTransform itemInfoSlidePanel;
    private Vector2 itemInfoOpenPos;
    private Vector2 itemInfoClosedPos;


    /// <summary>
    // Initialization coroutine that delays execution by one frame to ensure all UI elements
    // have completed their layout and scaling calculations before configuring the panels.
    // This avoids inaccurate size or position data due to Unity's layout system timing.
    // Calls configuration methods to prepare the player stats and inventory UI for interaction.
    /// </summary>
    IEnumerator Start()
    {
        yield return null;

        ConfigurePlayerStatsPanel();
        ConfigureInventoryPanel();
        ConfigureItemInfoPanel();
    }


    void Update()
    {

    }

    /// <summary>
    // Configures the player stats panel’s width dynamically based on the actual screen size
    // and the panel’s current scale in the UI hierarchy. This guarantees the panel occupies
    // exactly one-quarter of the visible screen width regardless of any Canvas or parent scaling.
    // It also sets up two key positions: the fully visible “open” position and an off-screen
    // “closed” position shifted to the left by the panel’s width, enabling smooth slide-in/out animations.
    // Finally, the panel starts hidden off-screen, ensuring a clean initial UI state.
    /// </summary>
    private void ConfigurePlayerStatsPanel()
    {
        float width = Screen.width / (4 * playerStatsPanel.lossyScale.x);
        playerStatsPanel.offsetMax = playerStatsPanel.offsetMax.With(x: width);

        playerStatsOpenPos = playerStatsPanel.anchoredPosition;
        playerStatsClosedPos = playerStatsOpenPos + Vector2.left * width;

        playerStatsPanel.anchoredPosition = playerStatsClosedPos;

        HidePlayerStats();
    }



    /// <summary>
    /// Activates and animates the player stats panel into view with easing for a polished effect.
    /// Enables the close button’s interaction and fades it in concurrently to prevent abrupt UI pop-in.
    /// </summary>
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

    /// <summary>
    /// Slides the player stats panel off-screen and fades out the close button.
    /// Disables raycast immediately to block input during animation,
    /// then deactivates UI elements on completion to keep the hierarchy clean.
    /// </summary>

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

    //Inventory Panel


    private void ConfigureInventoryPanel()
    {
        float width = Screen.width / (4 * inventoryPanel.lossyScale.x);
        inventoryPanel.offsetMin = inventoryPanel.offsetMin.With(x: -width);

        inventoryOpenPos = inventoryPanel.anchoredPosition;
        inventoryClosedPos = inventoryOpenPos - Vector2.left * width;

        inventoryPanel.anchoredPosition = inventoryClosedPos;

        HideInventory(false);
    }


    [Button]
    public void ShowInventory()
    {
        inventoryContainer.SetActive(true);
        inventoryClosedButton.gameObject.SetActive(true);
        inventoryClosedButton.GetComponent<Image>().raycastTarget = true;

        LeanTween.cancel(inventoryPanel);
        LeanTween.move(inventoryPanel, inventoryOpenPos, .5f).setEase(LeanTweenType.easeInCubic);


        LeanTween.cancel(inventoryClosedButton);
        LeanTween.alpha(inventoryClosedButton, 0.8f, .5f).setRecursive(false);
    }


    [Button]
    public void HideInventory(bool hideItemInfo = true)
    {
        inventoryClosedButton.GetComponent<Image>().raycastTarget = false;

        LeanTween.cancel(inventoryPanel);
        LeanTween
        .move(inventoryPanel, inventoryClosedPos, .5f)
        .setEase(LeanTweenType.easeOutCubic)
        .setOnComplete(() => inventoryContainer.SetActive(false));

        LeanTween.cancel(inventoryClosedButton);
        LeanTween.alpha(inventoryClosedButton, 0, .5f).setRecursive(false)
        .setOnComplete(() => inventoryClosedButton.gameObject.SetActive(false));

        if (hideItemInfo)
            HideItemInfo();
    }

    //Item Info Panel


    private void ConfigureItemInfoPanel()
    {
        float height = Screen.height / (2 * itemInfoSlidePanel.lossyScale.x);

        itemInfoSlidePanel.offsetMax = itemInfoSlidePanel.offsetMax.With(y: height);

        itemInfoOpenPos = itemInfoSlidePanel.anchoredPosition;
        itemInfoClosedPos = itemInfoOpenPos + Vector2.down * height;


        itemInfoSlidePanel.anchoredPosition = itemInfoClosedPos;
    }

    [Button]
    public void ShowItemInfo()
    {
        itemInfoSlidePanel.gameObject.SetActive(true);

        itemInfoSlidePanel.LeanCancel();
        itemInfoSlidePanel
        .LeanMove((Vector3)itemInfoOpenPos, .3f).setEase(LeanTweenType.easeOutCirc);
    }

    [Button]
    public void HideItemInfo()
    {
        itemInfoSlidePanel.LeanCancel();
        itemInfoSlidePanel
        .LeanMove((Vector3)itemInfoClosedPos, .3f).setEase(LeanTweenType.easeInCirc)

        .setOnComplete(() => itemInfoSlidePanel.gameObject.SetActive(false));
    }
}
