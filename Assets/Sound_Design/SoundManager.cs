using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource musicSource;
    public AudioSource SFXSource;
    [SerializeField] AudioMixer mixer;
    public SoundManager instance;

    [Header("Audio Clip")]
    public AudioClip CombatMusic;
    public AudioClip Walk;
    public AudioClip JumpStart;
    public AudioClip JumpLand;
    public AudioClip Attack;
    public AudioClip Attack2;
    public AudioClip Attack3;
    public AudioClip Wind;

    [Header("VolumeSliders")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {


        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.onValueChanged.AddListener(ChangeSFXVolume);
        }
    }
    public void Update()
    {
        if (GameManager.GetInstance().GetGameState() != GAME_STATE.EXPLORATION)
        {
            musicSource.clip = CombatMusic;
            musicSource.Play();
        }

    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void ChangeMasterVolume(float sliderValue)
    {
        // Convertimos el rango del slider (0 a 1) a un rango de dB (-80 a 0)
        float volumeInDB = (1 - Mathf.Sqrt(sliderValue)) * -80f;
        mixer.SetFloat("MasterVolume", volumeInDB);
        Debug.Log($"Volumen ajustado a: {volumeInDB} dB con valor del slider: {sliderValue}");
    }

    public void ChangeMusicVolume(float sliderValue)
    {
        // Convertimos el rango del slider (0 a 1) a un rango de dB (-80 a 0)
        float volumeInDB = (1 - Mathf.Sqrt(sliderValue)) * -80f;
        mixer.SetFloat("MusicVolume", volumeInDB);
        Debug.Log($"Volumen ajustado a: {volumeInDB} dB con valor del slider: {sliderValue}");
    }

    public void ChangeSFXVolume(float sliderValue)
    {
        // Convertimos el rango del slider (0 a 1) a un rango de dB (-80 a 0)
        float volumeInDB = (1 - Mathf.Sqrt(sliderValue)) * -80f;
        mixer.SetFloat("SFXVolume", volumeInDB);
        Debug.Log($"Volumen ajustado a: {volumeInDB} dB con valor del slider: {sliderValue}");
    }
}
