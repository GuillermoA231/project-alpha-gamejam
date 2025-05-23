using UnityEngine;
using UnityEngine.Pool;

public class DamageTextManager : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private DamageText damageTextPrefab;

    [Header("Elements")]
    private ObjectPool<DamageText> damageTextPool;

    private void Awake()
    {

        Enemy.onDamageTaken += EnemyHitCallback;
    }

    private void OnDestroy()
    {
        Enemy.onDamageTaken -= EnemyHitCallback;
    }
    void Start()
    {
        damageTextPool = new ObjectPool<DamageText>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    private DamageText CreateFunction()
    {
        return Instantiate(damageTextPrefab, transform);
    }

    private void ActionOnGet(DamageText damageText)
    {

        damageText.gameObject.SetActive(true);
    }

    private void ActionOnRelease(DamageText damageText)
    {
        damageText.gameObject.SetActive(false);   
    }

    private void ActionOnDestroy(DamageText damageText)
    {
        Destroy(damageText.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void EnemyHitCallback(int damage, Vector2 enemyPosition)
    {

        DamageText damageTextInstance = damageTextPool.Get();

        Vector3 spawnPosition = enemyPosition + Vector2.up * Random.Range(0.3f, 1.3f);
        damageTextInstance.transform.position = spawnPosition;

        damageTextInstance.AnimatePopUp(damage);

        LeanTween.delayedCall(1, () => damageTextPool.Release(damageTextInstance));
    }
}
