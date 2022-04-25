using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button PlayButton;
    public Button multiplayerButton;
    public Slider MasterVolume;
    void Start()
    {

    }
    void Update()
    {

    }
    public void loadGameScene()
    {
        SceneManager.LoadScene("Game Scene");
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
}
