using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    const string mixerMaster = "MasterVolume";
    const string mixerBGM = "BGMVolume";
    const string mixerSFX = "SFXVolume";


    private void Awake()
    {
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }


    void SetMasterVolume(float value)
    {
        mixer.SetFloat(mixerMaster, Mathf.Log10(value) * 10);
    }

    void SetMusicVolume(float value)
    {
        mixer.SetFloat(mixerBGM, Mathf.Log10(value) * 10);

    }

    void SetSFXVolume(float value)
    {
        mixer.SetFloat(mixerSFX, Mathf.Log10(value) * 10);

    }
}
