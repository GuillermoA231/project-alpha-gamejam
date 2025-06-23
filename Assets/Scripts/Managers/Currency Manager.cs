using UnityEngine;
using System;
using NaughtyAttributes;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    [field: SerializeField] public int Currency { get; private set; }

    [Header(" Actions")]
    public static Action onUpdated;


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

    [Button]
    private void DebugAdd500Currency()
    {
        AddCurrency(500);
    }

    public void AddCurrency(int amount)
    {
        Currency += amount;
        UpdateCurrencyText();

        onUpdated?.Invoke();
    }

    public void UseCurrency(int price) => AddCurrency(price);

    private void UpdateCurrencyText()
    {
        CurrencyText[] currencyTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (CurrencyText text in currencyTexts)
            text.UpdateText(Currency.ToString());
    }
    public bool HasEnoughCurrency(int price)
    {
        return Currency >= price;
    }
    public void UseCoins(int price)
    {
        AddCurrency(-price);
    }
}
