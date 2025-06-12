// ===================================
// Author: Andrada Joaquín Guillermo
// Script: Money
// Type: MonoBehaviour
//
// Description:
// Represents money currency that can be collected by the player.
// Invokes an event upon collection to notify other systems.
//
// Course: Tabsil Unity 2D Game - Kawaii Survivor - The Coolest Roguelike Ever
//
// Date: 28/05/2025
// ===================================
using System.Collections;
using UnityEngine;
using System;
public class Money : DroppableCurrency
{
    [Header(" Actions ")]
    public static Action<Money> onCollected;
    protected override void Collected()
    { 
        onCollected?.Invoke(this);
    }
}
