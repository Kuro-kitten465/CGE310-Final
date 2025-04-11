using Kuro.Components;
using UnityEngine;

public class UmbrellaPickup : MonoBehaviour, IInteractable
{
    [Header("Umbrella Pickup")]
    [SerializeField] private bool _triggerOneTimeOnly = false;
    public bool TriggerOneTimeOnly => _triggerOneTimeOnly;

    [SerializeField] private Sprite spriteAfterPickup;

    private bool _hasTriggered = false;
    public bool HasTriggered => _hasTriggered;

    public void Interact(PlayerManager manager)
    {
        if (_triggerOneTimeOnly) _hasTriggered = true;

        GetComponent<SpriteRenderer>().sprite = spriteAfterPickup;
        GameManager.Instance.UpdateFlag("Umbrella", true);
    }
}
