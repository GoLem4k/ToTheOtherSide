using UnityEngine;
using UnityEngine.Splines;

public class BoatMover : MonoBehaviour
{
    [Header("Splines")]
    [SerializeField] private SplineContainer[] splines;

    [Header("Movement")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool loop = false;

    private SplineContainer _currentSpline;
    private float _t;
    private bool _isMoving;

    // 👉 Вызов извне
    public void StartMove(int splineIndex)
    {
        if (splineIndex < 0 || splineIndex >= splines.Length)
        {
            Debug.LogWarning("Неверный индекс сплайна");
            return;
        }

        _currentSpline = splines[splineIndex];
        _t = 0f;
        _isMoving = true;
    }

    public void Stop()
    {
        _isMoving = false;
    }

    private void FixedUpdate()
    {
        if (!_isMoving || _currentSpline == null) return;

        // увеличиваем прогресс
        _t += speed * Time.deltaTime;

        float normalizedT = _t;

        // если сплайн не loop — ограничиваем
        if (!loop && normalizedT >= 1f)
        {
            normalizedT = 1f;
            _isMoving = false;
        }
        else if (loop)
        {
            normalizedT %= 1f;
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
}