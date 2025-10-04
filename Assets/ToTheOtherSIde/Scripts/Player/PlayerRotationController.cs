using UnityEngine;

public class PlayerRotationController : PausedBehaviour
{
    [SerializeField, Range(100, 1000)] private float sensitivity = 300f;
    [SerializeField] private Transform playerBody;

    private float xRotation = 0f;
    private bool firstFrameConsumed = false;

    void Start()
    {
        // Фиксируем курсор и обнуляем повороты
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        xRotation = 0f;
        transform.localRotation = Quaternion.identity;
        playerBody.rotation = Quaternion.identity;

        // Съедаем первый импульс мыши (Unity иногда даёт "мусор" на старте)
        Input.GetAxis("Mouse X");
        Input.GetAxis("Mouse Y");
    }

    protected override void GameUpdate()
    {
        // Пропускаем первый кадр, чтобы избежать скачка камеры
        if (!firstFrameConsumed)
        {
            Input.GetAxis("Mouse X");
            Input.GetAxis("Mouse Y");
            firstFrameConsumed = true;
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // Игнорируем микродвижения мыши
        if (Mathf.Abs(mouseX) < 0.001f) mouseX = 0f;
        if (Mathf.Abs(mouseY) < 0.001f) mouseY = 0f;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}