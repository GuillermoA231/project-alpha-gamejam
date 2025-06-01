    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    public class UpgradeContainer : MonoBehaviour
    {

        [Header("Elements")]
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI upgradeNameText;
        [SerializeField] private TextMeshProUGUI upgradeValueText;
        [field: SerializeField] public Button Button { get; private set; }
        public void Configure(Sprite icon, string upgradeName, string upgradeValue)
        {
            image.sprite = icon;
            upgradeNameText.text = upgradeName;
            upgradeValueText.text = upgradeValue;
        }

    }
