using UnityEngine;

public interface IGrabbable : IInteractable
{
    void Drop(GameObject grabber);
}