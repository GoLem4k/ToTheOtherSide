using System;
using UnityEngine;

public class Pedestal : MonoBehaviour, IInteractable
{
    [Header("Pedestal Setup")]
    public Transform placePoint;
    [Tooltip("Если хотите, поместите стартовый ItemData в инспекторе")]
    public ItemData currentItem;
    private GameObject placedObject;

    [Header("Start options")]
    [Tooltip("Если true — вызовет OnItemPlaced при старте, иначе только отобразит предмет без триггера")]
    public bool invokeOnStart = false;

    [Tooltip("Если true - Нельзя взять объект с пьедестала")]
    public bool locked = false;

    public bool HasItem => currentItem != null;

    // СОБЫТИЯ
    public event Action<ItemData> OnItemPlaced;
    public event Action<ItemData> OnItemRemoved;

    public virtual bool CanPlaceItem(ItemData item) => !HasItem;

    public Transform GetPlacePoint() => placePoint != null ? placePoint : transform;

    private void Start()
    {
        // Если в инспекторе задан currentItem — отобразим его
        if (currentItem != null)
        {
            // Создаём визуал (не вызываем PlaceItem, чтобы не дублировать логику событий,
            // если нужно триггерить событие при старте - используем invokeOnStart)
            SpawnPlacedObject(currentItem);

            if (invokeOnStart)
                OnItemPlaced?.Invoke(currentItem);
        }
    }

    public virtual void Interact(PlayerInteractor player)
    {
        if (HasItem && player.heldItem == null)
        {
            player.TakeItem(currentItem);
            RemoveItem();
        }
        else if (!HasItem && player.heldItem != null)
        {
            PlaceItem(player.DropItem());
        }
    }

    public bool CanInteract(PlayerInteractor player)
    {
        return ((HasItem && player.heldItem == null) ||
               (!HasItem && player.heldItem != null)) && !locked;
    }

    public virtual void PlaceItem(ItemData item)
    {
        Clear();
        currentItem = item;
        SpawnPlacedObject(item);

        // Генерируем событие — кто подписан, отреагирует
        OnItemPlaced?.Invoke(item);
    }

    public virtual void RemoveItem()
    {
        if (currentItem == null) return;

        ItemData removed = currentItem;
        Clear();

        // Генерируем событие удаления
        OnItemRemoved?.Invoke(removed);
    }

    // Вынесено в отдельный метод, чтобы избежать дублирования
    protected void SpawnPlacedObject(ItemData item)
    {
        if (item == null) return;

        // уничтожаем старую визуализацию, если есть
        if (placedObject != null) DestroyImmediate(placedObject);

        if (placePoint != null)
            placedObject = Instantiate(item.prefab, placePoint.position, placePoint.rotation, placePoint);
        else
            placedObject = Instantiate(item.prefab, transform.position, transform.rotation, transform);

        // защищаем от искажений родителя
        //placedObject.transform.localScale = Vector3.one;
    }

    public void Clear()
    {
        if (placedObject != null) Destroy(placedObject);
        currentItem = null;
    }
    
    public Transform GetTransform()
    {
        return transform;
    }

}
