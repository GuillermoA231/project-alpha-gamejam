using UnityEngine;

public class ColorHolder : MonoBehaviour
{
    public static ColorHolder instance;
    [Header("Elements")]
    [SerializeField] private PaletteSO palette;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }
    public static Color GetColor(int level)
    {
        level = Mathf.Clamp(level, 0, instance.palette.LevelColors.Length);
        return instance.palette.LevelColors[level];
    }


    public static Color GetOutlineColor(int level)
    {
        level = Mathf.Clamp(level, 0, instance.palette.LevelOutlineColors.Length);
        return instance.palette.LevelOutlineColors[level];
    }
}
