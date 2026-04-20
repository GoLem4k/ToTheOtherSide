using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class PathChanger : ButtonCore
{
    [SerializeField] private MoverBySpline _moverBySpline;
    [SerializeField] private int[] _pathsId;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private float _arrowOffset;
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
        if (_currentPathId == 0)
        {
            RotateY(_arrow.transform, -1 * _arrowOffset * (_pathsId.Length - 1));
        } else RotateY(_arrow.transform, _arrowOffset);
    }
    
    public Tween RotateY(Transform target, float additiveDegrees, float duration = 0.5f)
    {
        return target.DORotate(new Vector3(0, additiveDegrees, 0), duration, RotateMode.LocalAxisAdd);
    }
}
