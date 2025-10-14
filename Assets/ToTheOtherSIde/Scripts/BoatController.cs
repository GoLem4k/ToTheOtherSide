using System;
using UnityEngine;

public class BoatController : PausedBehaviour
{

    [SerializeField] private Transform _point1;
    [SerializeField] private Transform _point2;
    [SerializeField] private Transform _boat;
    [SerializeField] private float _boatSpeed = 5f;

    [SerializeField] private Vector3 _targetPosition;

    private void Awake()
    {
        _targetPosition = _point1.position;
    }

    protected override void GameUpdate()
    {
        MoveToTarget();
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SetTarget(_point1);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            SetTarget(_point2);
        }
    }

    private void SetTarget(Transform posTransform)
    {
        _targetPosition = posTransform.position;
    }

    private void MoveToTarget()
    {
        _boat.position = Vector3.MoveTowards(
            _boat.position,
            new Vector3(_boat.position.x, _boat.position.y, _targetPosition.z),
            _boatSpeed * Time.deltaTime
        );
    }
}
