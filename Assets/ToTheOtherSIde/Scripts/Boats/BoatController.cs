using UnityEngine;

public class BoatController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Pedestal boatPedestal;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Transform boat;
    [SerializeField] private float speed = 3f;
    [SerializeField] private BoatAreaLimiter areaLimiter;
    [SerializeField] private CharacterController playerController; // <-- вместо Transform

    private Vector3 lastBoatPos;
    private bool moving;
    private bool atPointA = true;

    private void OnEnable()
    {
        boatPedestal.OnItemPlaced += HandleItemPlaced;
        boatPedestal.OnItemRemoved += HandleItemRemoved;
    }

    private void OnDisable()
    {
        boatPedestal.OnItemPlaced -= HandleItemPlaced;
        boatPedestal.OnItemRemoved -= HandleItemRemoved;
    }

    private void HandleItemPlaced(ItemData item)
    {
        moving = true;
        areaLimiter.SetLocked(true);
        lastBoatPos = boat.position;
        boatPedestal.locked = true;
    }

    private void HandleItemRemoved(ItemData item)
    {
        if (!moving)
            areaLimiter.SetLocked(false);
    }

    private void Update()
    {
        if (!moving) return;

        Vector3 target = atPointA ? pointB.position : pointA.position;
        boat.position = Vector3.MoveTowards(boat.position, target, speed * Time.deltaTime);

        Vector3 delta = boat.position - lastBoatPos;

        // Перемещаем игрока вместе с лодкой
        if (playerController != null && playerController.enabled)
            playerController.Move(delta);

        lastBoatPos = boat.position;

        if (Vector3.Distance(boat.position, target) < 0.05f)
        {
            moving = false;
            boatPedestal.locked = false;
            atPointA = !atPointA;
            areaLimiter.SetLocked(false);
        }
    }
}