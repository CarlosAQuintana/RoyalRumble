using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    const string mixerBGM = "BGMVolume";
    const string mixerSFX = "SFXVolume";
    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }


    void SetMusicVolume(float value)
    {
        mixer.SetFloat(mixerBGM, Mathf.Log10(value) * 20);

    }

    void SetSFXVolume(float value)
    {
        mixer.SetFloat(mixerSFX, Mathf.Log10(value) * 20);

    }
}
