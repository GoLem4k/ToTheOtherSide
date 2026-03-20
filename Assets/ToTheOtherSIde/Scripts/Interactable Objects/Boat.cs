using UnityEngine;

public class Boat : Pedestal
{
    [SerializeField] protected MoverBySpline _Mover;

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