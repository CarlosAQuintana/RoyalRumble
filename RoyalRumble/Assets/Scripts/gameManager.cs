using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public roundManager roundManager;

    public GameObject playButton;
    public GameObject replayButton;
    public GameObject joinText;

    public Button play;
    public Button replay;

    public AudioClip startGameSound;

    public bool playGame = false; // Determines if game is being played.

    private void Start()
    {
        play.Select();
    }

    public void beginPlay() // Initiated when "Play" button is pressed.
    {
        if (roundManager.playerCount < 2 || playGame)
            return;

        disablePlayButton();
        playGame = true;
        roundManager.playerIsDead = new bool[roundManager.playerCount];
        roundManager.playerScore = new int[roundManager.playerCount];

        roundManager.players = new PlayerController[roundManager.playerCount];
        for (int p = 0; p < roundManager.players.Length; p++)
        {
            roundManager.players[p] = GameObject.Find("Player " + p).GetComponent<PlayerController>();
        }
        roundManager.combatControllers = new combatController[roundManager.playerCount];
        for (int c = 0; c < roundManager.combatControllers.Length; c++)
        {
            roundManager.combatControllers[c] = GameObject.Find("Player " + c).GetComponent<combatController>();
        }
        roundManager.swagControllers = new playerSwagController[roundManager.playerCount];
        for (int s = 0; s < roundManager.swagControllers.Length; s++)
        {
            roundManager.swagControllers[s] = GameObject.Find("Player " + s).GetComponent<playerSwagController>();
        }
        roundManager.currentRound = 1;
        roundManager.currentRoundState = roundManager.roundState.gameBegin;
        roundManager.roundStateController();
        AudioSource.PlayClipAtPoint(startGameSound, transform.position, 2.5f);
    }
    public void disablePlayButton()
    {
        playButton.SetActive(false);
        replayButton.SetActive(false);
        joinText.SetActive(false);
    }
    public void enablePlayButton()
    {
        replayButton.SetActive(true);
        replay.Select();
    }
}
