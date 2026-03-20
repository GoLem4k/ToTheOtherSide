using UnityEngine;

public class InteractionContext
{
    public GameObject Interactor; // кто взаимодействует
    public ItemCore HeldItem;     // объект в руках
}

public struct InteractionResult
{
    public ItemCore TakenItem;   // игрок получил предмет
    public bool ConsumedHeldItem; // игрок потерял предмет (поставил, использовал)
}