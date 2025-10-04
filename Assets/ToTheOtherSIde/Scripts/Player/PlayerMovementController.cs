using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : PausedBehaviour
{
    private CharacterController controller;
    private Vector3 moveDirection;

    [SerializeField, Range(1, 10)] private float speed = 5f;
    [SerializeField] private float gravity = -9.81f;

    private float verticalVelocity = 0f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    protected override void GameFixedUpdate()
    {
        // Получаем ввод
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        // Направление в локальных координатах игрока
        Vector3 move = transform.TransformDirection(input) * speed;

        // Вертикальная скорость (гравитация)
        if (controller.isGrounded)
        {
            verticalVelocity = -0.1f; // небольшой контакт с землёй
        }
        else
        {
            verticalVelocity += gravity * Time.fixedDeltaTime;
        }

        move.y = verticalVelocity;

        // Двигаем CharacterController
        controller.Move(move * Time.fixedDeltaTime);
    }

    public Vector3 GetVelocity()
    {
        Vector3 horizontal = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        return horizontal;
    }
}