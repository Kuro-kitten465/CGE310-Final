using UnityEngine;
using Kuro.GameSystem;
using Kuro.Components;

namespace Kuro.Dialogue
{
    public class DialogueTrigger : MonoBehaviour, IInteractable
    {
        public DialogueData dialogue;

        [Header("Trigger Settings")]
        public bool triggerOnStart = false;
        public bool triggerOnCollision = false;
        public bool triggerOnInteract = false;
        public string requiredFlag = ""; // Flag that must be set to trigger this dialogue

        private void Start()
        {
            if (triggerOnStart)
            {
                TriggerDialogue();
            }
        }

        public void Interact(PlayerManager manager)
        {
            if (triggerOnInteract)
            {
                TriggerDialogue();
            }
        }

        private void OnDialogue(Collider2D other)
        {
            if (triggerOnCollision && other.CompareTag("Player"))
            {
                TriggerDialogue();
            }
        }

        public void TriggerDialogue()
        {
            // Check required flag if specified
            if (!string.IsNullOrEmpty(requiredFlag) && !GameState.Instance.GetFlag(requiredFlag))
            {
                Debug.Log($"Dialogue trigger skipped - missing required flag: {requiredFlag}");
                return;
            }

            if (dialogue != null && !DialogueManager.Instance.IsDialogueActive())
            {
                DialogueManager.Instance.StartDialogue(dialogue);
            }
        }
    }
}
