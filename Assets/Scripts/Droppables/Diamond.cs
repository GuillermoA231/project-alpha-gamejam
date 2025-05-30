using System.Collections;
using UnityEngine;
using System;

public class Diamond : DroppableCurrency
{
    [Header(" Actions ")]
    public static Action<Diamond> onCollected;
    protected override void Collected()
    {
        onCollected?.Invoke(this);
    }
}
