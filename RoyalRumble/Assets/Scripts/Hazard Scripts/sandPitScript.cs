using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sandPitScript : MonoBehaviour
{
    private float slowdownTimer;
    // Start is called before the first frame update
    void Start()
    {
        slowdownTimer = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider hit)
    {
        if(hit.tag == ("Player"))
        {
            GameObject player = hit.gameObject;
            PlayerController pController = player.GetComponent<PlayerController>(); // Gets the controller of the player who fell.
            combatController pCombat = player.GetComponent<combatController>(); // Get the combat controller.
            GameObject roundmanager = GameObject.FindWithTag("gameManager");
            roundManager rManager = FindObjectOfType<roundManager>(); //And gets the Round Manager
            pCombat.goSpearDash = false;
            pController.playerSpeed = 0.5f;

            slowdownTimer -= Time.deltaTime;
            if(slowdownTimer < 0)
            {
                pCombat.killPlayer(pCombat, pController);
                rManager.numOfPlayersAlive -= 1;  //Reduces num of players alive by one
                rManager.playerIsDead[pController.playerID] = true;  //Set dead player as dead
                rManager.checkForRoundWin();  //Checks for the end of the round
            }
        }
    }

    void OnTriggerExit(Collider hit)
    {
        if(hit.tag == ("Player"))
        {
            GameObject player = hit.gameObject;
            PlayerController pController = player.GetComponent<PlayerController>(); // Gets the controller of the player who fell.
            combatController pCombat = player.GetComponent<combatController>(); // Get the combat controller.
            GameObject roundmanager = GameObject.FindWithTag("gameManager");
            roundManager rManager = FindObjectOfType<roundManager>(); //And gets the Round Manager

            pController.playerSpeed = 2.0f;
            pCombat.goSpearDash = true;
            slowdownTimer = 5;
        }
    }
}
