using System;
using UnityEngine;
using UnityEngine.Splines;

public class MoverBySpline : MonoBehaviour
{
    [Header("Splines")]
    [SerializeField] private Rail[] _rails;

    [Header("Movement")]
    [SerializeField] private float speed = 2f;
    
    public event Action OnSplineChanged;
    public event Action OnStartMoving;
    public event Action OnStopMoving;

    private Quaternion _rotationOffset;
    private SplineContainer _currentSpline;
    private float _t;
    private bool _isMoving;
    private bool _isMovingBackward;
    private int _currentPathStartId;
    private int _currentPathEndId;
    private int _stopAtPointId;

    private void Awake()
    {
        _t = 0f;
        _isMoving = false;
        _isMovingBackward = false;
        _stopAtPointId = 1;
        if (_rails == null)
        {
            Debug.LogWarning("MoverBySpline splines is null");
            return;
        }
        SetCurrentPath(0);
    }

    public void StartMove()
    {
        if (_stopAtPointId != _currentPathStartId && _stopAtPointId != _currentPathEndId) return;

        if (_stopAtPointId == _currentPathStartId) _isMovingBackward = false;
        if (_stopAtPointId == _currentPathEndId) _isMovingBackward = true;

        _t = (_isMovingBackward) ? 1f : 0f;

        // сохраняем разницу между текущим поворотом и сплайном
        Vector3 forward = _currentSpline.EvaluateTangent(_t);
        if (forward != Vector3.zero)
        {
            Quaternion splineRotation = Quaternion.LookRotation(forward);
            _rotationOffset = Quaternion.Inverse(splineRotation) * transform.rotation;
        }

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
        _currentPathStartId = _rails[splineIndex].PathStartId;
        _currentPathEndId = _rails[splineIndex].PathEndId;
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
            _stopAtPointId = _currentPathEndId;
            _isMovingBackward = true;
        }
        
        if (_isMovingBackward && normalizedT <= 0f)
        {
            normalizedT = 0f;
            Stop();
            _stopAtPointId = _currentPathStartId;
            _isMovingBackward = false;
        }

        // позиция
        Vector3 position = _currentSpline.EvaluatePosition(normalizedT);
        transform.position = position;

        // направление (для поворота)
        Vector3 forward = _currentSpline.EvaluateTangent(normalizedT);
        if (forward != Vector3.zero)
        {
            Quaternion splineRotation = Quaternion.LookRotation(forward);
            transform.rotation = splineRotation * _rotationOffset;
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
    public int PathStartId;
    public int PathEndId;
}
