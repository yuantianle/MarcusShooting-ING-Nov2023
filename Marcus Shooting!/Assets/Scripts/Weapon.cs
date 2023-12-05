using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class Weapon: MonoBehaviour
{
    [SerializeField] private string _weaponName = "Weapon";
    [SerializeField] private float _bulletSpeed = 20f;
    [SerializeField] private float _bulletsPerSecond = 10f;
    [SerializeField] private float _cameraShakeDuration = 0.1f;
    [SerializeField] private float _cameraShakeMagnitude = 0.5f;
    [SerializeField] private float _weight = 1f;  // 0-10, 0 is lightest, 10 is heaviest
    [SerializeField] private int _numTrack = 1; // number of tracks of bullets
    [SerializeField] private int _recoilForce = 10; // recoil force
    [SerializeField] private float _explosionProbability = 0f; // probability of explosion: 0-1
    [SerializeField] private float _explosionRadius = 0f; // explosion radius
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Vector3 _gunFirePositionOffset = Vector3.zero;
    [SerializeField] private GameObject _gunShootEffect;
    [SerializeField] private GameObject _shellPrefab;
    [SerializeField] private float _weaponAttack;

    public string weaponName { get { return _weaponName; } private set { _weaponName = value; } }
    public float bulletSpeed { get { return _bulletSpeed; } private set { _bulletSpeed = value; } }
    public float bulletsPerSecond { get { return _bulletsPerSecond; } private set { _bulletsPerSecond = value; } }
    public float cameraShakeDuration { get { return _cameraShakeDuration; } private set { _cameraShakeDuration = value; } }
    public float cameraShakeMagnitude { get { return _cameraShakeMagnitude; } private set { _cameraShakeMagnitude = value; } }
    public float weight { get { return _weight; } private set { _weight = value; } }
    
    public int numTrack { get { return _numTrack; } private set { _numTrack = value; } }
    public int recoilForce { get { return _recoilForce; } private set { _recoilForce = value; } }

    public float explosionProbability { get { return _explosionProbability; } private set { _explosionProbability = value; } }
    public float explosionRadius { get { return _explosionRadius; } private set { _explosionRadius = value; } }

    public GameObject bulletPrefab { get { return _bulletPrefab; } private set { _bulletPrefab = value; } }

    public Vector3 gunFirePositionOffset { get { return _gunFirePositionOffset; } private set { _gunFirePositionOffset = value; } }

    public GameObject gunShootEffect { get { return _gunShootEffect; } private set { _gunShootEffect = value; } }

    public GameObject shellPrefab { get { return _shellPrefab; } private set { _shellPrefab = value; } }

    public float weaponAttack { get { return _weaponAttack; } private set { _weaponAttack = value; } }

}
