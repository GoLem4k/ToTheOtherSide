using UnityEngine;

public class Dublicator : MonoBehaviour, IInteractable, IItemUser
{
    [SerializeField] private Transform _placePoint;
    [SerializeField] private int _numberOfClones = 1;

    public InteractionResult Interact(InteractionContext context)
    {
        return default;
    }
    public InteractionResult InteractWithItem(InteractionContext context)
    {
        if (context.HeldItem != null)
        {
            if (_numberOfClones <= 0)
            {
                Debug.Log($"_numberOfClones: {_numberOfClones}");
                return default;
            }
            Debug.Log("Try to copy");
            Instantiate(context.HeldItem.GetCopy(), _placePoint.position, _placePoint.rotation);
            _numberOfClones--;
        }
        Debug.Log("HeldItem is null");
        return default;
    } 
}
