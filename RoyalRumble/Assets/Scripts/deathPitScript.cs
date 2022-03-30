using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathPitScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter()
    {
        GameObject player = GameObject.FindWithTag("Player");
        PlayerController pController = player.GetComponent<PlayerController>();     //Gets the controller of the player who fell
        GameObject roundmanager = GameObject.FindWithTag("gameManager");
        roundManager rManager = roundmanager.GetComponent<roundManager>();  //And gets the Round Manager
        

        pController.canMove = false;    //Freezes fallen player's movement
        rManager.numOfPlayersAlive -= 1;  //Reduces num of players alive by one
        rManager.playerIsDead[pController.playerID] = true;  //Set dead player as dead
        rManager.checkForRoundWin();  //Checks for the end of the round





        
    }
}
