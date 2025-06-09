using UnityEngine;
using UnityEngine.Pool;
public class DamageTextManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private DamageText damageTextPrefab;

    private ObjectPool<DamageText> damageTextPool;

    void Awake()
    {
        Enemy.onDamageTaken += EnemyHitCallback;
        PlayerHealth.onAttackDodged += AttackDodgedCallback;
        damageTextPool = new ObjectPool<DamageText>(
            CreateFunction,
            ActionOnGet,
            ActionOnRelease,
            ActionOnDestroy
        );
    }
    private void OnDestroy()
    {
        Enemy.onDamageTaken -= EnemyHitCallback;
        PlayerHealth.onAttackDodged -= AttackDodgedCallback;
    }


    private DamageText CreateFunction()
    {
        return Instantiate(damageTextPrefab, transform);
    }

    private void ActionOnGet(DamageText dt)
    {
        if (dt != null)
            dt.gameObject.SetActive(true);
    }

    private void ActionOnRelease(DamageText dt)
    {
        if (dt != null)
            dt.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(DamageText dt)
    {
        if (dt != null)
            Destroy(dt.gameObject);
    }

    private void EnemyHitCallback(int damage, Vector2 enemyPosition, bool isCritical)
    {
        DamageText dt = damageTextPool.Get();
        if (dt == null) return;

        // position and animate
        Vector3 spawnPos = enemyPosition + Vector2.up * Random.Range(0.3f, 1.3f);
        dt.transform.position = spawnPos;
        dt.AnimatePopUp(damage.ToString(), isCritical);

        // schedule pool-release *on the dt's GameObject*, so it
        // cancels itself if that object is destroyed
        LeanTween.delayedCall(
            dt.gameObject,
            2f,
            () =>
            {
                // safety check
                if (dt != null)
                    damageTextPool.Release(dt);
            }
        );
    }

    private void AttackDodgedCallback(Vector2 playerPosition)
    {
        DamageText dt = damageTextPool.Get();
        if (dt == null) return;

        Vector3 spawnPos = playerPosition + Vector2.up * Random.Range(0.3f, 1.6f);
        dt.transform.position = spawnPos;
        dt.AnimatePopUp("Dodged", false);

        LeanTween.delayedCall(
            dt.gameObject,
            2f,
            () =>
            {
                if (dt != null)
                    damageTextPool.Release(dt);
            }
        );
    }
}
