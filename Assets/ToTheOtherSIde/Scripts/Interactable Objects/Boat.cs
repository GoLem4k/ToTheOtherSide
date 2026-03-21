using UnityEngine;

public class Boat : ItemMoverBoat
{
    [SerializeField] private BoatSmartArea _SmartArea;

    protected override void OnInteractWithItem()
    {
        if (_SmartArea.ItemsCountInZone != 1) return;
        base.OnInteractWithItem();
    }
}