using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marcus : MonoBehaviour
{   
    private Rigidbody2D _rb;
    private Animator _anim;

    private float _speed = 2f;
    private float _jumpForce = 10f;
    private float _horizontalVal;
    private float _modelScale = 6f;
    private float _runSpeedTime = 2f;

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    private float _groundCheckRadius = 0.2f;
    private bool _isRunning;
    private bool _isGrounded;
    private bool _isJump;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }
    void Update()
    {
        _horizontalVal = Input.GetAxis("Horizontal");

        // push shift to run
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isRunning = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isRunning = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded )
        {
            _anim.SetBool("Jump", true);
            _isJump = true;
        }
        Debug.Log(_isJump);
        _anim.SetFloat("yVelocity", _rb.velocity.y);
    }

    void FixedUpdate()
    {
        GroundCheck();
        Move(_horizontalVal);
    }

    void GroundCheck()
    {
        _isGrounded = false;

        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(_groundCheck.position, _groundCheckRadius, _groundLayer);
        if (collider2Ds.Length > 0)
        {
            _isGrounded = true;
        }
        _anim.SetBool("Jump", !_isGrounded);
    }

    void Move(float direction)
    {
        #region movement
        float velocityX = direction * _speed * 100 * Time.fixedDeltaTime;
        if (_isRunning)
        {
            velocityX *= _runSpeedTime;
        }
        Vector2  Velocity = new Vector2(velocityX, _rb.velocity.y);
        _rb.velocity = Velocity;

        if (direction < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1) * _modelScale;
        }
        else if (direction > 0)
        {
            transform.localScale = new Vector3(1, 1, 1) * _modelScale;
        }

        _anim.SetFloat("xVelocity", Mathf.Abs(direction));
        #endregion

        #region jump
        if (_isGrounded && _isJump)
        {
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _isJump = false;
        }

        #endregion
    }
}
