using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [Header(" Elements")]
    [SerializeField] private WeaponPosition[] weaponPositions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool TryAddWeapon(WeaponDataSO weapon, int weaponLevel)
    {
        for (int i = 0; i < weaponPositions.Length; i++)
        {
            if (weaponPositions[i].Weapon != null)
                continue;

            weaponPositions[i].AssignWeapon(weapon.Prefab, weaponLevel);
            return true;
        }
        return false;
    }

    public void MeltWeapon(int index)
    {

        for (int i = 0; i < weaponPositions.Length; i++)
        {
            if (i != index)
                continue;

            int meltPrice = weaponPositions[i].Weapon.GetMeltPrice();
            CurrencyManager.instance.AddCurrency(meltPrice);

            weaponPositions[i].RemoveWeapon();

            return;
        }
    }

    public Weapon[] GetWeapons()
    {
        List<Weapon> weaponsHolding = new List<Weapon>();


        foreach (WeaponPosition weaponPosition in weaponPositions)
        {
            if (weaponPosition.Weapon == null)
                weaponsHolding.Add(null);
            else
                weaponsHolding.Add(weaponPosition.Weapon);
        }

        return weaponsHolding.ToArray();
    }
    
}
