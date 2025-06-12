// ===================================
// Author: Andrada Joaquín Guillermo
// Script: DroppableCurrency
// Type: MonoBehaviour
//
// Description:
// Abstract base class for currency objects collectible by the player.
// Designed to be inherited by concrete currency types, providing shared functionality
// such as smooth movement towards the player on collection, and requiring
// subclasses to implement specific behavior via abstract methods.
//
// Course: Tabsil Unity 2D Game - Kawaii Survivor - The Coolest Roguelike Ever
//
// Date: 27/05/2025
// ===================================


using System.Collections;
using UnityEngine;

public abstract class DroppableCurrency : MonoBehaviour , ICollectable
{
    private bool collected;

    private void OnEnable()
    {
        collected = false;
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

        while (timer < 1)
        {
            Vector2 targetPosition = player.GetShootPosition();
            transform.position = Vector2.Lerp(initialPosition, targetPosition, timer);
            timer += Time.deltaTime;
            yield return null;
        }

        Collected();
    }

    protected abstract void Collected();
}
