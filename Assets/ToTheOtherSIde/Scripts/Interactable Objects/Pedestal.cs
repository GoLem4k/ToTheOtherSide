using UnityEngine;

public class Pedestal : MonoBehaviour, IInteractable, IItemUser, IItemContainer
{
    private ItemCore _storedItem;
    [SerializeField] private Transform placePoint;

    // взять предмет
    public InteractionResult Interact(InteractionContext context)
    {
        if (_storedItem == null) return default;

        var item = _storedItem;
        _storedItem = null;

        item.Grab(context.Interactor);
        return new InteractionResult
        {
            TakenItem = item
        };
    }

    // поставить предмет
    public InteractionResult InteractWithItem(InteractionContext context)
    {
        if (_storedItem != null) return default;

        _storedItem = context.HeldItem;

        _storedItem.transform.SetParent(placePoint);
        _storedItem.transform.localPosition = Vector3.zero;
        _storedItem.transform.localRotation = Quaternion.identity;

        _storedItem.SetGrabbedState(false);

        // Объект узнает, что он в контейнере
        _storedItem.SetContainer(this);
        
        return new InteractionResult
        {
            ConsumedHeldItem = true
        };
    }
    
    public void RemoveItem(ItemCore item)
    {
        if (_storedItem == item)
        {
            _storedItem.Unfreeze();
            _storedItem = null;
            Debug.Log("Пьедестал: предмет удалён");
        }
    }
}