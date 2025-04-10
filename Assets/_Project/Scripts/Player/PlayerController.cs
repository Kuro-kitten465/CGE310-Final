using Kuro.Utilities.DesignPattern;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;

    private Rigidbody2D _rb;
    private Animator _animator;
    private bool _isRight = false;
    public bool _canMove = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe(EventCollector.ContinueEvent, OnContinue);
        EventBus.Subscribe(EventCollector.StopEvent, OnPause);
    }

    void Update()
    {
        if (!_canMove) return;

        Move();
    }

    private void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");

        if (moveInput != 0)
        {
            _animator.Play("Walk");
        }
        else
        {
            _animator.StopPlayback();
            _animator.Play("Idle");
        }

        if (moveInput > 0 && !_isRight)
        {
            _isRight = true;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveInput < 0 && _isRight)
        {
            _isRight = false;
            transform.localScale = new Vector3(1, 1, 1);
        }

        _rb.linearVelocityX = moveInput * _moveSpeed;
    }

    private void OnContinue()
    {
        _canMove = true;
        _rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void OnPause()
    {
        _canMove = false;
        _rb.bodyType = RigidbodyType2D.Kinematic;
        _rb.linearVelocityX = 0f;
        _animator.StopPlayback();
        _animator.Play("Idle");
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(EventCollector.ContinueEvent);
        EventBus.Unsubscribe(EventCollector.StopEvent);
    }
}
