using UnityEngine;

public class BoatController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Transform boat;
    [SerializeField] private float speed = 3f;

    [Header("Player settings")]
    [SerializeField] private CharacterController playerController;
    [SerializeField] private bool movePlayerWithBoat = true; // <-- новая переменная

    [Header("Pedestals")]
    [SerializeField] private Pedestal[] pedestals; // <-- теперь массив
    [SerializeField] private bool requireAllPedestals = true; 
    // true = требуется активировать все
    // false = достаточно любого

    [Header("Area limit")]
    [SerializeField] private BoatAreaLimiter areaLimiter;

    private Vector3 lastBoatPos;
    private bool moving;
    private bool atPointA = true;


    // private void OnEnable()
    // {
    //     foreach (var p in pedestals)
    //     {
    //         p.OnItemPlaced += HandleItemPlaced;
    //         p.OnItemRemoved += HandleItemRemoved;
    //     }
    // }
    //
    // private void OnDisable()
    // {
    //     foreach (var p in pedestals)
    //     {
    //         p.OnItemPlaced -= HandleItemPlaced;
    //         p.OnItemRemoved -= HandleItemRemoved;
    //     }
    // }
    //
    // // =============================
    // //       СТАРТ ЛОДКИ СКРИПТОМ
    // // =============================
    // public void LaunchBoat()
    // {
    //     if (moving) return;
    //     if (!CanLaunch()) return;
    //
    //     StartMoving();
    // }
    //
    // // Проверка условий запуска
    // private bool CanLaunch()
    // {
    //     if (pedestals == null || pedestals.Length == 0)
    //         return true;
    //
    //     int activated = 0;
    //     foreach (var p in pedestals)
    //     {
    //         if (p.HasItem) activated++;
    //     }
    //
    //     return requireAllPedestals
    //         ? activated == pedestals.Length
    //         : activated > 0;
    // }
    //
    //
    // private void HandleItemPlaced(ItemData item)
    // {
    //     if (CanLaunch())
    //         StartMoving();
    // }
    //
    // private void HandleItemRemoved(ItemData item)
    // {
    //     if (!moving)
    //         areaLimiter.SetLocked(false);
    // }
    //
    //
    // private void StartMoving()
    // {
    //     moving = true;
    //     lastBoatPos = boat.position;
    //     areaLimiter.SetLocked(true);
    //
    //     foreach (var p in pedestals)
    //         p.locked = true;
    // }
    //
    //
    // private void Update()
    // {
    //     if (!moving) return;
    //
    //     Vector3 target = atPointA ? pointB.position : pointA.position;
    //     boat.position = Vector3.MoveTowards(boat.position, target, speed * Time.deltaTime);
    //
    //     // Дельта перемещения лодки
    //     Vector3 delta = boat.position - lastBoatPos;
    //
    //     // Перемещаем игрока, если нужно
    //     if (movePlayerWithBoat && playerController != null && playerController.enabled)
    //         playerController.Move(delta);
    //
    //     lastBoatPos = boat.position;
    //
    //     // Достигли точки
    //     if (Vector3.Distance(boat.position, target) < 0.05f)
    //     {
    //         moving = false;
    //         atPointA = !atPointA;
    //
    //         foreach (var p in pedestals)
    //             p.locked = false;
    //
    //         areaLimiter.SetLocked(false);
    //     }
    // }
}
