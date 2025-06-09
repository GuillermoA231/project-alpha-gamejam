using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager Instance { get; private set; }
    public Transform Container => transform;  

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}
