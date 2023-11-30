using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    private Animator animator;
    private Vector3 moveDirection;
    private bool _facingRight = true;
    private bool _isDead = false;
    private bool _hasHitGround = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        moveDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    private void Update()
    {
        if (!_isDead) MoveEnemy();
    }

    private void MoveEnemy()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        float speed = moveDirection != Vector3.zero ? 1.0f : 0.0f;
        animator.SetFloat("Speed", speed);
        if ((moveDirection.x > 0 && !_facingRight) || (moveDirection.x < 0 && _facingRight))
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
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if hit the wall, change direction
        moveDirection = Vector2.Reflect(moveDirection, collision.contacts[0].normal).normalized;

        //if died, forbid enemy to move by rigidbody
        float collisionAngle = Vector2.Angle(collision.contacts[0].normal, Vector2.up);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && _isDead && collisionAngle <= 20f)
        {
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
