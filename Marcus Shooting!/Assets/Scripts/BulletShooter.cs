using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class BulletShooter : MonoBehaviour
{
    private int _numTrack;
    private float _bulletSpawnDistance = 1f; // bullet generation distance from player
    private Vector3 _shellEjectDirection = new Vector3(-5, 5, 0);  // direction of shell ejection
    private float _nextFireTime = 0f;
    private CameraFollow _cameraFollowComponent;
    private Marcus _player;
    private Weapon _currentWeapon;

    void Awake()
    {        
        _cameraFollowComponent = Camera.main.GetComponent<CameraFollow>();
        _player = GetComponent<Marcus>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.S) && Time.time > _nextFireTime)
        {
            _currentWeapon = WeaponManager._instance._instantiatedWeapons[WeaponManager._instance._currentWeaponIndex].GetComponent<Weapon>();
            _nextFireTime = Time.time + 1f / _currentWeapon.bulletsPerSecond;
            _cameraFollowComponent.TriggerShake(_currentWeapon.cameraShakeDuration, _currentWeapon.cameraShakeMagnitude);
            ShootBullet();
        }
    }

    void ShootBullet()
    {
        Vector3 Direction = _player.playerDirection ? Vector3.right : Vector3.left;
        Vector3 spawnPosition = transform.position + Direction * _bulletSpawnDistance;
        
        _numTrack = _currentWeapon.numTrack;

        // Shooting the bullet
        if (_numTrack == 1)
        {
            switch (_currentWeapon.weaponName)
            {
                case "Pistol":
                    AudioManager.Instance.PlayWeaponSFX("Pistol");
                    break;
                case "MaskinPistol":
                    AudioManager.Instance.PlayWeaponSFX("MaskinPistol");
                    break;
                case "MachineGun":
                    AudioManager.Instance.PlayWeaponSFX("MachineGun");
                    break;
                case "FireGun":
                    AudioManager.Instance.PlayWeaponSFX("FireGun");
                    break;
                case "TripleGun":
                    AudioManager.Instance.PlayWeaponSFX("TripleGun");
                    break;
                default:
                    break;
            }
            float currentExplosionProbability = _currentWeapon.explosionProbability;
            float currentExplosionRadius = _currentWeapon.explosionRadius;
            GameObject bullet = Instantiate(_currentWeapon.bulletPrefab, spawnPosition, transform.rotation);
            bullet.GetComponent<Bullet>().explosionProbability = currentExplosionProbability;
            bullet.GetComponent<Bullet>().explosionRadius = currentExplosionRadius;

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.velocity = Direction * _currentWeapon.bulletSpeed;
            }
        }
        else
        {
            float totalAngleRange = 10f;
            float angleStep = totalAngleRange / (_numTrack - 1);
            float startAngle = -totalAngleRange / 2;
            for (int i = 0; i < _numTrack; i++)
            {
                switch (_currentWeapon.weaponName)
                {
                    case "Pistol":
                        AudioManager.Instance.PlayWeaponSFX("Pistol");
                        break;
                    case "MaskinPistol":
                        AudioManager.Instance.PlayWeaponSFX("MaskinPistol");
                        break;
                    case "MachineGun":
                        AudioManager.Instance.PlayWeaponSFX("MachineGun");
                        break;
                    case "FireGun":
                        AudioManager.Instance.PlayWeaponSFX("FireGun");
                        break;
                    case "TripleGun":
                        AudioManager.Instance.PlayWeaponSFX("TripleGun");
                        break;
                    default:
                        break;
                }
                float angle = startAngle + angleStep * i;
                Vector3 bulletDirection = Quaternion.Euler(0, 0, angle) * Direction;
                GameObject bullet = Instantiate(_currentWeapon.bulletPrefab, spawnPosition, transform.rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.velocity = bulletDirection * _currentWeapon.bulletSpeed;
                }
            }
        }

        // Gun fire effect
        GameObject gunFireVFX = Instantiate(_currentWeapon.gunFireEffect, transform.position, Quaternion.identity);
        gunFireVFX.transform.position += _currentWeapon.gunFirePositionOffset;

        // Ejecting the shell
        Vector3 shellspawnPositon = transform.position + (-1) * Direction * (_bulletSpawnDistance);
        GameObject shell = Instantiate(_currentWeapon.shellPrefab, shellspawnPositon, Quaternion.identity);
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

