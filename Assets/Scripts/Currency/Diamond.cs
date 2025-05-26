using System.Collections;
using UnityEngine;

public class Diamond : MonoBehaviour
{

    private bool collected;
    public void Collect(Player player)
    {
        if (collected)
            return;
        collected = true;

        StartCoroutine(MoveTowardsPlayer(player));
    }

    IEnumerator MoveTowardsPlayer(Player player)
    {
        float timer = 0;
        Vector2 initialPosition = transform.position;

        // Vector2 targetPosition = player.GetShootPosition();

        while (timer < 1)
        {
            Vector2 targetPosition = player.GetShootPosition();
            transform.position = Vector2.Lerp(initialPosition, targetPosition, timer);
            timer += Time.deltaTime;
            yield return null;
        }

        Collected();
    }

    private void Collected()
    {
        gameObject.SetActive(false);
    }
}
