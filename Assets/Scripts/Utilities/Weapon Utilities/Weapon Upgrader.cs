using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class WeaponUpgrader : MonoBehaviour
{
    public static WeaponUpgrader instance;

    [Header("Elements")]
    [SerializeField] private PlayerWeapons playerWeapons;

    [Header("Settings")]
    private List<Weapon> weaponsToUpgrade = new List<Weapon>();


    [Header("Actions")]
    public static Action<Weapon> onUpgrade;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    public bool CanUpgrade(Weapon weapon)
    {
        if (weapon.Level >= 3)
        {
            return false;
        }
        weaponsToUpgrade.Clear();
        weaponsToUpgrade.Add(weapon);

        Weapon[] weapons = playerWeapons.GetWeapons();

        foreach (Weapon playerWeapon in weapons)
        {
            if (playerWeapon == null)
                continue;

            if (playerWeapon == weapon)
                continue;

            if (playerWeapon.WeaponData.Name != weapon.WeaponData.Name)
                continue;

            if (playerWeapon.Level != weapon.Level)
                continue;

            weaponsToUpgrade.Add(playerWeapon);

            return true;
        }
        return false;
    }

    public void Upgrade()
    {
        if (weaponsToUpgrade.Count < 2)
        {
            Debug.LogError("WEAPONS TO UPGRADE ARE LESS THAN TWO. CHECK WEAPON UPGRADER.CS");
            return;
        }
        DestroyImmediate(weaponsToUpgrade[1].gameObject);

        weaponsToUpgrade[0].IncreaseLevel();

        Weapon weapon = weaponsToUpgrade[0];
        weaponsToUpgrade.Clear();

        onUpgrade?.Invoke(weapon);
        AudioManager.instance.PlayUpgradeSound();
    }


}
