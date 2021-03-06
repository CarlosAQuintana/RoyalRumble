using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireHitboxScript : MonoBehaviour
{
    public float timeActive;

    // Start is called before the first frame update
    void Start()
    {
        timeActive = 1.5f;  //Sets the time the hitbox will be active to 1.5 seconds
    }

    // Update is called once per frame
    void Update()
    {
        GameObject gameManager = GameObject.FindWithTag("gameManager");  //Gets the hazard and round manager scripts
        hazardManager hM = gameManager.GetComponent<hazardManager>();
        roundManager rM = gameManager.GetComponent<roundManager>();

        timeActive -= Time.deltaTime;
        if(timeActive < 0)
        {
            Destroy(gameObject);  //Destroys the hitbox after the timer has expired
        }
    }

    void OnTriggerEnter (Collider hit)   //Checks to see if a player is hit by the blast, kills them if so
    {
        if(hit.tag == ("Player"))
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
