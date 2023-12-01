using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
public class BulletShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject shellPrefab;
    private float _bulletSpeed = 20f;
    private float _bulletsPerSecond = 10f; 
    private float _bulletSpawnDistance = 1f; // bullet generation distance from player
    private Vector3 _shellEjectDirection = new Vector3(-5, 5, 0);  // direction of shell ejection
    private float _nextFireTime = 0f;
    private CameraFollow _cameraFollowComponent;
    private Marcus _player;

    void Awake()
    {
        _cameraFollowComponent = Camera.main.GetComponent<CameraFollow>();
        _player = GetComponent<Marcus>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.S) && Time.time > _nextFireTime)
        {
            _nextFireTime = Time.time + 1f / _bulletsPerSecond;
            _cameraFollowComponent.TriggerShake(0.2f, 0.9f);
            ShootBullet();
        }
    }

    void ShootBullet()
    {
        Vector3 Direction = _player.playerDirection ? Vector3.right : Vector3.left;
        Vector3 spawnPosition = transform.position + Direction * _bulletSpawnDistance;

        // Shooting the bullet
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, transform.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Direction * _bulletSpeed;
        }

        // Ejecting the shell
        Vector3 shellspawnPositon = transform.position + (-1) * Direction * (_bulletSpawnDistance);
        GameObject shell = Instantiate(shellPrefab, shellspawnPositon, Quaternion.identity);
        Rigidbody2D shellRb = shell.GetComponent<Rigidbody2D>();
        if (shellRb != null)
        {
            shellRb.mass = 0.5f;
            _shellEjectDirection.x = Mathf.Abs(_shellEjectDirection.x) * Direction.x * (-1);
            shellRb.velocity = _shellEjectDirection;
            shellRb.isKinematic = false;

        }

        // Recoil
        //change player is recolic
        _player.gameObject.GetComponent<Marcus>()._isRecoil = true;
        
    }
}

