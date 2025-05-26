using System.Collections;
using UnityEngine;

public class Money : MonoBehaviour
{

    private bool collected;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

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
