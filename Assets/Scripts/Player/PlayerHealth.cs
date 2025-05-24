using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header(" Settings ")]
    [SerializeField] private int maxHealth;
    private int health;


    [Header(" Elements ")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    void Start()
    {
        health = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        int realDamage;
        realDamage = Mathf.Min(health, damage);
        health -= realDamage;

        
        UpdateHealthUI();

        if (health < 0)
            Death();
    }

    private void Death()
    {
        Debug.Log("death");
        SceneManager.LoadScene(0);
    }

    private void UpdateHealthUI()
    {
        float normalizedHealth;
        normalizedHealth = (float)health / maxHealth;
        healthSlider.value = normalizedHealth;
        healthText.text = health + " / " + maxHealth;
    }
}
