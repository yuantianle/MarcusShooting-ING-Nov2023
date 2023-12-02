using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    public static AudioManager Instance; //singleton
    //SFX & BGM
    public AudioClip jump, fall, land, bgm;
    public AudioClip pistol;//, maskinPistol, machineGun, fireGun, tripleGun;
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
                SoundObjectCreation(jump, "Jump", 0.6f);
                break;
            case "Fall":
                SoundObjectCreation(fall, "Fall", 0.6f);
                break;
            default:
                break;
        }
    }

    public void PlayWeaponSFX(string sfxName)
    {
        switch (sfxName)
        {
            case "Pistol":
                SoundObjectCreation(pistol, "Pistol", 0.4f);
                break;
            default:
                break;
        }
    }

    private void SoundObjectCreation(AudioClip clip, string name, float volume)
    {
        GameObject soundObject = Instantiate(this.soundObject, transform);
        soundObject.GetComponent<AudioSource>().clip = clip;        
        soundObject.GetComponent<AudioSource>().name = name;
        soundObject.GetComponent<AudioSource>().volume = volume;
        soundObject.GetComponent<AudioSource>().Play();

    }

}
