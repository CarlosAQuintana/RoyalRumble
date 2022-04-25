using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public roundManager roundManager;
    public bool playGame = false; // Determines if game is being played.
    public void beginPlay() // Initiated when "Play" button is pressed.
    {
        if (roundManager.playerCount < 2 || playGame)
            return;
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
        roundManager.currentRound = 1;
        roundManager.currentRoundState = roundManager.roundState.gameBegin;
        roundManager.roundStateController();
    }
}
