using Kuro.Dialogue;
using Kuro.GameSystem;
using UnityEngine;
using UnityEngine.Playables;

public class EventInvoker : MonoBehaviour
{
    private enum EventType
    {
        Cutscene,
        Dialogue,
        Quest
    }

    [Header("Event Invoker")]
    [SerializeField] private EventType eventType;

    [Header("References")]
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private DialogueData dialogueData;
    [SerializeField] private Quest quest;

    public void Invoker()
    {
        switch (eventType)
        {
            case EventType.Cutscene:
                InvokeCutscene();
                break;
            case EventType.Dialogue:
                InvokeDialogue();
                break;
            case EventType.Quest:
                InvokeQuest();
                break;
        }
    }

    private void InvokeCutscene()
    {
        if (playableDirector == null)
        {
            Debug.LogError("Playable Director is not assigned.");
            return;
        }

        playableDirector.Play();
    }

    private void InvokeDialogue()
    {
        if (dialogueData == null)
        {
            Debug.LogError("Dialogue data is not assigned.");
            return;
        }

        DialogueManager.Instance.StartDialogue(dialogueData);
    }

    private void InvokeQuest()
    {
        if (quest == null)
        {
            Debug.LogError("Quest data is not assigned.");
            return;
        }
    }
}
