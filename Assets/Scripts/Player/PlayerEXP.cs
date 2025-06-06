using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerEXP : MonoBehaviour
{
    [Header("Settings")]
    private int requiredXP;
    private int currentXP;
    private int level;
    private int levelsEarnedThisWave;

    [Header("Elements")]
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI lvlText;

    [Header("DEBUG")]
    [SerializeField] private bool debugMode;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        Money.onCollected += moneyCollectedCallback;
        Diamond.onCollected += diamondCollectedCallback;
    }
    private void OnDestroy()
    {

        Money.onCollected -= moneyCollectedCallback;
        Diamond.onCollected -= diamondCollectedCallback;
    }

    void Start()
    {
        UpdateRequiredXP();
        UpdateEXPUI();
    }

    void UpdateRequiredXP() => requiredXP = (level + 2) * 5;

    private void UpdateEXPUI()
    {
        float normalizedXP;
        normalizedXP = (float)currentXP / requiredXP;
        xpSlider.value = normalizedXP;
        xpText.text = currentXP + " / " + requiredXP;
        lvlText.text = "LV: " + (level + 1);
    }
    private void moneyCollectedCallback(Money money)
    {
        currentXP++;
        if (currentXP >= requiredXP)
            LevelUp();
        UpdateEXPUI();
    }
    private void diamondCollectedCallback(Diamond diamond)
    {
        currentXP += 5;
        if (currentXP >= requiredXP)
            LevelUp();
        UpdateEXPUI();
    }
    private void LevelUp()
    {
        level++;
        currentXP = 0;
        levelsEarnedThisWave++;
        UpdateRequiredXP();
        UpdateEXPUI();
    }
    public bool HasLeveledUp()
    {
        if(debugMode)
            return true;

        if (levelsEarnedThisWave > 0)
        {
            levelsEarnedThisWave--;
            return true;
        }
        return false;
    }

}
