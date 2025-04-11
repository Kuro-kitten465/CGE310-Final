using UnityEngine;
using Kuro.Components;

public class PlayerManager : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float _detectionRadius = 0.5f;
    [SerializeField] private LayerMask _interactLayer;
    [Header("Input Settings")]
    [SerializeField] private KeyCode[] _interactKeys = { KeyCode.F, KeyCode.Space };
    [SerializeField] private KeyCode[] _cancelKeys = { KeyCode.Escape, KeyCode.X };

    [Header("References")]
    [SerializeField] private GameObject _interactBTN;

    private GameObject _currentInteractable;
    private Collider2D _currentHit;
    private PlayerController _playerController;

    public PlayerController PlayerController => _playerController;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();    
    }

    private void Update()
    {
        if (!_playerController._canMove) return;

        var hit = Physics2D.OverlapCircle(transform.position, _detectionRadius, _interactLayer);

        if (hit is null)
        {
            if (_currentInteractable != null)
                Destroy(_currentInteractable);
            
            return;
        }

        if (hit.TryGetComponent<IInteractable>(out var interactable))
        {
            if (interactable.HasTriggered && interactable.TriggerOneTimeOnly)
                return;

            if (_currentHit is null || _currentHit != hit)
            {
                _currentHit = hit;
                if (_currentInteractable != null)
                    Destroy(_currentInteractable);
            }
                

            if (_currentInteractable == null)
                _currentInteractable = Instantiate(
                    _interactBTN,
                    new Vector3(hit.transform.position.x, hit.bounds.max.y, hit.transform.position.z),
                    Quaternion.identity
                );

            if (Input.GetKeyDown(_interactKeys[0]) || Input.GetKeyDown(_interactKeys[1]))
            {
                interactable.Interact(this);
                Destroy(_currentInteractable);
            }
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
