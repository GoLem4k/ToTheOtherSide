using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class PathChanger : ButtonCore
{
    [SerializeField] private MoverBySpline _moverBySpline;
    [SerializeField] private int[] _pathsId;
    private int _currentPathId = 0;

    protected override void OnButtonClicked()
    {
        _currentPathId++;
        if (_currentPathId >= _pathsId.Length) _currentPathId = 0;
        if (_currentPathId < 0 || _currentPathId >= _pathsId.Length)
        {
            Debug.LogWarning("Неверный индекс для смены пути в PathChanger");
            return;
        }
        _moverBySpline.SetCurrentPath(_pathsId[_currentPathId]);
    }
}
