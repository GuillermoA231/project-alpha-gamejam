using System.Runtime.CompilerServices;
using NaughtyAttributes;
using UnityEngine;
public class WeaponSelectionManager : MonoBehaviour, IGameStateListener
{
    [Header(" Elements")]
    [SerializeField] private Transform containersParent;
    [SerializeField] private WeaponSelectionContainer weaponContainerPrefab;
    [SerializeField] private PlayerWeapons playerWeapons;
    [Header(" Data")]
    [SerializeField] private WeaponDataSO[] starterWeapons;
    private WeaponDataSO selectedWeapon;
    private int initialWeaponLevel;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GAME:

                if (selectedWeapon == null)
                    return;

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

        for (int i = 0; i < 6; i++)
        {
            GenerateWeaponContainer();
        }

    }

    [Button]
    private void CleanContainerChildren()
    {
        Debug.Log(containersParent.childCount);
        while (containersParent.childCount > 0)
        {
            Transform child = containersParent.transform.GetChild(0);
            child.SetParent(null);
            Object.Destroy(child.gameObject);
        }
        Debug.Log(containersParent.childCount);
    }

    private void GenerateWeaponContainer()
    {
        WeaponSelectionContainer containerInstance = Instantiate(weaponContainerPrefab, containersParent);

        WeaponDataSO weaponData = starterWeapons[Random.Range(0, starterWeapons.Length)];

        int level = Random.Range(0, 4);


        containerInstance.Configure(weaponData.Sprite, weaponData.Name, level);

        containerInstance.Button.onClick.RemoveAllListeners();
        containerInstance.Button.onClick.AddListener(() => WeaponSelectedCallback(containerInstance, weaponData, level));
    }

    private void WeaponSelectedCallback(WeaponSelectionContainer containerInstance, WeaponDataSO weaponData, int level)
    {
        selectedWeapon = weaponData;
        initialWeaponLevel = level;


        foreach (WeaponSelectionContainer container in containersParent.GetComponentsInChildren<WeaponSelectionContainer>())
        {
            if (container == containerInstance)
            {
                container.Select();
            }
            else
            {
                container.Deselect();
            }
        }
    }

}
