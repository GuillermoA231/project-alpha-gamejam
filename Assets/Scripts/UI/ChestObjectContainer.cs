using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;
using System.Collections.Generic;
using UnityEngine.Events;

public class ChestObjectContainer : MonoBehaviour
{
     [Header("Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private Image nameBackgroundColor;
    [SerializeField] private Outline containerOutline;
    [SerializeField] private TextMeshProUGUI nameText;
    [Header("Stats")]
    [SerializeField] private Transform statsContainerParent;
    [SerializeField] private TextMeshProUGUI recyclePriceText;
    [field: SerializeField] public Button InstallButton { get; private set; }
    [field: SerializeField] public Button MeltButton { get; private set; }

    [Header("Color")]
    [SerializeField] private Image[] containerImage;

    
    [Header("Audio (UI)")]
    [SerializeField] private AudioClip installClip;
    [SerializeField] private AudioClip meltClip;
    

    private string objectKey;
    private int objectLevel;

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private void OnLocaleChanged(UnityEngine.Localization.Locale _)
    {
        UpdateObjectName();
    }

    private void UpdateObjectName()
    {
        string localizationTable = "Objects";
        int displayLevel = objectLevel + 1;
        var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(localizationTable, objectKey);

        if (op.IsDone)
        {
            nameText.text = $"{op.Result} (LVL {displayLevel})";
        }
        else
        {
            op.Completed += handle =>
            {
                nameText.text = $"{handle.Result} (LVL {displayLevel})";
            };
        }
    }

    public void Configure(ObjectDataSO objectDataSO, int level)
    {
        icon.sprite = objectDataSO.Icon;
        objectLevel = level;
        objectKey = objectDataSO.name;
        recyclePriceText.text = objectDataSO.MeltPrice.ToString();

        Color imageColor = ColorHolder.GetColor(objectDataSO.Rarity);
        Color outlineColor = ColorHolder.GetOutlineColor(objectDataSO.Rarity);
        nameText.color = imageColor;
        nameBackgroundColor.color = outlineColor;
        containerOutline.effectColor = outlineColor;

        foreach (Image img in containerImage)
            img.color = imageColor;

        UpdateObjectName();

        GenerateStatContainers(objectDataSO.baseStats);
    }
    private void GenerateStatContainers(Dictionary<Stat,float> stats)
    {
        StatContainerManager.GenerateStatContainers(stats, statsContainerParent);
    }

     public void SetupButtonCallbacks(UnityAction installAction, UnityAction meltAction)
    {
        InstallButton.onClick.RemoveAllListeners();
        MeltButton.onClick.RemoveAllListeners();

        // INSTALL
        InstallButton.onClick.AddListener(() =>
        {
            // pick override or AudioManager.default
            var clip = installClip != null
                ? installClip
                : AudioManager.Instance.buyClip;
            AudioManager.Instance.PlaySFX2D(clip);
            installAction?.Invoke();
        });

        // MELT
        MeltButton.onClick.AddListener(() =>
        {
            var clip = meltClip != null
                ? meltClip
                : AudioManager.Instance.sellClip;
            AudioManager.Instance.PlaySFX2D(clip);
            meltAction?.Invoke();
        });
    }

    public void Select()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one * 1.025f, .3f)
                 .setEase(LeanTweenType.easeInOutSine);
    }

    public void Deselect()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one, .3f)
                 .setEase(LeanTweenType.easeInOutSine);
    }
}
