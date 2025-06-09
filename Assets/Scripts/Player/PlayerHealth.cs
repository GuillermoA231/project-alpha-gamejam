using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class PlayerHealth : MonoBehaviour, IPlayerStatsDependency
{
    [Header(" Settings ")]
    [SerializeField] private int baseMaxHealth;
    private float maxHealth;
    private float health;
    private float armor;
    private float lifeSteal;
    private float dodge;
    private float healthRecoverySpeed;
    private float healthRecoveryTimer;
    private float healthRecoveryDuration;


    [Header(" Elements ")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header(" Actions ")]
    public static Action<Vector2> onAttackDodged;


    private void Awake()
    {
        Enemy.onDamageTaken += EnemyTookDamageCallback;
    }
    private void OnDestroy()
    {
        Enemy.onDamageTaken -= EnemyTookDamageCallback;
    }
    private void EnemyTookDamageCallback(int damage, Vector2 enemyPos, bool isCriticalHit)
    {
        if (health >= maxHealth)
        {
            return;
        }

        float lifeStealValue = damage * lifeSteal;
        float healthToAdd = Mathf.Min(lifeStealValue, (maxHealth - health));

        health += healthToAdd;
        UpdateHealthUI();
    }

    void Start()
    {
    }

    void Update()
    {
        if (health < maxHealth)
        {
            RecoverHealth();
        }
    }

    private void RecoverHealth()
    {
        healthRecoveryTimer += Time.deltaTime;

        if (healthRecoveryTimer >= healthRecoveryDuration)
        {
            healthRecoveryTimer = 0;

            float healthToAdd = Mathf.Min(1f, maxHealth - health);
            health += healthToAdd;

            UpdateHealthUI();
        }
    }

    public void TakeDamage(int damage)
    {
        if (ShouldDodge())
        {
            onAttackDodged?.Invoke(transform.position);
            return;
        }

        float realDamage;
        realDamage = damage * Mathf.Clamp(1 - (armor / 1000), 0, 10000);
        realDamage = Mathf.Min(realDamage, damage);
        health -= realDamage;


        UpdateHealthUI();

        if (health <= 0)
            Death();
    }
    private bool ShouldDodge()
    {
        return Random.Range(0f, 100f) < Mathf.Clamp(dodge,0f, 70f);
    }

    private void Death()
    {
        GameManager.instance.SetGameState(GameState.GAMEOVER);
    }

    private void UpdateHealthUI()
    {
        float normalizedHealth;
        normalizedHealth = health / maxHealth;
        healthSlider.value = normalizedHealth;
        healthText.text = health.ToString("F0") + " / " + maxHealth;
    }

    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        float addedHealth = playerStatsManager.GetStatValue(Stat.MaxHealth);
        maxHealth = baseMaxHealth + (int)addedHealth;
        maxHealth = Mathf.Max(maxHealth, 1);

        health = maxHealth;
        UpdateHealthUI();

        armor = playerStatsManager.GetStatValue(Stat.Armor);
        lifeSteal = playerStatsManager.GetStatValue(Stat.Lifesteal) / 100;
        dodge = playerStatsManager.GetStatValue(Stat.Dodge);
        healthRecoverySpeed = Mathf.Max(.0001f, playerStatsManager.GetStatValue(Stat.HealthRegeneration));
        healthRecoveryDuration = 1f / healthRecoverySpeed;


    }
}
