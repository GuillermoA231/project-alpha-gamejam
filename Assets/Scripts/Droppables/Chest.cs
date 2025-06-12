// ===================================
// Author: Andrada Joaquín Guillermo
// Script: Chest
// Type: MonoBehaviour
//
// Description:
// Represents a collectable chest in the game. When collected by the player, it triggers
// a global event and destroys itself from the scene.
//
// Course: Tabsil Unity 2D Game - Kawaii Survivor - The Coolest Roguelike Ever
//
// Date: 28/05/2025
// ===================================



using System;
using UnityEngine;

public class Chest : MonoBehaviour, ICollectable
{
    [Header(" Actions ")]
    public static Action<Chest> onCollected;

    public void Collect(Player player)
    {
        onCollected?.Invoke(this);
        Destroy(gameObject);
    }

}
