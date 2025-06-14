using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class WeaponSelectionManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private Transform containersParent;
    [SerializeField] private WeaponSelectionContainer weaponContainerPrefab;
    [SerializeField] private PlayerWeapons playerWeapons;

    [Header("Data")]
    [SerializeField] private WeaponDataSO[] starterWeapons;

    private WeaponDataSO selectedWeapon;
    private int initialWeaponLevel;

    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GAME:
                if (selectedWeapon == null) return;
                playerWeapons.TryAddWeapon(selectedWeapon, initialWeaponLevel);
                selectedWeapon = null;
                initialWeaponLevel = 0;
                break;

            case GameState.WEAPONSELECTION:
                Configure();
                break;
        }
    }

    [Button]
    private void Configure()
    {
        CleanContainerChildren();

        for (int i = 0; i < 3; i++)
            GenerateWeaponContainer();
    }

    [Button]
    private void CleanContainerChildren()
    {
        while (containersParent.childCount > 0)
        {
            var child = containersParent.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
    }

    private void GenerateWeaponContainer()
    {
        var container = Instantiate(weaponContainerPrefab, containersParent);
        var weaponData = starterWeapons[Random.Range(0, starterWeapons.Length)];
        var level = Random.Range(0, 4);

        container.Configure(weaponData.Sprite, level, weaponData);

        container.Button.onClick.RemoveAllListeners();
        container.Button.onClick.AddListener(() =>
            WeaponSelectedCallback(container, weaponData, level)
        );
    }

    private void WeaponSelectedCallback(WeaponSelectionContainer container, WeaponDataSO weaponData, int level)
    {
        selectedWeapon = weaponData;
        initialWeaponLevel = level;

        foreach (var c in containersParent.GetComponentsInChildren<WeaponSelectionContainer>())
        {
            if (c == container) c.Select();
            else c.Deselect();
        }
    }
}
