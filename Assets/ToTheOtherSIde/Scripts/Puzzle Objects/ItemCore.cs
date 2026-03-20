using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemCore : MonoBehaviour, IInteractable
{
    [Header("Объекта")]
    [SerializeField] private ItemData itemData;
    [SerializeField] private float defaultInteractionRange = 2f;
    private IItemContainer _container;

    
    private Rigidbody _rb;
    private Collider _collider;
    private bool _isGrabbed = false;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        if (_rb == null)
        {
            Debug.LogWarning($"Rigidbody не найден на {gameObject.name}, добавляю...");
            _rb = gameObject.AddComponent<Rigidbody>();
        }
        if (_rb == null)
        {
            Debug.LogWarning($"BoxCollider не найден на {gameObject.name}, добавляю...");
            _collider = gameObject.AddComponent<BoxCollider>();
        }
    }
    
    public void SetContainer(IItemContainer container)
    {
        _container = container;
    }
    public InteractionResult Interact(InteractionContext context)
    {
        // если держим объект -> бросить -> результат: игрок избавляется от объекта в памяти
        if (_isGrabbed)
        {
            Drop();
            return new InteractionResult { ConsumedHeldItem = true };
        }

        // в руках нет объекта -> стать взятым -> результат: игрок должен запомнить взятый объект
        if (context.HeldItem == null)
        {
            Grab(context.Interactor);
            return new InteractionResult { TakenItem = this };
        }

        return default;
    }

    // private void Grab(GameObject interactor)
    // {
    //     gameObject.transform.parent = interactor.transform.Find("Body/GrabPointer");
    //     transform.localPosition = Vector3.zero;
    //     transform.localRotation = Quaternion.identity;
    //     SetGrabbedState(true);
    //     Debug.Log($"Объект {itemData.id} взят в руки.");
    // }
    
    public void Grab(GameObject interactor)
    {
        // если предмет был в контейнере — уведомляем его
        _container?.RemoveItem(this);
        _container = null;

        transform.SetParent(interactor.transform.Find("Body/GrabPointer"));
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        
        SetGrabbedState(true);

        Debug.Log($"Объект {itemData.id} взят в руки.");
    }

    public void Drop()
    {
        gameObject.transform.SetParent(null);
        SetGrabbedState(false);
        Debug.Log($"Объект {itemData.id} брошен.");
    }
    
    public void SetGrabbedState(bool grabbed)
    {
        _isGrabbed = grabbed;
        
        _rb.isKinematic = grabbed;
        _rb.useGravity = !grabbed;
        //_collider.enabled = !grabbed;

        if (grabbed) Freeze();
            else Unfreeze();
        
        gameObject.layer = grabbed
            ? LayerMask.NameToLayer("HeldItem")
            : LayerMask.NameToLayer("Item");
    }
    
    public void Freeze()
    {
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void Unfreeze()
    {
        _rb.constraints = RigidbodyConstraints.None;
    }
    
    // // Предмет можно взять если он не взят и если расстояние между объектом и игроком позволяет сделать это
    // public bool CanInteract(GameObject interactor, InteractionType type)
    // {
    //     switch (type)
    //     {
    //         case InteractionType.Grab:
    //             if (_isGrabbed)
    //             {
    //                 Debug.LogWarning("Объект уже взят!");
    //                 return false;
    //             }
    //             if (Vector3.Distance(transform.position, interactor.transform.position) <= itemData.interactionRange)
    //             {
    //                 Debug.LogWarning("Объект вне зоны взаимодействия");
    //                 return false;
    //             }
    //             return true;
    //         default:
    //             Debug.LogWarning($"Взаимодействие отклонено, код взаимодействия: {type}");
    //             break;
    //     }
    //     return false;
    // }
    //
    // // По уполчанию предмет можно только взять
    // public void Interact(GameObject interactor, InteractionType type)
    // {
    //     switch (type)
    //     {
    //         case InteractionType.Grab:
    //             gameObject.transform.parent = interactor.transform.Find("GrabPointer");
    //             transform.position = interactor.transform.position;
    //             _isGrabbed = true;
    //             break;
    //         default:
    //             Debug.LogWarning($"Взаимодействие отклонено, код взаимодействия: {type}");
    //             break;
    //     }
    // }
    //
    // public void Drop(GameObject grabber)
    // {
    //     if (!_isGrabbed)
    //     {
    //         Debug.LogWarning("Объект не взят");
    //     }
    //     
    //     _isGrabbed = false;
    //     
    //     // Включаем физику
    //     transform.SetParent(null);
    //     _rb.isKinematic = false;
    //     _rb.useGravity = true;
    //     
    //     // Небольшой толчок при броске
    //     _rb.AddForce(grabber.transform.forward * 5f, ForceMode.Impulse);
    //     
    //     Debug.Log($"[{gameObject.name}] Брошен");
    // }
}