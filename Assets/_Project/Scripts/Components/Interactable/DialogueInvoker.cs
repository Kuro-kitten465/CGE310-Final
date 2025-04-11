using Kuro.Components;
using Kuro.Dialogue;
using UnityEngine;

public class DialogueInvoker : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueData _dialogueData;
    [SerializeField] private bool _triggerOneTimeOnly = false;
    public bool TriggerOneTimeOnly => _triggerOneTimeOnly;

    private bool _hasTriggered = false;
    public bool HasTriggered => _hasTriggered;

    public void Interact(PlayerManager manager)
    {
        if (_dialogueData == null)
        {
            Debug.LogError("Dialogue data is not assigned.");
            return;
        }

        if (_triggerOneTimeOnly) _hasTriggered = true;

        DialogueManager.Instance.StartDialogue(_dialogueData);
    }
}
