using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image container;
    [SerializeField] private Image outline;
    [SerializeField] private Image icon;

    public void Configure(Color containerColor, Color outlineColor, Sprite spriteIcon)
    {
        container.color = containerColor;
        outline.color = outlineColor;
        icon.sprite = spriteIcon;
    }
    public void Configure(Weapon weapon)
    {
        Color color = ColorHolder.GetColor(weapon.Level);
        Color outlineColor = ColorHolder.GetOutlineColor(weapon.Level);

        Sprite icon = weapon.WeaponData.Sprite;

        Configure(color, outlineColor, icon);
    }
    public void Configure(ObjectDataSO objectData)
    {
        Color color = ColorHolder.GetColor(objectData.Rarity);
        Color outlineColor = ColorHolder.GetOutlineColor(objectData.Rarity);

        Sprite icon = objectData.Icon;

        Configure(color, outlineColor, icon);
    }
}
