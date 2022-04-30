using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]
public class uiSounds : MonoBehaviour
{
    public AudioSource source;
    public AudioClip hover;
    public AudioClip select;


    [Header("Accessibility Sounds")]
    public AudioClip bgmClip;
    public AudioClip brightClip;
    public AudioClip creditsClip;
    public AudioClip MasterVolClip;
    public AudioClip multiPlayerClip;
    public AudioClip PlaybuttonClip;
    public AudioClip quitClip;
    public AudioClip settingsClip;
    public AudioClip sfxClip;
    public AudioClip tutClip;
    public AudioClip returnClip;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    void Update()
    {

    }
    public void playHoverSound()
    {
        source.Stop();
        source.clip = hover;
        source.Play();
    }
    public void playSelectSound()
    {
        source.Stop();
        source.clip = select;
        source.Play();
    }
    // Whole Lotta Jank
    public void playBGMClip()
    {
        source.Stop();
        source.clip = bgmClip;
        source.Play();
    }
    public void playBrightnessClip()
    {
        source.Stop();
        source.clip = brightClip;
        source.Play();
    }
    public void playCreditsClip()
    {
        source.Stop();
        source.clip = creditsClip;
        source.Play();
    }
    public void playMasterClip()
    {
        source.Stop();
        source.clip = MasterVolClip;
        source.Play();
    }
    public void playMultiplayerClip()
    {
        source.Stop();
        source.clip = multiPlayerClip;
        source.Play();
    }
    public void playPlayButtonClip()
    {
        source.Stop();
        source.clip = PlaybuttonClip;
        source.Play();
    }
    public void playQuitClip()
    {
        source.Stop();
        source.clip = quitClip;
        source.Play();
    }
    public void playSettingsClip()
    {
        source.Stop();
        source.clip = settingsClip;
        source.Play();
    }
    public void playSFXClip()
    {
        source.Stop();
        source.clip = sfxClip;
        source.Play();
    }
    public void playTutClip()
    {
        source.Stop();
        source.clip = tutClip;
        source.Play();
    }
    public void playReturnClip()
    {
        source.Stop();
        source.clip = returnClip;
        source.Play();
    }
}
