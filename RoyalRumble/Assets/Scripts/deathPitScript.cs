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

    void OnTriggerEnter(Collider hit)
    {
        if (hit.tag == ("Player"))
        {
            GameObject player = GameObject.FindWithTag("Player");
            PlayerController pController = player.GetComponent<PlayerController>();     //Gets the controller of the player who fell'
            combatController pCombat = player.GetComponent<combatController>(); // Get the combat controller.
            GameObject roundmanager = GameObject.FindWithTag("gameManager");
            roundManager rManager = roundmanager.GetComponent<roundManager>();  //And gets the Round Manager
            pController.canMove = false;    //Freezes fallen player's movement

            pCombat.StopCoroutine("spearAttack"); // If player is mid spear attack, stop the attack.
            pCombat.goSpearDash = false;

            rManager.numOfPlayersAlive -= 1;  //Reduces num of players alive by one
            rManager.playerIsDead[pController.playerID] = true;  //Set dead player as dead
            rManager.checkForRoundWin();  //Checks for the end of the round
        }
    }
}
