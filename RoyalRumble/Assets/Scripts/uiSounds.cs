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
}
