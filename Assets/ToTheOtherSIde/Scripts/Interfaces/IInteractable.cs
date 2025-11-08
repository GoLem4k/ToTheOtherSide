using UnityEngine;

public interface IInteractable
{
    bool CanInteract(PlayerInteractor player);
    void Interact(PlayerInteractor player);
    Transform GetTransform(); // Для удобства, чтобы получать позицию при наведении
}