using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class PathChanger : MonoBehaviour
{
    [SerializeField] private MoverBySpline _mover;
    private void Start()
    {
        StartCoroutine(WaitAndChangePath());
    }

    public IEnumerator WaitAndChangePath()
    {
        while (true)
        {
            yield return new WaitForSeconds(15.0f);
            _mover.SetCurrentPath(1);
            yield return new WaitForSeconds(15.0f);
            _mover.SetCurrentPath(0);
        }
    }
}
