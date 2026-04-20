using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private bool _isMetamorphosed = false;
    
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

    public ItemType GetItemType()
    {
        return itemData.type;
    }
    
    // ----------Ближнее взаимодействие предметов----------
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ItemCore item))
        {
            if (!_isMetamorphosed) CheckForMetamorphosis(item.GetItemType());
            CheckForDestroy(item.GetItemType());
        }
    }

    public void CheckForMetamorphosis(ItemType itemType)
    {
        if (itemType == itemData.metamorphosisByType)
        {
            Instantiate(itemData.metamorphosisInto, transform.position, transform.rotation);
            _isMetamorphosed = true;
            OnMetamorphosis(itemType);
            Destroy(gameObject);
        }
    }
    
    public void CheckForDestroy(ItemType itemType)
    {
        if (itemType == itemData.destroyedByType)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnMetamorphosis(ItemType itemType)
    {
        Debug.Log($"Метаморфоза! Объект {itemData.id} был превращен объектом {itemType}");
    }

    public void OnDestroy()
    {
        _container?.RemoveItem(this);
        _container = null;
        Debug.Log($"Объект {itemData.id} был уничтожен!");
    }

    public GameObject GetCopy()
    {
        return itemData.prefab;
    }
}