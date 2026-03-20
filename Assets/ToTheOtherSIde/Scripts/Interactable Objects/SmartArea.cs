using System;
using System.Collections.Generic;
using UnityEngine;

public class SmartArea : MonoBehaviour
{
    // 👉 события
    public event Action<GameObject> OnPlayerEnter;
    public event Action<GameObject> OnPlayerExit;

    public event Action<ItemCore> OnItemEnter;
    public event Action<ItemCore> OnItemExit;

    // 👉 внутреннее состояние
    private readonly List<ItemCore> _items = new();
    private readonly HashSet<GameObject> _players = new();

    // 👉 доступ наружу
    public int ItemsCountInZone => _items.Count;
    public IReadOnlyList<ItemCore> Items => _items;

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
                Debug.Log($"Player {other.gameObject.name} exited");
            }
        }

        // --- предмет ---
        if (other.TryGetComponent(out ItemCore item))
        {
            if (IsStillInside(other)) return;
            if (_items.Remove(item))
            {
                OnItemExit?.Invoke(item);
                Debug.Log($"Item {other.gameObject.name} exited");
            }
        }
    }
    
    private bool IsStillInside(Collider target)
    {
        Collider zoneCollider = GetComponent<Collider>();

        Collider[] hits = Physics.OverlapBox(
            zoneCollider.bounds.center,
            zoneCollider.bounds.extents,
            zoneCollider.transform.rotation
        );

        foreach (var hit in hits)
        {
            if (hit == target)
                return true;
        }

        return false;
    }
}