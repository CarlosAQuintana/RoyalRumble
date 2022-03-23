using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I reworked the game manager from the bottom as the old manager's
// functions are no longer needed with the current system.

public class gameManager : MonoBehaviour
{
    public roundManager roundManager;

    // Determines if game is being played.
    public bool playGame = false;

    // Initiated when "Play" button is pressed.
    public void beginPlay()
    {
        if (roundManager.playerCount == 0 || playGame)
            return;
        playGame = true;
        roundManager.playerIsDead = new bool[roundManager.playerCount];
        roundManager.playerScore = new int[roundManager.playerCount];
        roundManager.currentRoundState = roundManager.roundState.gameBegin;
        roundManager.roundStateController();
    }
}
