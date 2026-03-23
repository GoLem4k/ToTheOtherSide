using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartArea : MonoBehaviour
{
    public event Action<GameObject> OnPlayerEnter;
    public event Action<GameObject> OnPlayerExit;

    public event Action<ItemCore> OnItemEnter;
    public event Action<ItemCore> OnItemExit;
    
    private readonly List<ItemCore> _items = new();
    private readonly HashSet<GameObject> _players = new();
    
    public int ItemsCountInZone => _items.Count;
    public IReadOnlyList<ItemCore> Items => _items;
    
    public bool IsPlayerInZone => _players.Count > 0;

    public bool canMetamorphAndDestroy = false;
    

    // === Trigger ===
    private void OnTriggerEnter(Collider other)
    {
        // --- игрок ---
        if (other.CompareTag("Player"))
        {
            if (_players.Add(other.gameObject))
            {
                OnPlayerEnter?.Invoke(other.gameObject);
                
                Debug.Log($"Player {other.gameObject.name} entered");
            }
        }

        // --- предмет ---
        if (other.TryGetComponent(out ItemCore item))
        {
            if (!_items.Contains(item))
            {
                _items.Add(item);
                OnItemEnter?.Invoke(item);
                StartCoroutine(WaitAndForce());
                Debug.Log($"Item {other.gameObject.name} entered");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // --- игрок ---
        if (other.CompareTag("Player"))
        {
            if (_players.Remove(other.gameObject))
            {
                OnPlayerExit?.Invoke(other.gameObject);
                StartCoroutine(WaitAndForce());
                Debug.Log($"Player {other.gameObject.name} exited");
            }
        }

        // --- предмет ---
        if (other.TryGetComponent(out ItemCore item))
        {
            //if (IsStillInside(other)) return;
            if (_items.Remove(item))
            {
                OnItemExit?.Invoke(item);
                StartCoroutine(WaitAndForce());
                Debug.Log($"Item {other.gameObject.name} exited");
            }
        }
    }

    private List<ItemType> Snapshot()
    {
        List<ItemType> itemsSnapshot = new();
        foreach (ItemCore item in _items)
        {
            if (!itemsSnapshot.Contains(item.GetItemType()))
            {
                itemsSnapshot.Add(item.GetItemType());
            }
        }
        return itemsSnapshot;
    }

    private IEnumerator WaitAndForce()
    {
        yield return new WaitForSeconds(1f);
        ForceToInteract();
    }

    private void ForceToInteract()
    {
        if (_items.Count == 0) return;
        if (IsPlayerInZone) return;
        foreach (ItemCore item in _items)
        {
            foreach (ItemType type in Snapshot())
            {
                item.CheckForMetamorphosis(type);
                item.CheckForDestroy(type);
            }
        }
    }
    
    
    // private bool IsStillInside(Collider target)
    // {
    //     Collider zoneCollider = GetComponent<Collider>();
    //
    //     Collider[] hits = Physics.OverlapBox(
    //         zoneCollider.bounds.center,
    //         zoneCollider.bounds.extents,
    //         zoneCollider.transform.rotation
    //     );
    //
    //     foreach (var hit in hits)
    //     {
    //         if (hit == target)
    //             return true;
    //     }
    //
    //     return false;
    // }
}