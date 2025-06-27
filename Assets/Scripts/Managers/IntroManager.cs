using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
 
/// <summary>
///  • Shows each slide’s image & caption
///  • Auto-advances after “duration” seconds (if >0)
///  • Advances immediately on click/tap
///  • Signals GameStateController when done
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class IntroManager : MonoBehaviour, IPointerClickHandler, IGameStateListener
{
    [Header("Slides (order matters)")]
    public List<IntroUI> slides = new List<IntroUI>();
 
    [Header("UI References")]
    [Tooltip("Full-screen Image component")]
    public Image backgroundImage;
    [Tooltip("Bottom-anchored TextMeshProUGUI")]
    public TextMeshProUGUI captionText;
 
    int currentIndex = 0;
    Coroutine autoAdvance;
    CanvasGroup canvasGroup;
 
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
 
    public void StartIntro()
    {
        if (slides == null || slides.Count == 0) return;
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
 
        currentIndex = 0;
        ShowSlide(currentIndex);
    }
 
    private void ShowSlide(int index)
    {
        if (index >= slides.Count)
        {
            EndIntro();
            return;
        }
 
        var slide = slides[index];
        backgroundImage.sprite = slide.background;
        captionText.text       = slide.caption;
 
        if (autoAdvance != null)
            StopCoroutine(autoAdvance);
 
        if (slide.duration > 0f)
            autoAdvance = StartCoroutine(AutoAdvanceAfter(slide.duration));
    }
 
    IEnumerator AutoAdvanceAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        NextSlide();
    }
 
    /// <summary>
    /// Advance to the very next slide (or end).
    /// </summary>
    public void NextSlide()
    {
        currentIndex++;
        ShowSlide(currentIndex);
    }

    void EndIntro()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        var controller = Object.FindFirstObjectByType<GameManager>();
        if (controller != null)
            //controller.SetState(GameState.MENU);
            Debug.Log("ENRIQUEEE");
    }
 
    // IPointerClickHandler
    public void OnPointerClick(PointerEventData eventData)
    {
        // click anywhere on this Canvas → next slide
        NextSlide();
    }

    public void GameStateChangedCallback(GameState gameState)
    {
        throw new System.NotImplementedException();
    }
}
