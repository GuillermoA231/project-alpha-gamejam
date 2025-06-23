using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    [field: SerializeField] public int Currency { get; private set; }


    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        UpdateCurrencyText();
    }

    void Update()
    {

    }

    public void AddCurrency(int amount)
    {
        Currency += amount;
        UpdateCurrencyText();
    }

    private void UpdateCurrencyText()
    {
        CurrencyText[] currencyTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (CurrencyText text in currencyTexts)
        text.UpdateText(Currency.ToString());
    }
}
