using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;

    private Rigidbody2D _rb;
    private Animator _animator;
    private bool _isRight = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");

        if (moveInput != 0)
        {
            _animator.StopPlayback();
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

        _rb.linearVelocityX = moveInput * _moveSpeed * Time.deltaTime;
    }
}
