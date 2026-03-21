using UnityEngine;

public class ButtonCore : MonoBehaviour, IInteractable
{
    public virtual InteractionResult Interact(InteractionContext context)
    {
        if(context.HeldItem == null) OnButtonClicked();
        return default;
    }

    protected virtual void OnButtonClicked()
    {
        Debug.Log("Нажата кнопка");
    }
}
