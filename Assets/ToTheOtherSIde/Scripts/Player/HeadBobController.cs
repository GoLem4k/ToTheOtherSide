using UnityEngine;

public class HeadBobController : PausedBehaviour
{
    [SerializeField] private Transform cameraTransform;  // Камера игрока
    [SerializeField, Range(0.01f, 0.1f)] private float bobAmplitude = 0.05f; // амплитуда покачивания
    [SerializeField, Range(1f, 10f)] private float bobFrequency = 5f;        // частота покачивания
    [SerializeField] private PlayerMovementController movementController;    // ссылка на контроллер движения

    private Vector3 startPos;
    private float bobTimer;

    private void OnEnable()
    {
        startPos = cameraTransform.localPosition; // запоминаем исходное положение камеры
    }

    protected override void GameUpdate()
    {
        Vector3 velocity = movementController.GetVelocity(); // добавим метод ниже
        float speed = velocity.magnitude;

        if (speed > 0.1f)
        {
            // Игрок движется — включаем покачивание
            bobTimer += Time.deltaTime * bobFrequency;
            float offsetY = Mathf.Sin(bobTimer) * bobAmplitude;
            float offsetX = Mathf.Cos(bobTimer * 0.5f) * bobAmplitude * 0.5f;

            cameraTransform.localPosition = startPos + new Vector3(offsetX, offsetY, 0);
        }
        else
        {
            // Игрок стоит — возвращаем камеру на место плавно
            bobTimer = 0f;
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, startPos, Time.deltaTime * 5f);
        }
    }
}