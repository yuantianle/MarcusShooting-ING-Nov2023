using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Marcus : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _anim;

    private float _speed = 2f;
    private float _jumpForce = 12f;
    private float _horizontalVal;
    private float _modelScale = 6f;
    private float _runSpeedTime = 2f;

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    private float _groundCheckRadius = 0.2f;
    private bool _isRunning;
    private bool _isGrounded;
    private bool _isJump;
    private bool _isPaused;
    public bool _isRecoil;
    public float recoilForce = 10f;
    private bool _showGoldvfx = true;
    private VisualEffect _goldVFX;

    public bool playerDirection { get; private set; } = true; //false: left, true: right, 

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _goldVFX = GetComponent<VisualEffect>();
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
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _anim.SetBool("Jump", true);
            _isJump = true;
        }
        _anim.SetFloat("yVelocity", _rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isPaused = !_isPaused;
            Time.timeScale = _isPaused ? 0 : 1;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            _showGoldvfx = !_showGoldvfx;
            if (_showGoldvfx) _goldVFX.Play();
            else _goldVFX.Stop();
        }
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
        //if step on enemy, enemy trigger hit animation
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.GetComponent<EnemyAI>().GetHit(0f);
            }
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
        Vector2 Velocity = new Vector2(velocityX, _rb.velocity.y);
        _rb.velocity = Velocity;

        if (direction < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1) * _modelScale;
            playerDirection = false;
        }
        else if (direction > 0)
        {
            transform.localScale = new Vector3(1, 1, 1) * _modelScale;
            playerDirection = true;
        }

        _anim.SetFloat("xVelocity", Mathf.Abs(direction));


        #endregion

        #region jump
        if (_isGrounded && _isJump)
        {
            AudioManager.Instance.PlayMarcusSFX("Jump");
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _isJump = false;
        }

        if (_isRecoil)
        {
            Vector2 recoilDirection = playerDirection ? Vector2.left : Vector2.right;
            _rb.AddForce(recoilDirection * recoilForce, ForceMode2D.Impulse);
            _isRecoil = false;
        }

        #endregion
    }

    public void PlayWalkSound()
    {
        if (Mathf.Abs(_rb.velocity.x) >= 2.4f) AudioManager.Instance.PlayMarcusSFX("Walk");
    }
}
