using Kuro.Components;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class KeyPickup : MonoBehaviour, IInteractable
{
    [Header("Key Pickup")]
    [SerializeField] private bool _triggerOneTimeOnly = false;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    public bool TriggerOneTimeOnly => _triggerOneTimeOnly;

    private bool _hasTriggered = false;
    public bool HasTriggered => _hasTriggered;

    [SerializeField] private PlayableDirector playableDirector;

    public void Interact(PlayerManager manager)
    {
        if (_triggerOneTimeOnly) _hasTriggered = true;

        GameManager.Instance.UpdateFlag("Key", true);
        playableDirector.Play();
        cinemachineCamera.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
