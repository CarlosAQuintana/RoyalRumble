using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I reworked the game manager from the bottom as the old manager's
// functions are no longer needed with the current system.

public class gameManager : MonoBehaviour
{
    // Determines if game is being played.
    public bool playGame = false;


    // Initiated when "Play" button is pressed.
    public void beginPlay()
    {
        playGame = true;
    }
}
