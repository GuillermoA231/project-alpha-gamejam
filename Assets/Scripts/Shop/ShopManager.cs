using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class ShopManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private Transform containersParent;
    [SerializeField] private ShopObjectContainer shopItemContainerPrefab;

    public void GameStateChangedCallback(GameState gameState)
    {
        if (gameState == GameState.SHOP)
        {
            Configure();
        }
    }

    private void Configure()
    {
        List<GameObject> toDestroy = new List<GameObject>();

        for (int i = 0; i < containersParent.childCount; i++)
        {
            ShopObjectContainer shopContainer = containersParent.GetChild(i).GetComponent<ShopObjectContainer>();

            if (!shopContainer.IsLocked)
            {
                toDestroy.Add(shopContainer.gameObject);
            }
        }

        while (toDestroy.Count > 0)
        {
            Transform t = toDestroy[0].transform;
            t.SetParent(null);
            Destroy(t.gameObject);
            toDestroy.RemoveAt(0);
        }

        int containerstoAdd = Mathf.Max(0,6 - containersParent.childCount);
        // int weaponContainerCount = Random.Range(Mathf.Min(0, containerstoAdd), containerstoAdd);
         int weaponContainerCount = Random.Range(0, containerstoAdd + 1);
        int objectContainerCount = containerstoAdd - weaponContainerCount;

        for (int i = 0; i < weaponContainerCount; i++)
        {
            ShopObjectContainer weaponContainerInstance = Instantiate(shopItemContainerPrefab, containersParent);
            WeaponDataSO randomWeapon = ResourcesManager.getRandomWeapon();
            weaponContainerInstance.name = "Weapon Container " + i;
            weaponContainerInstance.Configure(randomWeapon, Random.Range(0, 3));
        }
        for (int i = 0; i < objectContainerCount; i++)
        {
            ShopObjectContainer objectContainerInstance = Instantiate(shopItemContainerPrefab, containersParent);
            ObjectDataSO randomObject = ResourcesManager.getRandomObject();
            objectContainerInstance.name = "Object Container " + i;
            objectContainerInstance.Configure(randomObject);
        }
    }

    public void Reroll()
    {
        Configure();
    }
}
