using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class icicleSpikeScript : MonoBehaviour
{
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject gameManager = GameObject.FindWithTag("gameManager");  //Gets the hazard and round manager scripts
        hazardManager hM = gameManager.GetComponent<hazardManager>();
        roundManager rM = gameManager.GetComponent<roundManager>();

        if(hM.iceLaneSet == 1)
        {
            rb.velocity = new Vector3 (-20, 0, 0);
        }
        if(hM.iceLaneSet == 2)
        {
            rb.velocity = new Vector3 (20, 0, 0);
        }
        if(hM.iceLaneSet == 3)
        {
            rb.velocity = new Vector3 (-20, 0, 0);
        }
        if(hM.iceLaneSet == 4)
        {
            rb.velocity = new Vector3 (20, 0, 0);
        }
        if(hM.iceLaneSet == 5)
        {
            rb.velocity = new Vector3 (0, 0, -20);
        }

        if(transform.position.magnitude > 750.0f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter (Collision collision)
    {
        if(collision.collider.tag == "Player")
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
