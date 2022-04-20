using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    PlayerControls controls;
    public GameObject pauseScreen;
    public bool paused;
    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Enable();
        controls.Player.Pause.performed += pause;
    }
    void Start()
    {
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
        paused = false;
    }
    void Update()
    {

    }
    public void pause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!paused)
            {
                pauseScreen.SetActive(true);
                paused = true;
            }
            else if (paused)
            {
                pauseScreen.SetActive(false);
                paused = false;
            }
        }
    }
    public void uiPause()
    {
        if (paused)
        {
            pauseScreen.SetActive(false);
            paused = false;
        }
    }
    public void quitToMenu(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void uiQuitToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
