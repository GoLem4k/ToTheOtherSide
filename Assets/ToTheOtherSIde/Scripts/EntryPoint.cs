using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private Pedestal[] _sideAPedestals;
    [SerializeField] private Pedestal[] _sideBPedestals;

    private List<ItemData> _itemBufferA;
    private List<ItemData> _itemBufferB;

    private void Awake()
    {
        _itemBufferA = new List<ItemData>();
        _itemBufferB = new List<ItemData>();
    }

    private void OnEnable()
    {
        foreach (var pedestal in _sideAPedestals)
        {
            pedestal.OnItemPlaced += AddItemToBufferA;
            pedestal.OnItemRemoved += RemoveItemFromBufferA;
        }

        foreach (var pedestal in _sideBPedestals)
        {
            pedestal.OnItemPlaced += AddItemToBufferB;
            pedestal.OnItemRemoved += RemoveItemFromBufferB;
        }
    }
    
    private void OnDisable()
    {
        foreach (var pedestal in _sideAPedestals)
        {
            pedestal.OnItemPlaced -= AddItemToBufferA;
            pedestal.OnItemRemoved -= RemoveItemFromBufferA;
        }

        foreach (var pedestal in _sideBPedestals)
        {
            pedestal.OnItemPlaced -= AddItemToBufferB;
            pedestal.OnItemRemoved -= RemoveItemFromBufferB;
        }
    }

    private void AddItemToBufferA(ItemData item)
    {
        _itemBufferA.Add(item);
        PrintItems();
    }

    private void AddItemToBufferB(ItemData item)
    {
        _itemBufferB.Add(item);
        PrintItems();
    }

    private void RemoveItemFromBufferA(ItemData item)
    {
        _itemBufferA.Remove(item);
        PrintItems();
    }

    private void RemoveItemFromBufferB(ItemData item)
    {
        _itemBufferB.Remove(item);
        PrintItems();
    }

    private void PrintItems()
    {
        string aItems = string.Join(", ", _itemBufferA.Select(i => i.name));
        string bItems = string.Join(", ", _itemBufferB.Select(i => i.name));
        Debug.Log($"A: [{aItems}]  B: [{bItems}]");
    }

    
    private void ClearItemBuffer()
    {
        
    }

}
