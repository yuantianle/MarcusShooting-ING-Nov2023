using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    private Animator animator;
    private Vector3 _moveDirection;
    private bool _facingRight = true;
    private bool _isDead = false;
    private bool _hasHitGround = false;
    private bool _hasHitWall = false;
    private Material _material;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        _moveDirection = new Vector3(Random.Range(-1f, 1f), 0, 0).normalized;
        _material = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        if (!_isDead) MoveEnemy();
    }

    private void MoveEnemy()
    {
        if (_hasHitWall)
        {
            _moveDirection = Vector3.Reflect(_moveDirection, Vector3.right).normalized;
            _hasHitWall = false;
        }
        transform.position += _moveDirection * moveSpeed * Time.deltaTime;
        float speed = _moveDirection != Vector3.zero ? 1.0f : 0.0f;
        animator.SetFloat("Speed", speed);
        if ((_moveDirection.x > 0 && !_facingRight) || (_moveDirection.x < 0 && _facingRight))
        {
            //check if need to flip the sprite
            _facingRight = !_facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
    public void GetHit()
    {
        if (!_isDead)
        {
            animator.SetTrigger("Hit");
            animator.SetBool("IsDead", true);
            _isDead = true;
            moveSpeed = 0;
            StartCoroutine(FadeOut());
        }
    }
    IEnumerator FadeOut()
    {
        float duration = 10f;
        float target = 0.8f;
        float t = 0f;
        float startValue = _material.GetFloat("_DieRate");
        while (t < duration)
        {
            _material.SetFloat("_DieRate", Mathf.Lerp(startValue, target, t / duration));
            t += Time.deltaTime;
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if hit the wall layer, change direction
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            _hasHitWall = true;//_moveDirection = Vector2.Reflect(_moveDirection, collision.contacts[0].normal).normalized;
        }

        //if died, forbid enemy to move by rigidbody
        float collisionAngle = Vector2.Angle(collision.contacts[0].normal, Vector2.up);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && _isDead && collisionAngle <= 20f)
        {
            gameObject.layer = LayerMask.NameToLayer("Dead");  // so that the player can walk over the dead enemy
            _hasHitGround = true;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
            }
        }
    }
}
