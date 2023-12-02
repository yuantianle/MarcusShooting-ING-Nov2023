using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
public class BulletShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject shellPrefab;
    public float bulletSpeed { get; set; } = 20f;
    public float bulletsPerSecond { get; set; } = 10f;
    public float cameraShakeDuration { get; set; } = 0.1f;
    public float cameraShakeMagnitude { get; set; } = 0.5f;
    public int numTrack { get; set; } = 1;

    private float _bulletSpawnDistance = 1f; // bullet generation distance from player
    private Vector3 _shellEjectDirection = new Vector3(-5, 5, 0);  // direction of shell ejection
    private float _nextFireTime = 0f;
    private CameraFollow _cameraFollowComponent;
    private Marcus _player;
    private WeaponManager _weaponManager;


    void Awake()
    {
        _cameraFollowComponent = Camera.main.GetComponent<CameraFollow>();
        _player = GetComponent<Marcus>();
        _weaponManager = _player.GetComponentInChildren<WeaponManager>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.S) && Time.time > _nextFireTime)
        {
            _nextFireTime = Time.time + 1f / bulletsPerSecond;
            _cameraFollowComponent.TriggerShake(cameraShakeDuration, cameraShakeMagnitude);
            ShootBullet();
        }
    }

    void ShootBullet()
    {
        Vector3 Direction = _player.playerDirection ? Vector3.right : Vector3.left;
        Vector3 spawnPosition = transform.position + Direction * _bulletSpawnDistance;
        Weapon currentWeapon = _weaponManager._instantiatedWeapons[_weaponManager._currentWeaponIndex].GetComponent<Weapon>();
        switch (currentWeapon.weaponName)
        {
            case "Pistol":
                AudioManager.Instance.PlayWeaponSFX("Pistol");
                break;
            //case "MaskinPistol":
            //    AudioManager.Instance.PlaySFX("MaskinPistol");
            //    break;
            //case "MachineGun":
            //    AudioManager.Instance.PlaySFX("MachineGun");
            //    break;
            //case "FireGun":
            //    AudioManager.Instance.PlaySFX("FireGun");
            //    break;
            //case "TripleGun":
            //    AudioManager.Instance.PlaySFX("TripleGun");
            //    break;
            default:
                break;
        }
        AudioManager.Instance.PlayMarcusSFX("Land");
        // Shooting the bullet
        if (numTrack == 1)
        {

            float currentExplosionProbability = currentWeapon.explosionProbability;
            float currentExplosionRadius = currentWeapon.explosionRadius;
            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, transform.rotation);
            bullet.GetComponent<Bullet>().explosionProbability = currentExplosionProbability;
            bullet.GetComponent<Bullet>().explosionRadius = currentExplosionRadius;

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.velocity = Direction * bulletSpeed;
            }
        }
        else
        {
            float totalAngleRange = 10f;
            float angleStep = totalAngleRange / (numTrack - 1);
            float startAngle = -totalAngleRange / 2;
            for (int i = 0; i < numTrack; i++)
            {
                float angle = startAngle + angleStep * i;
                Vector3 bulletDirection = Quaternion.Euler(0, 0, angle) * Direction;
                GameObject bullet = Instantiate(bulletPrefab, spawnPosition, transform.rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.velocity = bulletDirection * bulletSpeed;
                }
            }
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

