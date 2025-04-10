using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float _detectionRadius = 0.5f;
    [SerializeField] private LayerMask _interactLayer;
    [Header("Input Settings")]
    [SerializeField] private KeyCode[] _interactKeys = { KeyCode.F, KeyCode.E };
    [SerializeField] private KeyCode[] _cancelKeys = { KeyCode.Escape, KeyCode.X };

    private void Update()
    {
        var hit = Physics2D.OverlapCircle(transform.position, _detectionRadius, _interactLayer);

        if (hit is null) return;

        if (hit.TryGetComponent<IInteractable>(out var interactable))
        {
            if (Input.GetKeyDown(_interactKeys[0]) || Input.GetKeyDown(_interactKeys[1]))
                interactable.Interact(this);
        }
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
    #endif
}
