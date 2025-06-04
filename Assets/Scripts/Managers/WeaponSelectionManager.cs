using System.Runtime.CompilerServices;
using UnityEngine;

public class WeaponSelectionManager : MonoBehaviour, IGameStateListener
{
    [Header(" Elements")]
    [SerializeField] private Transform containersParent;
    [SerializeField] private WeaponSelectionContainer weaponContainerPrefab;
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
            case GameState.WEAPONSELECTION:
                Configure();
                break;

        }
    }

    private void Configure()
    {
        CleanContainerChildren();

        for (int i = 0; i < 3; i++)
        {
            GenerateWeaponContainer();
        }

    }

    private void CleanContainerChildren()
    {
        while (transform.childCount > 0)
        {
            Transform child = transform.transform.GetChild(0);
            child.SetParent(null);
            Object.Destroy(child.gameObject);
        }
    }

    private void GenerateWeaponContainer()
    {
        WeaponSelectionContainer containerInstance = Instantiate(weaponContainerPrefab, containersParent);
    }

}
