using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private List<Pedestal> _pedestals;
    [SerializeField] private List<Task> _taskList;
    [SerializeField] private GameObject _portalTrigger;
    
    private void Awake()
    {
        foreach (Pedestal pedestal in _pedestals)
        {
            pedestal.OnItemPlaced += AddItemToList;
            pedestal.OnItemTaked += RemoveItemFromList;
        }
    }

    private void AddItemToList(ItemCore item)
    {
        foreach (Task task in _taskList)
        {
            if (task.ItemType == item.GetItemType())
            {
                task.ItemAmount -= 1;
            }
        }
        CheckForTaskIsComplite();
    }

    private void RemoveItemFromList(ItemCore item)
    {
        foreach (Task task in _taskList)
        {
            if (task.ItemType == item.GetItemType())
            {
                task.ItemAmount += 1;
            }
        }
    }
    
    private void CheckForTaskIsComplite()
    {
        foreach (Task task in _taskList)
        {
            if (task.ItemAmount > 0) return;
        }

        foreach (Pedestal pedestal in _pedestals)
        {
            pedestal.LockItem();
        }
        StartCoroutine(WaitAndActivate());
    }

    private IEnumerator WaitAndActivate()
    {
        yield return new WaitForSeconds(0.25f);
        _portalTrigger.SetActive(true);
        Debug.Log("Портал активирован");
    }
    
}

[Serializable]
public class Task
{
    public ItemType ItemType;
    public int ItemAmount;
}
