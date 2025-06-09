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
    public void TryAddWeapon(WeaponDataSO selectedWeapon, int selectedWeaponLevel)
    {
        weaponPositions[2].AssignWeapon(selectedWeapon.Prefab, selectedWeaponLevel);
    }
}
