using System;
using UnityEngine;

public class ItemMoverBoat : Pedestal
{
    [SerializeField] protected bool _lockOnMove;
    [SerializeField] protected MoverBySpline _Mover;
    private void Awake()
    {
        if (_lockOnMove)
        {
            _Mover.OnStartMoving += LockItem;
            _Mover.OnStopMoving += UnlockItem;
        }
    }
    
    protected override void OnInteract()
    {
        _Mover.Stop();
    }

    protected override void OnInteractWithItem()
    {
        if (_Mover.IsStoppedOnPath())
        {
            _Mover.ChangeMoveDirection();
            _Mover.ContinueMove();
        }
        else _Mover.StartMove();
    }
}
