using System;
using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private List<Pedestal> _sideAPedestals;
    [SerializeField] private List<Pedestal> _sideBPedestals;
    [SerializeField] private Pedestal _controlPedestal;

    private void OnEnable()
    {
        _controlPedestal.OnItemPlaced += RealizeObjectInteractions;
    }

    private void RealizeObjectInteractions(ItemData item)
    {
        ObjectInteractions(_sideAPedestals);
        ObjectInteractions(_sideBPedestals);
    }

    private void ObjectInteractions(List<Pedestal> pedestals)
    {
        var filled = pedestals.FindAll(p => p.currentItem != null);

        filled.Sort((p1, p2) =>
            p1.currentItem.type.CompareTo(p2.currentItem.type));

        foreach (var current in filled)
        {
            foreach (var target in filled)
            {
                if (current.currentItem == null || target.currentItem == null)
                    continue;

                if (current.currentItem.destroyedByType == target.currentItem.type)
                {
                    current.RemoveItem();
                    break; // выходим из внутреннего цикла
                }
            }

            if (current.currentItem == null)
                continue; // текущий предмет уничтожён – пропускаем оставшиеся target
        }
    }

}
