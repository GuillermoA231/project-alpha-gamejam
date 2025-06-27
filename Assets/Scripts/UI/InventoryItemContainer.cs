using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InventoryItemContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image container;
    [SerializeField] private Image outline;
    [SerializeField] private Image icon;
    [SerializeField] private Button button;

    public int Index { get; private set; }

    public Weapon Weapon { get; private set; }
    public ObjectDataSO ObjectData{ get; private set; }

    private void ConfigureGraphics(Color containerColor, Color outlineColor, Sprite spriteIcon)
    {
        container.color = containerColor;
        outline.color = outlineColor;
        icon.sprite = spriteIcon;
    }
    public void Configure(Weapon weapon, int index, Action clickedCallback)
    {
        Weapon = weapon;
        Index = index;
        
        Color color = ColorHolder.GetColor(weapon.Level);
        Color outlineColor = ColorHolder.GetOutlineColor(weapon.Level);

        Sprite icon = weapon.WeaponData.Sprite;

        ConfigureGraphics(color, outlineColor, icon);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => clickedCallback?.Invoke());
        button.onClick.AddListener(() =>
        {
            LeanTween.scale(button.gameObject, new Vector3(1.2f, 1.2f, 1f), 0.2f)
                     .setEasePunch();
            AudioManager.instance.PlayClickSound();
        });
    }
    public void Configure(ObjectDataSO objectData, Action clickedCallback)
    {
        ObjectData = objectData;

        Color color = ColorHolder.GetColor(objectData.Rarity);
        Color outlineColor = ColorHolder.GetOutlineColor(objectData.Rarity);

        Sprite icon = objectData.Icon;

        ConfigureGraphics(color, outlineColor, icon);

        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => clickedCallback?.Invoke());
        button.onClick.AddListener(() => 
        {
            LeanTween.scale(button.gameObject, new Vector3(1.2f, 1.2f, 1f), 0.2f)
                     .setEasePunch();
            AudioManager.instance.PlayClickSound();
        });
    }
}
