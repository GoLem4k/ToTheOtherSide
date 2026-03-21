using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactRange;
    public LayerMask interactLayer;
    public Camera playerCamera;

    [Header("Held Item")]
    [SerializeField] public ItemCore _heldItem; // Текущий объект в руках
    
    //[Header("Grab Point")]
    //public Transform grabPoint; // точка в руках игрока

    [Header("Hands Visuals")]
    public GameObject handsDefault;
    public GameObject handsGrab;
    
    // [Header("Interaction Buttons")]
    // [SerializeField] private KeyCode interactButtonPrimary;
    // [SerializeField] private KeyCode interactButtonSecondary;
    // [SerializeField] private KeyCode interactButtonContext;
    // [SerializeField] private KeyCode interactButtonDrop;
    
    [Header("Debug")]
    [SerializeField] private bool showDebugRay = true;
    [SerializeField] private Color rayColor = Color.green;
    [SerializeField] private Color hitColor = Color.red;

    private IInteractable _target;

    void Update()
    {
        HandleInteractionCheck();
        HandleInteractionInput();
        //UpdateHologram();
        DrawDebugRay();
    }

    private InteractionContext GetInteractionContext()
    {
        return new InteractionContext
        {
            Interactor = gameObject,
            HeldItem = _heldItem
        };
    }

    // === Проверка наведения ===
    void HandleInteractionCheck()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
        {
            // Проверяем IInteractable
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                _target = interactable;
            }
            else
            {
                ClearTarget();
            }
        }
        else
        {
            ClearTarget();
        }
    }

    private void ClearTarget()
    {
        if (_target != null)
        {
            _target = null;
        }
    }

    // === Обработка нажатий ===
    void HandleInteractionInput()
    {
        // Проверяем все клавиши
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
        else if (Input.GetKeyDown(KeyCode.G) && _heldItem != null)
        {
            _heldItem.Drop();
            _heldItem = null;
            handsGrab.SetActive(false);
            handsDefault.SetActive(true);
        }
    }

    private void TryInteract()
    {
        if (_target == null) return;

        var context = GetInteractionContext();

        InteractionResult result;

        // если держим предмет → пробуем использовать
        if (_heldItem != null && _target is IItemUser itemUser)
        {
            result = itemUser.InteractWithItem(context);
        }
        else
        {
            result = _target.Interact(context);
        }

        ApplyResult(result);
    }
    
    private void ApplyResult(InteractionResult result)
    {
        // если получили предмет
        if (result.TakenItem != null)
        {
            // запоминаем полученный предмет
            _heldItem = result.TakenItem;

            // поднимаем ручки
            handsDefault.SetActive(false);
            handsGrab.SetActive(true);
        }

        // если потеряли предмет
        if (result.ConsumedHeldItem)
        {
            _heldItem = null;

            handsGrab.SetActive(false);
            handsDefault.SetActive(true);
        }
    }
    
    // === Визуализация луча ===
    void DrawDebugRay()
    {
        if (!showDebugRay) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
        {
            // Попали в объект
            Debug.DrawLine(ray.origin, hit.point, hitColor);
            
            // Рисуем сферу в точке попадания
            DrawSphere(hit.point, 0.2f, hitColor);
            
            // Рисуем нормаль
            Debug.DrawLine(hit.point, hit.point + hit.normal * 0.5f, Color.blue);
        }
        else
        {
            // Не попали
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * interactRange, rayColor);
        }
    }

    private void DrawSphere(Vector3 position, float radius, Color color)
    {
        // Простая визуализация сферы через линии
        int segments = 16;
        float angleStep = 360f / segments;
        
        Vector3 prevPoint = position + new Vector3(Mathf.Cos(0) * radius, 0, Mathf.Sin(0) * radius);
        
        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 newPoint = position + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            Debug.DrawLine(prevPoint, newPoint, color);
            prevPoint = newPoint;
        }
    }

    // === Визуализация в редакторе ===
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}