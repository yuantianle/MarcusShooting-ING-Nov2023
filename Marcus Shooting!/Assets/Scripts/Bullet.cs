using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject collisionEffectPrefab;
    public GameObject explosionPrefab; // explosion effect
    public float explosionProbability = 0.2f; // probability of explosion: 0-1
    public float explosionRadius = 1f; // explosion radius

    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];
        Instantiate(collisionEffectPrefab, contact.point, Quaternion.identity);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.mass = 0.3f;

        EnemyAI enemy = collision.collider.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            enemy.GetHit();
            if (Random.value < explosionProbability)
            {
                if (explosionPrefab != null)
                {
                    GameObject explosure = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                    explosionPrefab.transform.localScale = new Vector3(explosionRadius, explosionRadius, 1);
                }
            }
        }
        Destroy(gameObject);
    }
}

