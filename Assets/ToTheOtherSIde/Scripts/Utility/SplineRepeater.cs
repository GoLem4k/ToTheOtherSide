using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineRepeater : MonoBehaviour
{
    [SerializeField] private SplineContainer _mainSpline;
    [SerializeField] private SplineContainer[] _subSplines;
    [SerializeField] private float _deltaY;
    private void OnValidate()
    {
        foreach (var spline in _subSplines)
        {
            spline.Spline = _mainSpline.Spline;
            spline.transform.position = _mainSpline.transform.position + new Vector3(0, _deltaY, 0);
        }
    }
}
