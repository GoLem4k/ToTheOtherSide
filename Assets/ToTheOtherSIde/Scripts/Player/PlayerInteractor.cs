using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactRange = 3f;
    public LayerMask interactLayer;
    public Camera playerCamera;

    [Header("Held Item")]
    public ItemData heldItem;
    private GameObject heldVisual;
    
    [Header("Grab Point")]
    public Transform grabPoint; // точка в руках игрока

    [Header("Hands Visuals")]
    public GameObject handsDefault;
    public GameObject handsGrab;

    [Header("Hologram")]
    public HologramVisualizer hologramVisualizer;

    [Header("UI Prompt")]
    [SerializeField] private GameObject interactPrompt; // твой готовый UI элемент
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private IInteractable currentTarget;

    void Update()
    {
        HandleInteractionCheck();
        HandleInteractionInput();
        UpdateHologram();
    }

    // === Проверка наведения ===
    void HandleInteractionCheck()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, interactRange, interactLayer))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable) && interactable.CanInteract(this))
            {
                if (currentTarget != interactable)
                {
                    currentTarget = interactable;
                    ShowPrompt(true);
                }
                return;
            }
        }

        if (currentTarget != null)
        {
            currentTarget = null;
            ShowPrompt(false);
        }
    }

    // === Обработка нажатия E ===
    void HandleInteractionInput()
    {
        if (Input.GetKeyDown(interactKey) && currentTarget != null)
        {
            if (currentTarget.CanInteract(this)) currentTarget.Interact(this);
        }
    }

    // === Управление UI-подсказкой ===
    void ShowPrompt(bool show)
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(show);
    }

    // === Обновление голограммы (если держим предмет) ===
    void UpdateHologram()
    {
        if (heldItem == null)
        {
            hologramVisualizer.Hide();
            return;
        }

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, interactRange, interactLayer))
        {
            if (hit.collider.TryGetComponent(out Pedeslat pedestal))
            {
                if (pedestal.CanPlaceItem(heldItem))
                {
                    hologramVisualizer.Show(heldItem, pedestal.GetPlacePoint());
                    return;
                }
            }
        }

        hologramVisualizer.Hide();
    }

    // === Работа с предметом ===
    public void TakeItem(ItemData item)
    {
        heldItem = item;

        if (heldVisual != null)
            Destroy(heldVisual);

        // Создаём предмет в точке удержания
        heldVisual = Instantiate(item.prefab, grabPoint.position, grabPoint.rotation, grabPoint);
        heldVisual.transform.localScale = Vector3.one;

        // Меняем состояние рук
        if (handsDefault != null) handsDefault.SetActive(false);
        if (handsGrab != null) handsGrab.SetActive(true);
    }
    public ItemData DropItem()
    {
        ItemData dropped = heldItem;
        heldItem = null;

        if (heldVisual != null)
        {
            Destroy(heldVisual);
            heldVisual = null;
        }

        // Возвращаем руки в исходное положение
        if (handsGrab != null) handsGrab.SetActive(false);
        if (handsDefault != null) handsDefault.SetActive(true);

        return dropped;
    }

}
