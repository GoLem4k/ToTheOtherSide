using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    InteractionResult Interact(InteractionContext context);
}
// public enum InteractionType
// {
//     Grab = KeyCode.E,
//     Put = KeyCode.E,
//     Drop = KeyCode.G
// }