using System;
using UnityEngine;

public class ItemMoverBoat : Boat
{
    private void Awake()
    {
        _Mover.OnStartMoving += LockItem;
        _Mover.OnStopMoving += UnlockItem;
    }
}
