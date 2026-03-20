using System;
using UnityEngine;
using UnityEngine.Splines;

public class MoverBySpline : MonoBehaviour
{
    [Header("Splines")]
    [SerializeField] private Rail[] _rails;

    [SerializeField] private SplineInstantiate[] splinesVisual;

    [Header("Movement")]
    [SerializeField] private float speed = 2f;
    
    public event Action OnSplineChanged;
    public event Action OnStartMoving;
    public event Action OnStopMoving;

    private SplineContainer _currentSpline;
    private float _t;
    private bool _isMoving;
    private bool _isMovingBackward;

    private void Awake()
    {
        _t = 0f;
        _isMoving = false;
        _isMovingBackward = false;
        if (_rails == null)
        {
            Debug.LogWarning("MoverBySpline splines is null");
            return;
        }
        _currentSpline = _rails[0].Path;
    }

    public void StartMove()
    {
        _t = (_isMovingBackward) ? 1f : 0f;
        _isMoving = true;
        OnStartMoving?.Invoke();
    }

    public void ContinueMove()
    {
        _isMoving = true;
    }

    public bool IsStoppedOnPath()
    {
        return (!_isMoving && _t > 0f && _t < 1f);
    }

    public void ChangeMoveDirection()
    {
        _isMovingBackward = !_isMovingBackward;
    }
    public void Stop()
    {
        _isMoving = false;
        OnStopMoving?.Invoke();
    }

    public void SetCurrentPath(int splineIndex)
    {
        if (splineIndex < 0 || splineIndex >= _rails.Length)
        {
            Debug.LogWarning("Неверный индекс сплайна");
            return;
        }
        if (_currentSpline != _rails[splineIndex].Path) OnSplineChanged?.Invoke();
        foreach (Rail rail in _rails)
        {
            rail.VisualActive.SetActive(false);
            rail.VisualNotActive.SetActive(true);
        }
        _rails[splineIndex].VisualNotActive.SetActive(false);
        _rails[splineIndex].VisualActive.SetActive(true);
        _currentSpline = _rails[splineIndex].Path;
        Debug.Log($"Путь изменён на {splineIndex}");
    }
    
    private void FixedUpdate()
    {
        if (!_isMoving || _currentSpline == null) return;

        // увеличиваем прогресс
        _t += (_isMovingBackward) ? -1 *speed * Time.deltaTime : speed * Time.deltaTime;

        float normalizedT = _t;

        if (!_isMovingBackward && normalizedT >= 1f)
        {
            normalizedT = 1f;
            Stop();
            _isMovingBackward = true;
        }
        
        if (_isMovingBackward && normalizedT <= 0f)
        {
            normalizedT = 0f;
            Stop();
            _isMovingBackward = false;
        }

        // позиция
        Vector3 position = _currentSpline.EvaluatePosition(normalizedT);
        transform.position = position;

        // направление (для поворота)
        Vector3 forward = _currentSpline.EvaluateTangent(normalizedT);
        if (forward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(forward);
        }
    }

    public bool IsMoving()
    {
        return _isMoving;
    }
}

[Serializable]
public class Rail
{
    public SplineContainer Path;
    public GameObject VisualActive;
    public GameObject VisualNotActive;
}
