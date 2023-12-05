using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject _collisionEffectPrefab;
    [SerializeField] private GameObject _explosionPrefab; // explosion effect
    //They are for public refer not for inspector
    public float explosionProbability = 0.2f; // probability of explosion: 0-1
    public float explosionRadius = 1f; // explosion radius
    public float attack = 0f;
    private GameObject _collisionFX;

    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];

        if (_collisionEffectPrefab != null)
            _collisionFX = Instantiate(_collisionEffectPrefab, contact.point, Quaternion.identity);
        //trigger the vfx
        var vfx = _collisionFX.GetComponent<VisualEffect>();
        if (vfx != null)
        {
            Vector2 hitDirection2D = collision.relativeVelocity.normalized; // get the direction of the hit
            Vector3 hitDirection3D = new Vector3(hitDirection2D.x, 0 ,hitDirection2D.y);
            _collisionFX.transform.forward = hitDirection3D;
            vfx.Play();
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.mass = 0.3f;

        EnemyAI enemy = collision.collider.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            enemy.GetHit(attack);
            if (Random.value < explosionProbability)
            {
                if (_explosionPrefab != null)
                {
                    GameObject explosure = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                    //Aadd some random size for explosion
                    float randomSize = Random.Range(-0.2f, 0.2f);
                    _explosionPrefab.transform.localScale = new Vector3(explosionRadius, explosionRadius+ randomSize, 1);
                }
            }
        }
        Destroy(gameObject);
    }
}

