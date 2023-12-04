using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    public static AudioManager Instance; //singleton
    //SFX & BGM
    public AudioClip jump, fall, land, walk, bgm;
    public AudioClip weaponSwitch, pistol, maskinPistol, machineGun, fireGun, tripleGun;
    public GameObject soundObject;
    public GameObject weaponSoundObject;
    private GameObject _bgmObject;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            _bgmObject = Instantiate(this.soundObject, transform);
            _bgmObject.GetComponent<AudioSource>().clip = bgm;
            _bgmObject.GetComponent<AudioSource>().loop = true;
            _bgmObject.GetComponent<AudioSource>().volume = 0.4f;
            _bgmObject.GetComponent<AudioSource>().pitch = 1f;
            _bgmObject.GetComponent<AudioSource>().name = "BGM";
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    void Start()
    {
        _bgmObject.GetComponent<AudioSource>().Play();
    }

    public void PlayMarcusSFX(string sfxName)
    {
        switch (sfxName)
        {
            case "Jump":
                SoundObjectCreation(jump, "Jump", 0.3f, 1f);
                break;
            case "Fall":
                SoundObjectCreation(fall, "Fall", 0.5f, 1f);
                break;
            case "Walk":
                SoundObjectCreation(walk, "Walk", 0.2f, 1f);
                //soundObject
                break;
            default:
                break;
        }
    }

    public void PlayWeaponSFX(string sfxName)
    {
        switch (sfxName)
        {
            case "WeaponSwitch":
                SoundObjectCreation(weaponSwitch, "Weapon Switch", 0.5f, 1f);
                break;
            case "Pistol":
                SoundObjectCreation(pistol, "Pistol", 0.5f, 1f);
                break;
            case "MaskinPistol":
                SoundObjectCreation(maskinPistol, "Maskin Pistol", 0.7f, 1f);
                break;
            case "MachineGun":
                SoundObjectCreation(machineGun, "Machine Gun", 0.6f, 1f);
                break;
            case "FireGun":
                SoundObjectCreation(fireGun, "Fire Gun", 0.5f, 1f);
                break;
            case "TripleGun":
                SoundObjectCreation(tripleGun, "Triple Gun", 0.5f, 1f);
                break;
            default:
                break;
        }
    }

    private void SoundObjectCreation(AudioClip clip, string name, float volume, float pitch, bool isloop = false)
    {
        GameObject soundObject = Instantiate(this.soundObject, transform);
        soundObject.GetComponent<AudioSource>().clip = clip;
        soundObject.GetComponent<AudioSource>().name = name;
        soundObject.GetComponent<AudioSource>().volume = volume;
        soundObject.GetComponent<AudioSource>().pitch = pitch;
        soundObject.GetComponent<AudioSource>().loop = isloop;
        soundObject.GetComponent<AudioSource>().Play();
    }

}
