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
