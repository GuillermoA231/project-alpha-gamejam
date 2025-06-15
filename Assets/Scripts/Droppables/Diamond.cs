// ===================================
// Author: Andrada Joaquín Guillermo
// Script: Diamond
// Type: MonoBehaviour
//
// Description:
// Inherits from DroppableCurrency. When collected, it invokes a global event to notify
// that a diamond has been picked up.
//
// Course: Tabsil Unity 2D Game - Kawaii Survivor - The Coolest Roguelike Ever
//
// Date: 28/05/2025
// ===================================



using System.Collections;
using UnityEngine;
using System;

public class Diamond : DroppableCurrency
{
    [Header(" Actions ")]
    public static Action<Diamond> onCollected;
    protected override void Collected()
    {
        
        ScoreManager.Instance.RegisterCoin(5);
        onCollected?.Invoke(this);
    }
}
