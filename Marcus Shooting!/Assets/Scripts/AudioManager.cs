using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    public static AudioManager Instance; //singleton
    //SFX & BGM
    public AudioClip jump, fall, land, bgm;
    public GameObject soundObject;
    private GameObject _bgmObject;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            _bgmObject = Instantiate(this.soundObject, transform);
            _bgmObject.GetComponent<AudioSource>().clip = bgm;
            _bgmObject.GetComponent<AudioSource>().loop = true;
            _bgmObject.GetComponent<AudioSource>().volume = 0.3f;
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

    public void PlaySFX(string sfxName)
    {
        switch (sfxName)
        {
            case "Jump":
                SoundObjectCreation(jump, "Jump");
                break;
            case "Fall":
                SoundObjectCreation(fall, "Fall");
                break;
            //case "Run":
            //    SoundObjectCreation(run);
            //    break;
            //case "Land":
            //    SoundObjectCreation(land, "Land");
            //    break;
            //case "Shoot":
            //    SoundObjectCreation(shoot);
            //    break;
            default:
                break;
        }
    }

    private void SoundObjectCreation(AudioClip clip, string name)
    {
        GameObject soundObject = Instantiate(this.soundObject, transform);
        soundObject.GetComponent<AudioSource>().clip = clip;
        soundObject.GetComponent<AudioSource>().Play();
        soundObject.GetComponent<AudioSource>().name = name;
        _bgmObject.GetComponent<AudioSource>().volume = 0.5f;
    }

}
