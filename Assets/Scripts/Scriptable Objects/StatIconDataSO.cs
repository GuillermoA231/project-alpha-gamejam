using UnityEngine;


[CreateAssetMenu(fileName = "Stat Icons", menuName = "Scriptable Objects/Stat Icons", order = 0)]

public class StatIconDataSO : ScriptableObject
{
    [field: SerializeField] public StatIcon[] StatIcons {get; private set;}
}

[System.Serializable]
public struct StatIcon
{
    public Stat stat;
    public Sprite icon;
}