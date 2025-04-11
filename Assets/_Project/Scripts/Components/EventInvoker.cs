using Kuro.Components;
using Kuro.Dialogue;
using Kuro.GameSystem;
using UnityEngine;
using UnityEngine.Playables;

public class EventInvoker : MonoBehaviour, IInteractable
{
    private enum EventType
    {
        Cutscene,
        Dialogue,
        Quest
    }

    [Header("Event Invoker")]
    [SerializeField] private EventType eventType;
    [SerializeField] private bool triggerOneTimeOnly = false;
    public bool TriggerOneTimeOnly => triggerOneTimeOnly;
    private bool hasTriggered = false;
    public bool HasTriggered => hasTriggered;
    [SerializeField] private bool _invokeOnTrigger = false;

    [Header("References")]
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private DialogueData dialogueData;
    [SerializeField] private Quest quest;

    [Header("Flags")]
    [SerializeField] private bool _checkFlag = false;
    [SerializeField] private string _flagToCheck = "None";
    [SerializeField] private DialogueData _onFlagError;
    [SerializeField] private bool _flagValueBool;
    [SerializeField] private int _flagValueInt;
    [SerializeField] private float _flagValueString;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_invokeOnTrigger || hasTriggered) return;

        if (triggerOneTimeOnly) hasTriggered = true;

        if (collision.CompareTag("Player"))
        {
            Invoker();
        }
    }

    private bool FlagChecker()
    {
        if (!_checkFlag) return true;

        var flag = GameManager.Instance.GetFlag(_flagToCheck, out var value);
        if (dialogueData != null && flag == GameManager.FlagType.NOT_FOUND)
        {
            return false;
        }

        if (flag == GameManager.FlagType.BOOL && value is bool boolValue && boolValue != _flagValueBool)
        {
            DialogueManager.Instance.StartDialogue(_onFlagError);
            return false;
        }
        else if (flag == GameManager.FlagType.INT && value is int intValue && intValue != _flagValueInt)
        {
            DialogueManager.Instance.StartDialogue(_onFlagError);
            return false;
        }
        else if (flag == GameManager.FlagType.STRING && value is string stringValue && stringValue != _flagValueString.ToString())
        {
            DialogueManager.Instance.StartDialogue(_onFlagError);
            return false;
        }

        return true;
    }

    public void Invoker()
    {
        if (hasTriggered) return;

        if (!FlagChecker()) return;

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

        if (triggerOneTimeOnly) hasTriggered = true;
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

    public void Interact(PlayerManager manager)
    {
        if (hasTriggered) return;

        if (!FlagChecker()) return;

        if (triggerOneTimeOnly) hasTriggered = true;

        Invoker();
    }
}
