
using UnityEngine;
using UnityEngine.UI;

public class IronCounter : MonoBehaviour
{
    public static IronCounter Instance { get; private set; }

    public int totalIron = 0;
    public Text ironText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void AddIron(int amount)
    {
        totalIron += amount;
        UpdateUI();
    }

    public void SpendIron(int amount)
    {
        totalIron -= amount;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (ironText != null)
            ironText.text = "Hierro" + totalIron.ToString();
    }
}
