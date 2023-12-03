using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager _instance;
    private GameObject _player;
    public List<GameObject> weaponPrefabs;  // store the weapon prefabs
    public List<GameObject> _instantiatedWeapons = new List<GameObject>(); // store the instantiated weapons
    private List<Vector3> _weaponOffsets = new List<Vector3>(); //strore the local position of each weapon relative to the player
    private List<Vector3> _weaponPreviousPositions = new List<Vector3>();
    public int _currentWeaponIndex = 0;
    public float _jitterYAmplitude = 0.1f;
    public float _jitterYPeriod = 1f;

    void Awake()
    {
        _instance = this;
        _player = transform.parent.gameObject;
        foreach (var weaponPrefab in weaponPrefabs)
        {
            GameObject weapon = Instantiate(weaponPrefab, transform.position, Quaternion.identity, transform);
            weapon.SetActive(false);
            _instantiatedWeapons.Add(weapon);
            _weaponOffsets.Add(weaponPrefab.transform.localPosition);
            _weaponPreviousPositions.Add(transform.position);
        }

        _instantiatedWeapons[_currentWeaponIndex].SetActive(true);
        UpdateWeapon();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _instantiatedWeapons[_currentWeaponIndex].SetActive(false);
            _currentWeaponIndex = (_currentWeaponIndex + 1) % _instantiatedWeapons.Count;
            _instantiatedWeapons[_currentWeaponIndex].SetActive(true);
            AudioManager.Instance.PlayWeaponSFX("WeaponSwitch");
            UpdateWeapon();
        }
    }

    private void FixedUpdate()
    {
        // Use the tri function to control the gun up and down move with human animation breath 
        float currentJitterY = _jitterYAmplitude * Mathf.Cos(Time.time * (2 * Mathf.PI / _jitterYPeriod));

        // Weapon following weapon manager
        Weapon currentWeapon = _instantiatedWeapons[_currentWeaponIndex].GetComponent<Weapon>();
        float followSpeed = Mathf.Max(0.1f, 30f - currentWeapon.weight);
        Vector3 weaponOffset = _player.GetComponent<Marcus>().playerDirection == true ? _weaponOffsets[_currentWeaponIndex] : new Vector3(-_weaponOffsets[_currentWeaponIndex].x, _weaponOffsets[_currentWeaponIndex].y, _weaponOffsets[_currentWeaponIndex].z);
        Vector3 targetPosition = transform.position + weaponOffset + new Vector3(0, currentJitterY, 0);
        currentWeapon.transform.position = Vector3.Lerp(
            _weaponPreviousPositions[_currentWeaponIndex],
            targetPosition,
            followSpeed * Time.deltaTime
        );
        _weaponPreviousPositions[_currentWeaponIndex] = currentWeapon.transform.position;
    }

    // trigger to the child instance for weapon parameters update
    private void UpdateWeapon()
    {
        Weapon currentWeapon = _instantiatedWeapons[_currentWeaponIndex].GetComponent<Weapon>();
        _player.GetComponent<Marcus>().recoilForce = currentWeapon.GetComponent<Weapon>().recoilForce;
    }
}
