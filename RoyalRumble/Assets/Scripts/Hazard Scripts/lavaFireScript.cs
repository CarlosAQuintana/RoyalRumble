using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lavaFireScript : MonoBehaviour
{
    Rigidbody rb;
    private float timeActive;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        timeActive = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject gameManager = GameObject.FindWithTag("gameManager");  //Gets the hazard and round manager scripts
        hazardManager hM = gameManager.GetComponent<hazardManager>();
        roundManager rM = gameManager.GetComponent<roundManager>();

        rb.velocity = new Vector3(0, 3, 0);
        timeActive -= Time.deltaTime;

        if (timeActive < 0)
        {
            rb.velocity = new Vector3(0, -3, 0);
            timeActive += Time.deltaTime;
        }
        if (transform.position.y < -185)
        {
            Destroy(gameObject);
        }



    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            GameObject player = GameObject.FindWithTag("Player");
            PlayerController pController = player.GetComponent<PlayerController>();     //Gets the controller of the player who fell'
            combatController pCombat = player.GetComponent<combatController>(); // Get the combat controller.
            pCombat.killPlayer(pCombat, pController);
        }
    }
}
