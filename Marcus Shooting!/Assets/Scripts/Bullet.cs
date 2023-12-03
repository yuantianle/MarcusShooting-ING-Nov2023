using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class Bullet : MonoBehaviour
{
    public GameObject collisionEffectPrefab;
    public GameObject explosionPrefab; // explosion effect
    public float explosionProbability = 0.2f; // probability of explosion: 0-1
    public float explosionRadius = 1f; // explosion radius

    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];

        GameObject vfxPrefab = Instantiate(collisionEffectPrefab, contact.point, Quaternion.identity);
        //trigger the vfx
        var vfx = vfxPrefab.GetComponent<VisualEffect>();
        if (vfx != null)
        {
            Vector2 hitDirection2D = collision.relativeVelocity.normalized; // get the direction of the hit
            Vector3 hitDirection3D = new Vector3(hitDirection2D.x, 0 ,hitDirection2D.y);
            vfxPrefab.transform.forward = hitDirection3D;
            vfx.Play();
        }

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

