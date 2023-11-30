using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject collisionEffectPrefab;

    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];
        Instantiate(collisionEffectPrefab, contact.point, Quaternion.identity);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.mass = 0.5f;

        EnemyAI enemy = collision.collider.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            enemy.GetHit();
        }
        Destroy(gameObject);
    }
}

