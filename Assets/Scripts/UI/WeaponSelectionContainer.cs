using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WeaponSelectionContainer : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;


    
    [Header("Color")]
    [SerializeField] private Image[] containerImage;

    [field: SerializeField] public Button Button { get; private set; }

    public void Configure(Sprite sprite, string name, int level)
    {
        icon.sprite = sprite;
        nameText.text = name;

        Color imageColor = ColorHolder.GetColor(level);
        Color imageOutlineColor = ColorHolder.GetOutlineColor(level);

        foreach (Image image in containerImage)
        {
            image.color = imageColor;
            image.transform.GetChild(0).GetComponent<Image>().color = imageOutlineColor;
            
        }
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
