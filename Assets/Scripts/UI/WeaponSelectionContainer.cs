using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
public class WeaponSelectionContainer : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private Image nameBackgroundColor;
    [SerializeField] private Outline containerOutline;
    [SerializeField] private TextMeshProUGUI nameText;

    [Header("Stats")]
    [SerializeField] private Transform statsContainerParent;
    [SerializeField] private Sprite statIcon;
    
    [Header("Color")]
    [SerializeField] private Image[] containerImage;

    [field: SerializeField] public Button Button { get; private set; }

    public void Configure(Sprite sprite, string name, int level, WeaponDataSO weaponData)
    {
        icon.sprite = sprite;
        nameText.text = name + $" (LVL {level + 1})";

        Color imageColor = ColorHolder.GetColor(level);
        Color imageOutlineColor = ColorHolder.GetOutlineColor(level);
        nameText.color = imageColor;
        nameBackgroundColor.color = imageOutlineColor;
        containerOutline.effectColor = imageOutlineColor;

        foreach (Image image in containerImage)
        {
            image.color = imageColor;
            // image.transform.GetChild(0).GetComponent<Image>().color = imageOutlineColor;

        }

        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(weaponData, level);
        ConfigureStatsContainers(calculatedStats);
    }

    private void ConfigureStatsContainers(Dictionary<Stat, float> calculatedStats)
    {
        StatContainerManager.GenerateStatContainers(calculatedStats, statsContainerParent);
    }

    public void Select()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one * 1.075f, .3f).setEase(LeanTweenType.easeInOutSine);
    }
    
    public void Deselect()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one, .3f).setEase(LeanTweenType.easeInOutSine);
    }
}
