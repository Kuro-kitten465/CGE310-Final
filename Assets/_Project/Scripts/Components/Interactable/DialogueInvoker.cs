using Kuro.Components;
using Kuro.Dialogue;
using UnityEngine;

public class DialogueInvoker : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueData _dialogueData;

    public void Interact(PlayerManager manager)
    {
        if (_dialogueData == null)
        {
            Debug.LogError("Dialogue data is not assigned.");
            return;
        }

        DialogueManager.Instance.StartDialogue(_dialogueData);
    }
}
