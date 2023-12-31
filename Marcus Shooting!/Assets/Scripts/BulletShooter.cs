using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
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
    private bool _isShooting = false;

    //for firegun only
    private GameObject _flameBullet;
    private float _lastShellEjectTime = 0f;
    private float _shellEjectInterval = 1f;

    private Vector3 _oldAimPosition;
    private Vector3 _AimPositionOffsetRecord;


    void Awake()
    {
        _cameraFollowComponent = Camera.main.GetComponent<CameraFollow>();
        _player = GetComponent<Marcus>();
        _currentWeapon = WeaponManager._instance._instantiatedWeapons[WeaponManager._instance._currentWeaponIndex].GetComponent<Weapon>();
    }

    private void FixedUpdate()
    {
        if (_isShooting)
        {
            if (_currentWeapon.weaponName == "FireGun")
            {
                UpdateFlameBullet();
            }
            else if (_currentWeapon.weaponName != "Pistol")
            {
                if (Time.time > _nextFireTime)
                {
                    _nextFireTime = Time.time + 1f / _currentWeapon.bulletsPerSecond;
                    _cameraFollowComponent.TriggerShake(_currentWeapon.cameraShakeDuration, _currentWeapon.cameraShakeMagnitude);
                    ShootBullet();
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            _currentWeapon = WeaponManager._instance._instantiatedWeapons[WeaponManager._instance._currentWeaponIndex].GetComponent<Weapon>();

            if (_currentWeapon.weaponName == "FireGun")
            {
                ActivateFlameBullet();
            }
            else if (_currentWeapon.weaponName == "Pistol")
            {
                DiscontinueShoot();
            }
            else if (_currentWeapon.weaponName == "LaserGun")
            {
                AudioManager.Instance.PlayWeaponSFX("LaserGun");
            }

            _isShooting = true;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            if (_currentWeapon.weaponName == "FireGun")
            {
                DeactivateFlameBullet();
            }
            else if (_currentWeapon.weaponName == "LaserGun")
            {
                AudioManager.Instance.StopPlayGun("LaserGun");
            }
            _isShooting = false;
        }
    }

    private void DiscontinueShoot()
    {
        Vector3 Direction = _player.playerDirection ? Vector3.right : Vector3.left;
        Vector3 spawnPosition = transform.position + Direction * _bulletSpawnDistance;

        _numTrack = _currentWeapon.numTrack;

        // Shooting the bullet

        AudioManager.Instance.PlayWeaponSFX("Pistol");

        GameObject bullet = Instantiate(_currentWeapon.bulletPrefab, spawnPosition, transform.rotation);
        bullet.GetComponent<Bullet>().explosionProbability = _currentWeapon.explosionProbability;
        bullet.GetComponent<Bullet>().explosionRadius = _currentWeapon.explosionRadius;
        bullet.GetComponent<Bullet>().attack = _currentWeapon.weaponAttack;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Direction * _currentWeapon.bulletSpeed;
        }

        // Shooting the gun fire effect
        if (_currentWeapon.gunShootEffect != null)
        {
            GameObject gunFire = Instantiate(_currentWeapon.gunShootEffect, spawnPosition, transform.rotation);
            gunFire.transform.localScale = new Vector3(_player.playerDirection ? 1f : -1f, 1f, 1f);
        }

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

    private void ShootBullet()
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
                default:
                    break;
            }
            GameObject bullet = Instantiate(_currentWeapon.bulletPrefab, spawnPosition, transform.rotation);
            bullet.GetComponent<Bullet>().explosionProbability = _currentWeapon.explosionProbability;
            bullet.GetComponent<Bullet>().explosionRadius = _currentWeapon.explosionRadius;
            bullet.GetComponent<Bullet>().attack = _currentWeapon.weaponAttack;

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
            GameObject bullet = null;
            for (int i = 0; i < _numTrack; i++)
            {
                switch (_currentWeapon.weaponName)
                {
                    case "MaskinPistol":
                        AudioManager.Instance.PlayWeaponSFX("MaskinPistol");
                        break;
                    case "MachineGun":
                        AudioManager.Instance.PlayWeaponSFX("MachineGun");
                        break;
                    default:
                        break;
                }
                float angle = startAngle + angleStep * i;
                Vector3 bulletDirection = Quaternion.Euler(0, 0, angle) * Direction;
                bullet = Instantiate(_currentWeapon.bulletPrefab, spawnPosition, transform.rotation);
                bullet.GetComponent<Bullet>().explosionProbability = _currentWeapon.explosionProbability;
                bullet.GetComponent<Bullet>().explosionRadius = _currentWeapon.explosionRadius;
                bullet.GetComponent<Bullet>().attack = _currentWeapon.weaponAttack;

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.velocity = bulletDirection * _currentWeapon.bulletSpeed;
                    rb.mass = 0.3f;
                }
            }
            if (_currentWeapon.weaponName == "TripleGun")
            {
                AudioManager.Instance.PlayWeaponSFX("TripleGun");
            }
        }

        // Shooting the gun fire effect
        if (_currentWeapon.gunShootEffect != null)
        {
            GameObject gunFire = Instantiate(_currentWeapon.gunShootEffect, spawnPosition, transform.rotation);
            gunFire.transform.localScale = new Vector3(_player.playerDirection ? 1f : -1f, 1f, 1f);
        }

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

    private void ActivateFlameBullet()  // all the changes here are only refreshed one time after pushing the S key
    {
        Vector3 Direction = _player.playerDirection ? Vector3.right : Vector3.left;
        Vector3 spawnPosition = transform.position + Direction * _bulletSpawnDistance;

        if (_flameBullet == null)
        {
            // initialize the flame bullet
            _flameBullet = Instantiate(_currentWeapon.bulletPrefab, spawnPosition, transform.rotation);
            _flameBullet.GetComponent<Bullet>().explosionProbability = _currentWeapon.explosionProbability; ;
            _flameBullet.GetComponent<Bullet>().explosionRadius = _currentWeapon.explosionRadius;
            _flameBullet.GetComponent<Bullet>().attack = _currentWeapon.weaponAttack;
            _AimPositionOffsetRecord = _flameBullet.transform.Find("Aim").localPosition;

        }
        _flameBullet.SetActive(true);

        // initialize the aim position
        _oldAimPosition = _AimPositionOffsetRecord;

        //play the weapon switch SFX
        AudioManager.Instance.PlayWeaponSFX("FireGun");

    }

    private void UpdateFlameBullet()
    {
        Vector3 Direction = _player.playerDirection ? Vector3.right : Vector3.left;
        Vector3 spawnPosition = transform.position + Direction * _bulletSpawnDistance;
        _flameBullet.transform.localScale = new Vector3(_player.playerDirection ? 1f : -1f, 1f, 1f);
        _flameBullet.transform.position = spawnPosition;

        // Shooting the gun fire effect
        if (_currentWeapon.gunShootEffect != null)
        {
            GameObject gunFire = Instantiate(_currentWeapon.gunShootEffect, spawnPosition, transform.rotation);
            gunFire.transform.localScale = new Vector3(_player.playerDirection ? 1f : -1f, 1f, 1f);
        }

        // play the flame bullet VFX
        VisualEffect flameVFX = _flameBullet.GetComponent<VisualEffect>();
        flameVFX.SetBool("IsRight", Direction.x == 1);


        if (Time.time > _lastShellEjectTime + _shellEjectInterval)
        {
            // Ejecting the shell
            Vector3 shellspawnPositon = transform.position + (-1) * Direction * _bulletSpawnDistance;
            GameObject shell = Instantiate(_currentWeapon.shellPrefab, shellspawnPositon, Quaternion.identity);
            Rigidbody2D shellRb = shell.GetComponent<Rigidbody2D>();
            if (shellRb != null)
            {
                shellRb.mass = 0.5f;
                _shellEjectDirection.x = Mathf.Abs(_shellEjectDirection.x) * Direction.x * (-1);
                shellRb.velocity = _shellEjectDirection;
                shellRb.isKinematic = false;
            }
            _lastShellEjectTime = Time.time; // update the last shell eject time
        }

        // Recoil
        _player.gameObject.GetComponent<Marcus>()._isRecoil = true;

        // Aim
        //_flameBullet.transform.Find("Aim").localScale = new Vector3(_player.playerDirection ? 1f : -1f, 1f, 1f);
        StartCoroutine(MoveAimAfterDelay(_flameBullet.transform.Find("Aim")));
    }

    private IEnumerator MoveAimAfterDelay(Transform aimTransform)
    {
        if (aimTransform == null) yield break;
        //Vector3 targetPosition = _oldAimPosition + _flameBullet.transform.position;
        Vector3 adjustedAimPosition = _oldAimPosition;
        if (_flameBullet.transform.localScale.x < 0)
        {
            adjustedAimPosition.x = -adjustedAimPosition.x;
        }
        Vector3 targetPosition = adjustedAimPosition + _flameBullet.transform.position;

        float elapsedTime = 0;
        float updateInterval = 1f;
        while (elapsedTime < updateInterval)
        {
            aimTransform.position = Vector3.Lerp(aimTransform.position, targetPosition, elapsedTime / updateInterval);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        aimTransform.position = targetPosition;
    }


    private void DeactivateFlameBullet()
    {
        if (_flameBullet != null)
        {
            //stop coroutines
            StopAllCoroutines();
            // stop the flame bullet VFX
            VisualEffect flameVFX = _flameBullet.GetComponent<VisualEffect>();
            if (flameVFX != null)
            {
                flameVFX.Stop();
            }
            _flameBullet.SetActive(false);
        }
    }

}

