using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : PausedBehaviour
{
    private CharacterController controller;
    private Vector3 moveDirection;

    [SerializeField, Range(1, 10)] private float speed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private float checkDistance = 1.5f;
    [SerializeField] private float checkRadius = 0.3f;

    private Transform currentPlatform;
    private Vector3 lastPlatformPosition;
    private Quaternion lastPlatformRotation;
    
    private float verticalVelocity = 0f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    { 
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Или если нужно обновить CharacterController
            controller.enabled = false;
            transform.position = new Vector3(0f, 2f, 0f);
            controller.enabled = true;
        }
    }
    
    protected override void GameFixedUpdate()
    {
        if (currentPlatform != null)
        {
            // --- движение платформы ---
            Vector3 platformDelta = currentPlatform.position - lastPlatformPosition;

            // --- вращение платформы ---
            Quaternion deltaRotation = currentPlatform.rotation * Quaternion.Inverse(lastPlatformRotation);

            // позиция игрока относительно платформы
            Vector3 relativePos = transform.position - currentPlatform.position;

            // поворачиваем эту позицию
            Vector3 rotatedRelativePos = deltaRotation * relativePos;

            // считаем итоговое смещение от вращения
            Vector3 rotationDelta = rotatedRelativePos - relativePos;

            // применяем всё вместе
            controller.Move(platformDelta + rotationDelta);
            
            // обновляем кеш
            lastPlatformPosition = currentPlatform.position;
            lastPlatformRotation = currentPlatform.rotation;
        }
        
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        Vector3 move = transform.TransformDirection(input) * speed;

        // 👉 проверяем горизонтальное движение
        Vector3 horizontalMove = new Vector3(move.x, 0, move.z) * Time.fixedDeltaTime;
        Vector3 nextPos = transform.position + horizontalMove;

        if (!CanMoveTo(nextPos))
        {
            Vector3 xMove = new Vector3(horizontalMove.x, 0, 0);
            Vector3 zMove = new Vector3(0, 0, horizontalMove.z);

            if (CanMoveTo(transform.position + xMove))
                horizontalMove = xMove;
            else if (CanMoveTo(transform.position + zMove))
                horizontalMove = zMove;
            else
                horizontalMove = Vector3.zero;
        }

        // 👉 гравитация
        if (controller.isGrounded)
        {
            verticalVelocity = -0.1f;
        }
        else
        {
            verticalVelocity += gravity * Time.fixedDeltaTime;
        }

        Vector3 finalMove = horizontalMove;
        finalMove.y = verticalVelocity * Time.fixedDeltaTime;

        controller.Move(finalMove);
        
    }
    
    private bool CanMoveTo(Vector3 position)
    {
        Vector3 origin = position + Vector3.up * 0.5f;

        // 👉 проверяем, есть ли под ногами что-то
        if (Physics.SphereCast(origin, checkRadius, Vector3.down, out RaycastHit hit, checkDistance, groundLayer))
        {
            return true; // земля или плот
        }

        // 👉 если нет земли — проверяем воду
        if (Physics.SphereCast(origin, checkRadius, Vector3.down, out RaycastHit waterHit, checkDistance, waterLayer))
        {
            return false; // вода → нельзя
        }

        return false; // обрыв → нельзя
    }
    
    
    public void SetPlatform(Transform platform)
    {
        currentPlatform = platform;

        if (platform != null)
        {
            lastPlatformPosition = platform.position;
            lastPlatformRotation = platform.rotation;
        }
    }
    

    public Vector3 GetVelocity()
    {
        Vector3 horizontal = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        return horizontal;
    }
}