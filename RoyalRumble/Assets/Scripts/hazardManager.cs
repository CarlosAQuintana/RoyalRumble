using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hazardManager : MonoBehaviour
{
    public bool holeDeath;

    // Start is called before the first frame update
    void Start()
    {
        holeDeath = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Kills a player if they fall into the pit
        if(holeDeath == true)
        {
            combatController enemyCombat = GetComponent<combatController>();
            PlayerController enemyControl = GetComponent<PlayerController>();
            roundManager rManager = FindObjectOfType<roundManager>();
            rManager.numOfPlayersAlive--;
            rManager.playerIsDead[enemyControl.playerID] = true;
            enemyCombat.player.canMove = false;
            rManager.checkForRoundWin();

            holeDeath = false; //Resets the hazard so another it can kill another player
        }
    }
}
