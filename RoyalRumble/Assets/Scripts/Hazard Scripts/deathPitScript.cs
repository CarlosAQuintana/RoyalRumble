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
            GameObject player = hit.gameObject;
            PlayerController pController = player.GetComponent<PlayerController>(); // Gets the controller of the player who fell.
            combatController pCombat = player.GetComponent<combatController>(); // Get the combat controller.
            GameObject roundmanager = GameObject.FindWithTag("gameManager");
            roundManager rManager = FindObjectOfType<roundManager>(); //And gets the Round Manager
            pCombat.StopAllCoroutines(); // If player is mid attack, stop the attack.
            pCombat.goSpearDash = false;
            pCombat.killPlayer(pCombat, pController);
        }
    }
}
