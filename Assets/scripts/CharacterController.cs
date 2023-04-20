using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private Animator _animator;
    private bool _isFacingRight = true;
    private bool _isGrounded;
    private bool _isMoving;
    private Vector2 _moveAction = new(0, 0);
    private Rigidbody2D _rb;
    private GameObject _self;

    // Start is called before the first frame update
    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _self = gameObject;
    }

    private void FixedUpdate()
    {
        if (_isGrounded)
        {
            _rb.AddForce(_moveAction * 1, ForceMode2D.Impulse);
            return;
        }

        _rb.AddForce(new Vector2(_moveAction.x * 1, 0), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer.Equals(6))
        {
            Debug.Log("player is now grounded");
            _isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer.Equals(6))
        {
            Debug.Log("no longer grounded");
            _isGrounded = false;
        }
    }

    private void OnMove(InputValue context)
    {
        _moveAction = context.Get<Vector2>();

        if (_moveAction.x < 0 && _isFacingRight)
            Flip();
        else if (_moveAction.x > 0 && !_isFacingRight) Flip();


        if (!_isMoving)
        {
            _animator.SetBool(IsRunning, true);
            _isMoving = true;
            return;
        }

        _isMoving = false;

        _animator.SetBool(IsRunning, false);
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        var currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
    }
}