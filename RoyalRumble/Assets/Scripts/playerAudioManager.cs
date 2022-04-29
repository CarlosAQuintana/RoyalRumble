using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAudioManager : MonoBehaviour
{
    public AudioSource audioS;
    [Header("Audio Clips")]
    public AudioClip grunt01;
    public AudioClip grunt02;
    public AudioClip attackGrunt01;
    public AudioClip attackGrunt02;
    public AudioClip throwGrunt;
    public AudioClip pDie;
    public AudioClip funnyThrowSound;
    void Start()
    {

    }
    void Update()
    {

    }
    public void playGruntSound()
    {
        audioS.Stop();
        audioS.clip = grunt01;
        audioS.Play();
    }
    public void plaAttackSound()
    {
        audioS.Stop();
        audioS.clip = attackGrunt01;
        audioS.Play();
    }
    public void playThrowSound()
    {
        audioS.Stop();
        audioS.clip = throwGrunt;
        audioS.Play();
    }
    public void playFunnyThrowSound()
    {
        audioS.Stop();
        audioS.clip = funnyThrowSound;
        audioS.Play();
    }
    public IEnumerator throwRelease()
    {
        playThrowSound();
        yield return new WaitForSeconds(.1f);
        playFunnyThrowSound();
    }
    public void playDeathSound()
    {
        audioS.Stop();
        audioS.clip = pDie;
        audioS.Play();
    }
}
