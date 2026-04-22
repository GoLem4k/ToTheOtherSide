using UnityEngine;

public class MenuCursor : MonoBehaviour
{
    void Start()
    {
        // Принудительно возвращаем курсор
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}