using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStatsManager))]
public class PlayerObjects : MonoBehaviour
{
    [field: SerializeField] public List<ObjectDataSO> Objects { get; private set; }
    private PlayerStatsManager playerStatsManager;

    private void Awake()
    {
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }
    void Start()
    {
        foreach (ObjectDataSO objectData in Objects)
        {
            //playerStatsManager.AddObject(objectData);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
