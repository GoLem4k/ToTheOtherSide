using System;
using UnityEngine;

public class PlayerBoat : MonoBehaviour
{
    [SerializeField] private MoverBySpline _moverBySpline;
    [SerializeField] private SmartArea _SmartArea;

    private void Awake()
    {
        _SmartArea.OnPlayerEnter += OnPlayerEnter;
        _SmartArea.OnPlayerExit += OnPlayerExit;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _moverBySpline.StartMove();
        }
    }

    private void OnPlayerEnter(GameObject player)
    {
        //Debug.Log("ДААААААААААА");
        if (_moverBySpline.IsStoppedOnPath())
        {
            _moverBySpline.ChangeMoveDirection();
            _moverBySpline.ContinueMove();
        }
        else _moverBySpline.StartMove();
    }
    private void OnPlayerExit(GameObject player)
    {
        //Debug.Log("НЕЕЕЕЕЕЕЕТ");
        _moverBySpline.Stop();
    }
}
