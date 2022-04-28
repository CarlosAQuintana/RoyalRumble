using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class tutorialManager : MonoBehaviour
{
    PlayerInputManager InputManager;
    PlayerController tutorialPlayer;
    [Header("References")]
    public Transform spawnPoint;
    public TextMeshProUGUI tutorialText;
    public GameObject TutorialPrompt;
    public GameObject tutorialBG;
    public Button yesButton;
    public AudioSource audioPlayer;
    [Header("Audio Clips")]
    public AudioClip tut01;
    public AudioClip tut02;
    public AudioClip tut03;
    public AudioClip tut04;
    void Start()
    {
        InputManager = GetComponent<PlayerInputManager>();
        yesButton.Select();
        tutorialText.text = ("");
    }
    void Update()
    {

    }
    void OnPlayerJoined(PlayerInput playerInput)
    {
        InputManager.DisableJoining();
        playerInput.gameObject.GetComponent<PlayerController>().startPos = spawnPoint.transform.position;
        combatController spawnedCombatant = playerInput.gameObject.GetComponent<combatController>();
        spawnedCombatant.inTutorial = true;
        spawnedCombatant.canAttack = true;
        PlayerController spawnedPLayer = playerInput.gameObject.GetComponent<PlayerController>();
        spawnedPLayer.playerID = playerInput.playerIndex;
        tutorialPlayer = spawnedPLayer;
        playerInput.gameObject.name = ("Player " + playerInput.playerIndex);
        StartCoroutine("quickEnableControl");
    }
    public IEnumerator quickEnableControl()
    {
        yield return new WaitForSeconds(0.25f);
        tutorialPlayer.canMove = true;
        tutorialPlayer.canControl = true;
    }
    public void AcceptTutorial()
    {
        TutorialPrompt.SetActive(false);
        StartCoroutine("tutorialAudio");
    }
    public void RejectTutorial()
    {
        tutorialBG.SetActive(false);
        TutorialPrompt.SetActive(false);
    }
    public IEnumerator tutorialAudio()
    {
        yield return new WaitForSeconds(.5f);
        updateTutorial("Press the 'action' button while unarmed to do a punch attack.", tut01);
        yield return new WaitForSeconds(4f);
        audioPlayer.Stop();
        updateTutorial("If you press the action button while armed you will do an action specific to your weapon!", tut02);
        yield return new WaitForSeconds(4.5f);
        audioPlayer.Stop();
        updateTutorial("Keep in mind each weapon can only be used a limited amount of times before you CAN'T do the action anymore...", tut03);
        yield return new WaitForSeconds(5.5f);
        audioPlayer.Stop();
        updateTutorial("At any time while you're armed you can choose to throw your weapon, but remember, once you throw your weapon, you LOSE IT!", tut04);
    }
    public void updateTutorial(string textToDisplay, AudioClip audioToPlay)
    {
        tutorialText.text = textToDisplay;
        audioPlayer.clip = audioToPlay;
        audioPlayer.Play();
    }
}
