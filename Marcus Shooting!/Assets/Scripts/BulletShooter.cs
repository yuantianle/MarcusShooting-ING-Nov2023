using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BulletShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 500f;
    public float bulletsPerSecond = 10f; 
    public float bulletSpawnDistance = 1f; // bullet generation distance from player
    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetKey(KeyCode.S) && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + 1f / bulletsPerSecond;
            ShootBullet();
        }
    }

    void ShootBullet()
    {
        Vector3 Direction = GetComponent<Marcus>().playerDirection ? Vector3.right : Vector3.left;
        Vector3 spawnPosition = transform.position + Direction * bulletSpawnDistance;

        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, transform.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Direction * bulletSpeed;
        }
    }
}

