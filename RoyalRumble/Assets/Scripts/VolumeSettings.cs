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

    private void Start()
    {
        UpdateUIOnStart();
    }

    private void UpdateUIOnStart()
    {
        UpdateSlider(masterSlider, mixerMaster);
        UpdateSlider(musicSlider, mixerBGM);
        UpdateSlider(sfxSlider, mixerSFX);
    }

    private void UpdateSlider(Slider uiSlider, string mixerSlider)
    {
        float mixerSliderDB = 0;

        bool results = mixer.GetFloat(mixerSlider, out mixerSliderDB);

        //float convertedValue = DecibelToLinear(mixerSliderDB);
        mixerSliderDB = DecibelToLinear(mixerSliderDB);

        uiSlider.value = mixerSliderDB;
    }


    private float DecibelToLinear(float dB)
    {
        float linear = Mathf.Pow(10.0f, dB / 20.0f);

        return linear;
    }


    void SetMasterVolume(float value)
    {
        mixer.SetFloat(mixerMaster, Mathf.Log10(value) * 20);
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
