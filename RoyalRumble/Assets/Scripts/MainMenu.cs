using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button PlayButton;
    public Button multiplayerButton;
    public Button creditsbackbutton;
    public Slider MasterVolume;
    public Slider brightness;
    public AudioSource bgmSource;
    void Start()
    {
        //bgmSource.Play();
    }

    void Awake()
    {


    }

    void Update()
    {
        //Screen.brightness = brightness.value;
        Debug.Log(Screen.brightness);
    }
    public void changeBrightness()
    {
        Screen.brightness = brightness.value;
    }
    public void loadGameScene()
    {
        SceneManager.LoadScene("Game Scene");
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0));
        // bgmSource.Stop();
    }
    public void loadTutorialScene()
    {
        SceneManager.LoadScene("Tutorial Scene");
    }
    public void quitToDesktop()
    {
        Application.Quit();
    }
    public void SelectPlayButton()
    {
        PlayButton.Select();
    }
    public void SelectMultiplayerMode()
    {
        multiplayerButton.Select();
    }
    public void SelectSlider()
    {
        MasterVolume.Select();
    }
    public void selectBackButton()
    {
        creditsbackbutton.Select();
    }
}
