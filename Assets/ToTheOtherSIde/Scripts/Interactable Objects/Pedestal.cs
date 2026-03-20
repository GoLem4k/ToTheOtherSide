using System;
using UnityEngine;

public class Pedestal : MonoBehaviour, IInteractable, IItemUser, IItemContainer
{
    private ItemCore _storedItem;
    [SerializeField] private Transform placePoint;
    private bool _isLocked = false;
    
    public event Action<ItemCore> OnItemPlaced;
    public event Action<ItemCore> OnItemTaked;
    // взять предмет
    public virtual InteractionResult Interact(InteractionContext context)
    {
        if (_storedItem == null) return default;
        if (_isLocked) return default;
        
        var item = _storedItem;
        OnItemTaked?.Invoke(_storedItem);
        OnInteract();

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
        
        OnItemPlaced?.Invoke(_storedItem);
        OnInteractWithItem();
        
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
            OnItemTaked?.Invoke(_storedItem);
            _storedItem = null;
            Debug.Log("Пьедестал: предмет удалён");
        }
    }

    public void LockItem()
    {
        _isLocked = true;
    }

    public void UnlockItem()
    {
        _isLocked = false;
    }

    protected virtual void OnInteract()
    {
        
    }

    protected virtual void OnInteractWithItem()
    {
        
    }
}