using Kuro.Components;
using UnityEngine;
using Kuro.GameSystem;
using Kuro.Utilities.DesignPattern;
using Unity.Cinemachine;
using Kuro.Dialogue;

public class Wrapper : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _objectToWrap;
    [SerializeField] private Transform _targetTransform;
    //[SerializeField] private bool _lockCameraOnStart = false;
    [SerializeField] private DynamicCameraControl _newActiveCam;
    [SerializeField] private DynamicCameraControl _oldActiveCam;

    [Header("Flags")]
    [SerializeField] private bool _checkFlag = false;
    [SerializeField] private string _flagToCheck = "None";
    [SerializeField] private GameManager.FlagType _flagType = GameManager.FlagType.NOT_FOUND;
    [SerializeField] private bool _flagValueBool;
    [SerializeField] private int _flagValueInt;
    [SerializeField] private float _flagValueString;
    [SerializeField] private bool _triggerOneTimeOnly = false;
    [SerializeField] private DialogueData dialogueData;
    private bool _hasTriggered = false;
    private bool _hasPass = false;

    [Header("Fade References")]
    [SerializeField] private float _fadeDuration = 2f;
    [SerializeField] private FadingHandler _fadingHandler;

    public bool TriggerOneTimeOnly => _triggerOneTimeOnly;
    public bool HasTriggered => _hasTriggered;

    public void Interact(PlayerManager manager)
    {
        if (_objectToWrap == null || _targetTransform == null)
        {
            Debug.LogError("Object to wrap or target transform is not assigned.");
            return;
        }

        if (_checkFlag)
        {
            var flag = GameManager.Instance.GetFlag(_flagToCheck, out var value);
            if (dialogueData != null && flag == GameManager.FlagType.NOT_FOUND)
            {
                return;
            }

            if (flag == GameManager.FlagType.BOOL && value is bool boolValue && boolValue != _flagValueBool)
            {
                DialogueManager.Instance.StartDialogue(dialogueData);
                return;
            }
            else if (flag == GameManager.FlagType.INT && value is int intValue && intValue != _flagValueInt)
            {
                DialogueManager.Instance.StartDialogue(dialogueData);
                return;
            }
            else if (flag == GameManager.FlagType.STRING && value is string stringValue && stringValue != _flagValueString.ToString())
            {
                DialogueManager.Instance.StartDialogue(dialogueData);
                return;
            }

            _hasPass = true;
        }

        if (_triggerOneTimeOnly && _hasTriggered) return;
        if (_checkFlag && !_hasPass) return;

        _hasTriggered = true;

        EventBus.Publish(EventCollector.StopEvent);

        _fadingHandler.OnFadeCompleted += WrapToTarget;
        _fadingHandler.StartFade(_fadeDuration, FadingHandler.FadingType.FadeIn);
    }

    private void WrapToTarget()
    {
        // Wrap the object around the target transform
        Vector3 offset = _targetTransform.position - _objectToWrap.transform.position;
        _objectToWrap.transform.position += offset;

        _oldActiveCam.SetActiveCamera(false);
        _newActiveCam.SetActiveCamera(true);

        _fadingHandler.OnFadeCompleted -= WrapToTarget;
        _fadingHandler.StartFade(_fadeDuration, FadingHandler.FadingType.FadeOut);
        EventBus.Publish(EventCollector.ContinueEvent);
    }
}
