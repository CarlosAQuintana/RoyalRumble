using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pauseMenu : MonoBehaviour
{
    PlayerControls controls;

    roundManager rm;
    gameManager gm;

    public GameObject pauseScreen;
    public Button firstSelect;
    public Button playButton;
    public Button replayButton;
    public bool paused;
    public bool inTutorial;
    private void Awake()
    {
        controls = new PlayerControls();
        controls.UI.Enable();
        controls.UI.Pause.performed += pause;
    }
    void Start()
    {
        gm = GetComponent<gameManager>();
        rm = GetComponent<roundManager>();

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
                Time.timeScale = 0f;
                pauseScreen.SetActive(true);
                firstSelect.Select();
                paused = true;
            }
            else if (paused)
            {
                Time.timeScale = 1f;
                pauseScreen.SetActive(false);
                paused = false;
                if (inTutorial)
                {
                    GetComponent<tutorialManager>().yesButton.Select();
                }
                if (gm == null)
                    return;
                if (!gm.playGame)
                {
                    playButton.Select();
                }
                else if (rm.gameOver)
                {
                    replayButton.Select();
                }
            }
        }
    }
    public void uiPause()
    {
        if (paused)
        {
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
            paused = false;
            if (inTutorial)
            {
                GetComponent<tutorialManager>().yesButton.Select();
            }
            if (gm == null)
                return;
            if (!gm.playGame)
            {
                playButton.Select();
            }
            else if (rm.gameOver)
            {
                replayButton.Select();
            }
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
